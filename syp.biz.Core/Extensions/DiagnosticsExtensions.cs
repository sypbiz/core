using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace syp.biz.Core.Extensions
{
    /// <summary>
    /// A collection of diagnostics extension methods
    /// </summary>
    public static class DiagnosticsExtensions
    {
        /// <summary>
        /// Measures the duration it takes to perform <paramref name="action"/>.
        /// </summary>
        /// <param name="stopwatch">The <see cref="Stopwatch"/> to use.</param>
        /// <param name="action">The <see cref="Action"/> to measure the duration of.</param>
        /// <returns>A <see cref="TimeSpan"/> representing the duration the action took.</returns>
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

        /// <summary>
        /// Asynchronously measures the duration it takes to perform <paramref name="asyncAction"/>.
        /// </summary>
        /// <param name="stopwatch">The <see cref="Stopwatch"/> to use.</param>
        /// <param name="asyncAction">The asynchronous task to measure the duration of.</param>
        /// <returns>A <see cref="TimeSpan"/> representing the duration the action took.</returns>
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

        /// <summary>
        /// Measures the duration it takes to perform <paramref name="action"/>.
        /// </summary>
        /// <param name="stopwatch">The <see cref="Stopwatch"/> to use.</param>
        /// <param name="action">The <see cref="Action"/> to measure the duration of.</param>
        /// <param name="duration">A <see cref="TimeSpan"/> representing the duration the action took.</param>
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

        /// <summary>
        /// Measures the duration it takes to perform <paramref name="action"/>.
        /// </summary>
        /// <param name="stopwatch">The <see cref="Stopwatch"/> to use.</param>
        /// <param name="action">The <see cref="Action"/> to measure the duration of.</param>
        /// <param name="onDone">A callback delegate to call with the <see cref="TimeSpan"/> duration the action took.</param>
        public static void Measure(this Stopwatch stopwatch, Action action, Action<TimeSpan> onDone)
        {
            stopwatch.Measure(action, out var duration);
            onDone(duration);
        }

        /// <summary>
        /// Measures the duration it takes to perform <paramref name="func"/>.
        /// </summary>
        /// <typeparam name="T">The return type of <paramref name="func"/>.</typeparam>
        /// <param name="stopwatch">The <see cref="Stopwatch"/> to use.</param>
        /// <param name="func">The <see cref="Func{T}"/> to measure the duration of.</param>
        /// <param name="duration">A <see cref="TimeSpan"/> representing the duration the action took.</param>
        /// <returns>The result of <paramref name="func"/>.</returns>
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

        /// <summary>
        /// Measures the duration it takes to perform <paramref name="func"/>.
        /// </summary>
        /// <typeparam name="T">The return type of <paramref name="func"/>.</typeparam>
        /// <param name="stopwatch">The <see cref="Stopwatch"/> to use.</param>
        /// <param name="func">The <see cref="Func{T}"/> to measure the duration of.</param>
        /// <param name="onDone">A callback delegate to call with the <see cref="TimeSpan"/> duration the action took.</param>
        /// <returns>The result of <paramref name="func"/>.</returns>
        public static T Measure<T>(this Stopwatch stopwatch, Func<T> func, Action<TimeSpan> onDone)
        {
            var rslt = stopwatch.Measure(func, out var duration);
            onDone(duration);
            return rslt;
        }
    }
}
