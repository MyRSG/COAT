using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using COAT.Models;
using Timer = System.Timers.Timer;

namespace COAT.Schedule
{
    public class ScheduleTaskRunner
    {
        private const double Duration = 2*60*1000;
        private readonly COATEntities _db = new COATEntities();


        private readonly object _locker = new object();

        private readonly Queue<IScheduleTask> _tasks = new Queue<IScheduleTask>();
        private readonly ScheduleThreadPool _threadPool = new ScheduleThreadPool();
        private readonly Timer _timer;

        public ScheduleTaskRunner()
        {
            _timer = new Timer(Duration) {AutoReset = true};
            _timer.Elapsed += TimerElapsed;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            GetUnEnqueueTask();
            TryStart();
        }

        private void GetUnEnqueueTask()
        {
            List<int> ids = _tasks.Select(t => t.Id).ToList();
            foreach (
                COATImportTask task in
                    _db.ImportTasks.Where(t => !ids.Contains(t.Id)).Select(i => new COATImportTask(i))
                )
            {
                _tasks.Enqueue(task);
            }
        }

        public void AddTask(IScheduleTask task)
        {
            lock (_locker)
            {
                if (task.Name == COATImportTask.ImportTaskName)
                {
// ReSharper disable SuspiciousTypeConversion.Global
                    _db.ImportTasks.AddObject((ImportTask) task);
// ReSharper restore SuspiciousTypeConversion.Global
                    _db.SaveChanges();
                }

                _tasks.Enqueue(task);
            }

            TryStart();
        }

        public void TryStart()
        {
            lock (_locker)
            {
                if (_tasks.Count == 0)
                    return;

                Thread th = _threadPool.GetThread(RunTask);
                if (th == null)
                    return;

                IScheduleTask task = _tasks.Dequeue();
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
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                task.Error(ex.Message);
                _db.SaveChanges();

                if (task.RunTimes < 3)
                {
                    task.RunTimes++;
                    _tasks.Enqueue(task);
                }
            }
        }
    }
}