using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace COAT.Schedule
{
    public class ScheduleThreadPool
    {
        private readonly List<Thread> _threadPool = new List<Thread>();

        public ScheduleThreadPool()
        {
            MaxThread = 20;
        }

        public int MaxThread { get; set; }

        public int AvaliableThreadCount
        {
            get { return _threadPool.Count(t => !IsState(t, ThreadState.Running) && !IsState(t, ThreadState.WaitSleepJoin)); }
        }

        public Thread GetThread(ParameterizedThreadStart start)
        {
            return GetAvaliableThread(start) ?? CreateThread(start);
        }

        public Thread GetAvaliableThread(ParameterizedThreadStart start)
        {
            return
                _threadPool.FirstOrDefault(
                    th =>
                    IsState(th, ThreadState.Stopped) || IsState(th, ThreadState.Aborted) ||
                    IsState(th, ThreadState.Unstarted));
        }

        public Thread CreateThread(ParameterizedThreadStart start)
        {
            if (AvaliableThreadCount >= MaxThread)
                return null;
            var th = new Thread(start) {IsBackground = true};
            _threadPool.Add(th);

            return th;
        }

        protected bool IsState(Thread th, ThreadState state)
        {
            return (th.ThreadState & state) == state;
        }
    }
}