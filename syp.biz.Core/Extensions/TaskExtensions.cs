using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using syp.biz.Core.Helpers;
using syp.biz.Core.Logging;

namespace syp.biz.Core.Extensions
{
    public static class TaskExtensions
    {
        private static readonly Task<bool> CachedCompletedTrueTask = Task.FromResult(true);
        private static readonly Task<bool> CachedCompletedFalseTask = Task.FromResult(true);

        public static Task<bool> CompletedTrueTask(this TaskFactory taskFactory)
        {
            return CachedCompletedTrueTask;
        }

        public static Task<bool> CompletedFalseTask(this TaskFactory taskFactory)
        {
            return CachedCompletedFalseTask;
        }

        public static void OnSuccess<T>(this Task<T> task, Action<T> handler)
        {
            task.ContinueWith(t => handler(t.Result), TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public static void OnSuccess(this Task task, Action handler)
        {
            task.ContinueWith(t => handler(), TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public static void OnFail(this Task task, Action<AggregateException> handler)
        {
            task.ContinueWith(t => handler(t.Exception), TaskContinuationOptions.NotOnRanToCompletion);
        }

        public static void OnCancel(this Task task, Action<AggregateException> handler)
        {
            task.ContinueWith(t => handler(t.Exception), TaskContinuationOptions.OnlyOnCanceled);
        }

        public static void LogErrors(this Task task, ILogger logger, string message)
        {
            task.ContinueWith(t => logger.Error(message, t.Exception), TaskContinuationOptions.NotOnRanToCompletion);
        }

        public static void NoAwait(this Task task)
        {
            // do nothing
        }

        public static Task<bool> IsSuccessful(this Task task)
        {
            return task.ContinueWith(t => t.IsCompleted);
        }

        /// <summary>
        /// Waits on <paramref name="task"/>, ignoring any exceptions
        /// </summary>
        /// <param name="task">A task to wait on.</param>
        public static void SafeWait(this Task task) => Try.Ignore(task.Wait);

        /// <summary>
        /// Waits <paramref name="task"/>. The wait terminates if <paramref name="token"/> is canceled before the task completes.
        /// </summary>
        /// <param name="task">A task to wait on.</param>
        /// <param name="token">A cancellation token to observe while waiting for <paramref name="task"/> to complete.</param>
        public static void SafeWait(this Task task, CancellationToken token)
        {
            try
            {
                task.Wait(token);
            }
            catch
            {
                // do nothing
            }
        }

        [DebuggerStepThrough]
        public static Task StartNewSTA(this TaskFactory factory, Action action) => factory.StartNew(action, factory.CancellationToken, factory.CreationOptions, STATaskScheduler.Instance);

        [DebuggerStepThrough]
        public static Task<T> StartNewSTA<T>(this TaskFactory factory, Func<T> function) => factory.StartNew(function, factory.CancellationToken, factory.CreationOptions, STATaskScheduler.Instance);

        [DebuggerStepThrough]
        public static void ExecuteInSTA(this TaskFactory factory, Action action) => factory.StartNewSTA(action).Wait();

        [DebuggerStepThrough]
        public static T ExecuteInSTA<T>(this TaskFactory factory, Func<T> function) => factory.StartNewSTA(function).Result;
    }
}
