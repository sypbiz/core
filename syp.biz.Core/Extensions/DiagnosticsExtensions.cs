using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace syp.biz.Core.Extensions
{
    public static class DiagnosticsExtensions
    {
        public static TimeSpan Measure(this Stopwatch stopwatch, Action action)
        {
            try
            {
                stopwatch.Start();
                action();
            }
            finally
            {
                stopwatch.Stop();
            }

            return stopwatch.Elapsed;
        }

        public static async Task<TimeSpan> Measure(this Stopwatch stopwatch, Func<Task> asyncAction)
        {
            try
            {
                stopwatch.Start();
                await asyncAction();
            }
            finally
            {
                stopwatch.Stop();
            }

            return stopwatch.Elapsed;
        }

        public static void Measure(this Stopwatch stopwatch, Action action, out TimeSpan duration)
        {
            try
            {
                stopwatch.Start();
                action();
                stopwatch.Stop();
                duration = stopwatch.Elapsed;
            }
            catch
            {
                stopwatch.Stop();
                duration = stopwatch.Elapsed;
                throw;
            }
        }

        public static void Measure(this Stopwatch stopwatch, Action action, Action<TimeSpan> onDone)
        {
            stopwatch.Measure(action, out var duration);
            onDone(duration);
        }

        public static T Measure<T>(this Stopwatch stopwatch, Func<T> func, out TimeSpan duration)
        {
            try
            {
                stopwatch.Start();
                var rslt = func();
                stopwatch.Stop();
                duration = stopwatch.Elapsed;
                return rslt;
            }
            catch
            {
                stopwatch.Stop();
                duration = stopwatch.Elapsed;
                throw;
            }
        }

        public static T Measure<T>(this Stopwatch stopwatch, Func<T> func, Action<TimeSpan> onDone)
        {
            var rslt = stopwatch.Measure(func, out var duration);
            onDone(duration);
            return rslt;
        }
    }
}
