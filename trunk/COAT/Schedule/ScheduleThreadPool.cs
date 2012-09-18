using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;

namespace COAT.Schedule
{
    public class ScheduleThreadPool
    {
        List<Thread> threadPool = new List<Thread>();

        public int MaxThread { get; set; }

        public int AvaliableThreadCount
        {
            get
            {
                return threadPool.Count(t => !IsState(t, ThreadState.Running) && !IsState(t, ThreadState.WaitSleepJoin));
            }
        }

        public ScheduleThreadPool()
        {
            MaxThread = 20;
        }

        public Thread GetThread(ParameterizedThreadStart start)
        {
            return GetAvaliableThread(start) ?? CreateThread(start);
        }

        public Thread GetAvaliableThread(ParameterizedThreadStart start)
        {
            foreach (var th in threadPool)
            {
                if (IsState(th, ThreadState.Stopped) || IsState(th, ThreadState.Aborted) || IsState(th, ThreadState.Unstarted))
                    return th;
            }

            return null;
        }

        public Thread CreateThread(ParameterizedThreadStart start)
        {
            if (AvaliableThreadCount >= MaxThread)
                return null;
            var th = new Thread(start);
            th.IsBackground = true;
            threadPool.Add(th);

            return th;
        }

        protected bool IsState(Thread th, ThreadState state)
        {
            return (th.ThreadState & state) == state;
        }
    }
}