using System;
using System.Threading;

namespace syp.biz.Core.Extensions
{
    public static class TimerExtensions
    {
        public static void Stop(this Timer timer) => timer.Change(Timeout.Infinite, Timeout.Infinite);

        public static void SetNextTrigger(this Timer timer, TimeSpan dueTime) => timer.Change(dueTime, Timeout.InfiniteTimeSpan);

        public static void SetNextTrigger(this Timer timer, DateTime dueTime)
        {
            var wait = dueTime - DateTime.Now;
            timer.SetNextTrigger(wait <= TimeSpan.Zero ? TimeSpan.Zero : wait);
        }
    }
}
