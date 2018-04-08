using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using syp.biz.Core.Helpers;
using syp.biz.Core.Logging;

namespace syp.biz.Core.Extensions
{
    /// <summary>
    /// A collection of task related extension methods.
    /// </summary>
    public static class TaskExtensions
    {
        private static readonly Task<bool> CachedCompletedTrueTask = Task.FromResult(true);
        private static readonly Task<bool> CachedCompletedFalseTask = Task.FromResult(true);

        /// <summary>
        /// Returns a completed task with a <c>true</c> result.
        /// </summary>
        /// <param name="taskFactory">A task factory to use.</param>
        /// <returns>A completed task with a <c>true</c> result.</returns>
        public static Task<bool> CompletedTrueTask(this TaskFactory taskFactory) => CachedCompletedTrueTask;

        /// <summary>
        /// Returns a completed task with a <c>false</c> result.
        /// </summary>
        /// <param name="taskFactory">A task factory to use.</param>
        /// <returns>A completed task with a <c>false</c> result.</returns>
        public static Task<bool> CompletedFalseTask(this TaskFactory taskFactory) => CachedCompletedFalseTask;

        /// <summary>
        /// Adds a <paramref name="handler"/> to be executed when the <paramref name="task"/> completes successfully.
        /// </summary>
        /// <typeparam name="T">The task's returned result type.</typeparam>
        /// <param name="task">The task to continue upon.</param>
        /// <param name="handler">The handler to invoke when the task completes successfully.</param>
        public static void OnSuccess<T>(this Task<T> task, Action<T> handler) => task.ContinueWith(t => handler(t.Result), TaskContinuationOptions.OnlyOnRanToCompletion);

        /// <summary>
        /// Adds a <paramref name="handler"/> to be executed when the <paramref name="task"/> completes successfully.
        /// </summary>
        /// <param name="task">The task to continue upon.</param>
        /// <param name="handler">The handler to invoke when the task completes successfully.</param>
        public static void OnSuccess(this Task task, Action handler) => task.ContinueWith(t => handler(), TaskContinuationOptions.OnlyOnRanToCompletion);

        /// <summary>
        /// Adds a <paramref name="handler"/> to be executed when the <paramref name="task"/> fails.
        /// </summary>
        /// <param name="task">The task to continue upon.</param>
        /// <param name="handler">The handler to invoke when the task fails.</param>
        public static void OnFail(this Task task, Action<AggregateException> handler) => task.ContinueWith(t => handler(t.Exception), TaskContinuationOptions.NotOnRanToCompletion);

        /// <summary>
        /// Adds a <paramref name="handler"/> to be executed when the <paramref name="task"/> is canceled.
        /// </summary>
        /// <param name="task">The task to continue upon.</param>
        /// <param name="handler">The handler to invoke when the task is canceled.</param>
        public static void OnCancel(this Task task, Action<AggregateException> handler) => task.ContinueWith(t => handler(t.Exception), TaskContinuationOptions.OnlyOnCanceled);

        /// <summary>
        /// Logs an error if the tasks fails or is canceled.
        /// </summary>
        /// <param name="task">The task to continue upon.</param>
        /// <param name="logger">The logger to use.</param>
        /// <param name="message">A message to log with the error.</param>
        public static void LogErrors(this Task task, ILogger logger, string message) => task.ContinueWith(t => logger.Error(message, t.Exception), TaskContinuationOptions.NotOnRanToCompletion);

        /// <summary>
        /// Ignores the task's awaiter.
        /// </summary>
        /// <param name="task">The task to not await on.</param>
        public static void NoAwait(this Task task) { /* do nothing */ }

        /// <summary>
        /// Ignores the task's awaiter after configuring it.
        /// </summary>
        /// <param name="task">The task to not await on.</param>
        /// <param name="continueOnCapturedContext"><c>true</c> to attempt to marshal the continuation back to the original context captured; otherwise, <c>false</c>.</param>
        public static void NoAwait(this Task task, bool continueOnCapturedContext) => task.ConfigureAwait(continueOnCapturedContext);

        /// <summary>
        /// Creates a new task with providing the result (successful vs. failed or canceled) of the given <paramref name="task"/>.
        /// </summary>
        /// <param name="task">The task to check.</param>
        /// <returns><c>true</c> if <paramref name="task"/> completed successfully, otherwise <c>false</c>.</returns>
        public static Task<bool> IsSuccessful(this Task task) => task.ContinueWith(t => t.IsCompleted);

        /// <summary>
        /// Waits on <paramref name="task"/>, ignoring any exceptions.
        /// </summary>
        /// <param name="task">A task to wait on.</param>
        public static void SafeWait(this Task task) => Try.Ignore(task.Wait);

        /// <summary>
        /// Waits <paramref name="task"/>. The wait terminates if <paramref name="token"/> is canceled before the task completes.
        /// </summary>
        /// <param name="task">A task to wait on.</param>
        /// <param name="token">A cancellation token to observe while waiting for <paramref name="task"/> to complete.</param>
        public static void SafeWait(this Task task, CancellationToken token) => Try.Ignore(() => task.Wait(token));

        /// <summary>
        /// Executes <paramref name="action"/> as <see cref="ApartmentState.STA"/> on the <see cref="STATaskScheduler"/>.
        /// </summary>
        /// <param name="taskFactory">A task factory to use.</param>
        /// <param name="action">The action to execute.</param>
        /// <returns>The started task.</returns>
        [DebuggerStepThrough]
        public static Task StartNewSTA(this TaskFactory taskFactory, Action action) => taskFactory.StartNew(action, taskFactory.CancellationToken, taskFactory.CreationOptions, STATaskScheduler.Instance);

        /// <summary>
        /// Executes <paramref name="function"/> as <see cref="ApartmentState.STA"/> on the <see cref="STATaskScheduler"/>.
        /// </summary>
        /// <param name="taskFactory">A task factory to use.</param>
        /// <param name="function">The function to execute.</param>
        /// <returns>The started task.</returns>
        [DebuggerStepThrough]
        public static Task<T> StartNewSTA<T>(this TaskFactory taskFactory, Func<T> function) => taskFactory.StartNew(function, taskFactory.CancellationToken, taskFactory.CreationOptions, STATaskScheduler.Instance);

        /// <summary>
        /// Executes <paramref name="action"/> as <see cref="ApartmentState.STA"/> on the <see cref="STATaskScheduler"/>.
        /// </summary>
        /// <param name="taskFactory">A task factory to use.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that will be assigned to the new <see cref="Task"/>.</param>
        /// <returns>The started task.</returns>
        [DebuggerStepThrough]
        public static Task StartNewSTA(this TaskFactory taskFactory, Action action, CancellationToken cancellationToken) => taskFactory.StartNew(action, cancellationToken, taskFactory.CreationOptions, STATaskScheduler.Instance);

        /// <summary>
        /// Executes <paramref name="function"/> as <see cref="ApartmentState.STA"/> on the <see cref="STATaskScheduler"/>.
        /// </summary>
        /// <param name="taskFactory">A task factory to use.</param>
        /// <param name="function">The function to execute.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that will be assigned to the new <see cref="Task"/>.</param>
        /// <returns>The started task.</returns>
        [DebuggerStepThrough]
        public static Task<T> StartNewSTA<T>(this TaskFactory taskFactory, Func<T> function, CancellationToken cancellationToken) => taskFactory.StartNew(function, cancellationToken, taskFactory.CreationOptions, STATaskScheduler.Instance);

        /// <summary>
        /// Synchronously executes <paramref name="action"/> as <see cref="ApartmentState.STA"/> on the <see cref="STATaskScheduler"/>.
        /// </summary>
        /// <param name="taskFactory">A task factory to use.</param>
        /// <param name="action">The action to execute.</param>
        [DebuggerStepThrough]
        public static void ExecuteInSTA(this TaskFactory taskFactory, Action action) => taskFactory.StartNewSTA(action).Wait();

        /// <summary>
        /// Synchronously executes <paramref name="function"/> as <see cref="ApartmentState.STA"/> on the <see cref="STATaskScheduler"/>.
        /// </summary>
        /// <param name="taskFactory">A task factory to use.</param>
        /// <param name="function">The function to execute.</param>
        /// <returns>The result of <paramref name="function"/>.</returns>
        [DebuggerStepThrough]
        public static T ExecuteInSTA<T>(this TaskFactory taskFactory, Func<T> function) => taskFactory.StartNewSTA(function).Result;
    }
}
