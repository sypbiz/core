using System;
using System.Threading;

namespace syp.biz.Core.Extensions
{
    /// <summary>
    /// A collection of timer related extension methods.
    /// </summary>
    public static class TimerExtensions
    {
        /// <summary>
        /// Stops the <paramref name="timer"/>.
        /// </summary>
        /// <param name="timer">The timer to stop.</param>
        /// <remarks>
        /// Calls <see cref="Timer.Change(int,int)"/> with <see cref="Timeout.Infinite"/>.
        /// </remarks>
        public static void Stop(this Timer timer) => timer.Change(Timeout.Infinite, Timeout.Infinite);

        /// <summary>
        /// Sets the duration until the next time the <paramref name="timer"/> would trigger.
        /// </summary>
        /// <param name="timer">The timer to set.</param>
        /// <param name="dueTime">The duration until the next time the <paramref name="timer"/> should trigger.</param>
        public static void SetNextTrigger(this Timer timer, TimeSpan dueTime) => timer.Change(dueTime, Timeout.InfiniteTimeSpan);

        /// <summary>
        /// Sets the next time the <paramref name="timer"/> would trigger.
        /// </summary>
        /// <param name="timer">The timer to set.</param>
        /// <param name="dueTime">The next time the <paramref name="timer"/> should trigger. If in now or in the past, the <paramref name="timer"/> will be set to trigger immediately.</param>
        /// <remarks>
        /// Calculates the duration for the next trigger based on the delta between <see cref="DateTime.Now"/> and <paramref name="dueTime"/>.
        /// </remarks>
        public static void SetNextTrigger(this Timer timer, DateTime dueTime)
        {
            var wait = dueTime - DateTime.Now;
            timer.SetNextTrigger(wait <= TimeSpan.Zero ? TimeSpan.Zero : wait);
        }
    }
}
