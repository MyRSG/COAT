using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using COAT.Models;

namespace COAT.Schedule
{
    public class ScheduleTaskRunner
    {
        const int MAX_THREAD = 5;
        const double DURATION = 2 * 60 * 1000;


        System.Timers.Timer timer = null;
        object locker = new object();

        COATEntities db = new COATEntities();
        ScheduleThreadPool threadPool = new ScheduleThreadPool();
        Queue<IScheduleTask> tasks = new Queue<IScheduleTask>();

        public ScheduleTaskRunner()
        {
            timer = new System.Timers.Timer(DURATION);
            timer.AutoReset = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            GetUnEnqueueTask();
            TryStart();
        }

        private void GetUnEnqueueTask()
        {
            var ids = tasks.Select(t => t.Id).ToList();
            var names = tasks.Select(t => t.Name).ToList();
            foreach (var task in db.ImportTasks.Where(t => !ids.Contains(t.Id)).Select(i => new COATImportTask(i)))
            {
                tasks.Enqueue(task);
            }
        }

        public void AddTask(IScheduleTask task)
        {
            lock (locker)
            {
                if (task.Name == COATImportTask.ImportTaskName)
                {
                    db.ImportTasks.AddObject((ImportTask)task);
                    db.SaveChanges();
                }

                tasks.Enqueue(task);
            }

            TryStart();
        }

        public void TryStart()
        {
            lock (locker)
            {
                if (tasks.Count == 0)
                    return;

                var th = threadPool.GetThread(RunTask);
                if (th == null)
                    return;

                var task = tasks.Dequeue();
                th.Start(task);
            }
        }

        protected void RunTask(Object obj)
        {
            var task = obj as IScheduleTask;
            if (task == null)
                return;

            try
            {
                task.Run(null);
                task.Success();
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                task.Error(ex.Message);
                db.SaveChanges();

                if (task.RunTimes < 3)
                {
                    task.RunTimes++;
                    tasks.Enqueue(task);
                }
            }

        }


    }
}