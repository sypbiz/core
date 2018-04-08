// TODO: cleanup

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using syp.biz.Core.Extensions;

namespace syp.biz.Core.Helpers
{
    /// <summary>
    /// An <see cref="ApartmentState.STA"/> task scheduler.
    /// </summary>
    public sealed class STATaskScheduler : TaskScheduler, IDisposable
    {
        private static readonly int MaxThreads = (int) ConfigSTA.Current.MaxThreads;
        private static readonly int MinThreads = (int) ConfigSTA.Current.MinThreads;
        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();
        private readonly BlockingCollection<Task> _tasks = new BlockingCollection<Task>();
        private readonly List<Thread> _threads = new List<Thread>();

        private STATaskScheduler() => this.InitThreadPool(MinThreads);

        /// <summary>
        /// The instance of the <see cref="ApartmentState.STA"/> task scheduler.
        /// </summary>
        public static STATaskScheduler Instance { get; } = new STATaskScheduler();

        #region Overrides of TaskScheduler
        /// <summary>Indicates the maximum concurrency level this <see cref="T:System.Threading.Tasks.TaskScheduler" /> is able to support.</summary>
        /// <returns>Returns an integer that represents the maximum concurrency level. The default scheduler returns <see cref="F:System.Int32.MaxValue" />.</returns>
        public override int MaximumConcurrencyLevel { get; } = MaxThreads;

        /// <summary>Queues a <see cref="T:System.Threading.Tasks.Task" /> to the scheduler. </summary>
        /// <param name="task">The <see cref="T:System.Threading.Tasks.Task" /> to be queued.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="task" /> argument is null.</exception>
        protected override void QueueTask(Task task)
        {
            this.ThrowOnDisposed();
            if (this.TryExecuteTaskInline(task, false)) return;
            this._tasks.Add(task, this._cancellationToken.Token);
        }

        /// <summary>Determines whether the provided <see cref="T:System.Threading.Tasks.Task" /> can be executed synchronously in this call, and if it can, executes it.</summary>
        /// <param name="task">The <see cref="T:System.Threading.Tasks.Task" /> to be executed.</param>
        /// <param name="taskWasPreviouslyQueued">A Boolean denoting whether or not task has previously been queued. If this parameter is True, then the task may have been previously queued (scheduled); if False, then the task is known not to have been queued, and this call is being made in order to execute the task inline without queuing it.</param>
        /// <returns>A Boolean value indicating whether the task was executed inline.</returns>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="task" /> argument is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">The <paramref name="task" /> was already executed.</exception>
        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            this.ThrowOnDisposed();
            if (taskWasPreviouslyQueued || !Thread.CurrentThread.IsSTA()) return false;

            return this.TryExecuteTask(task);
        }

        /// <summary>For debugger support only, generates an enumerable of <see cref="T:System.Threading.Tasks.Task" /> instances currently queued to the scheduler waiting to be executed.</summary>
        /// <returns>An enumerable that allows a debugger to traverse the tasks currently queued to this scheduler.</returns>
        /// <exception cref="T:System.NotSupportedException">This scheduler is unable to generate a list of queued tasks at this time.</exception>
        protected override IEnumerable<Task> GetScheduledTasks()
        {
            this.ThrowOnDisposed();
            return this._tasks;
        }
        #endregion Overrides of TaskScheduler

        #region IDisposable
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            if (this._cancellationToken == null || this._cancellationToken.IsCancellationRequested) return;
            this._tasks.CompleteAdding();
            this._cancellationToken.Cancel();
            this._threads.Where(t => t.IsAlive).ForEach(t => t.Abort());
            this._cancellationToken.Dispose();
            this._tasks.Dispose();
        }
        #endregion IDisposable

        private void Start()
        {
            try
            {
                var token = this._cancellationToken.Token;
                foreach (var task in this._tasks.GetConsumingEnumerable(token)) this.TryExecuteTask(task);
            }
            finally
            {
                this._tasks.Dispose();
            }
        }

        private void ThrowOnDisposed()
        {
            if (this._cancellationToken == null || this._cancellationToken.IsCancellationRequested)
                throw new ObjectDisposedException(nameof(STATaskScheduler));
        }

        private void InitThreadPool(int threadPoolSize)
        {
            for (var i = 0; i < threadPoolSize; i++)
            {
                var thread = new Thread(this.Start) { IsBackground = true , Name = $"{nameof(STATaskScheduler)}#{i}"};
                thread.TrySetApartmentState(ApartmentState.STA);
                this._threads.Add(thread);
                thread.Start();
            }
        }
    }
}