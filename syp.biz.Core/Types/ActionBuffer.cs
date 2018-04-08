using System;
using System.Collections.Concurrent;
using System.Threading;
using syp.biz.Core.Extensions;

namespace syp.biz.Core.Types
{
	/// <summary>
	/// Buffers actions to be executed out-of-band in a FIFO manner on an execution thread.
	/// </summary>
	public class ActionBuffer : IDisposable
	{
		private readonly BlockingCollection<Action> _pendingActions = new BlockingCollection<Action>();
		private readonly CancellationTokenSource _cts = new CancellationTokenSource();
		private readonly Thread _thread;
		private bool _disposed;

		/// <summary>
		/// Constructs a new <see cref="ActionBuffer"/>.
		/// </summary>
		/// <remarks>
		/// The execution thread will be named <c>ActionBuffer</c>.
		/// </remarks>
		public ActionBuffer() : this(null) { }

		/// <summary>
		/// Constructs a new <see cref="ActionBuffer"/> and names the execution thread.
		/// </summary>
		/// <param name="name">The name of the execution thread.</param>
		public ActionBuffer(string name)
		{
			this._thread = new Thread(this.ThreadLoop) { IsBackground = true };
			this._thread.TrySetApartmentState(ApartmentState.MTA);
			this._thread.TrySetName(name ?? nameof(ActionBuffer));
			this._thread.Start();
		}

		#region IDisposable
		/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
		public void Dispose()
		{
			lock (this)
			{
				if (this._disposed) return;
				this._disposed = true;

				this._cts.Cancel();
				if (!this._thread.Join(TimeSpan.FromSeconds(10))) this._thread.Abort();

				this._cts?.Dispose();
			}
		}
		#endregion IDisposable

		/// <summary>
		/// Enqueues an <paramref name="action"/> to be executed on the execution thread.
		/// </summary>
		/// <param name="action">The action to enqueue.</param>
		public void Enqueue(Action action)
		{
//			this.ThrowOnDisposed();
			this._pendingActions.Add(action, this._cts.Token);
		}

//		private void ThrowOnDisposed()
//		{
//			if (this._disposed || this._cts?.IsCancellationRequested != false) throw new ObjectDisposedException(nameof(STATaskScheduler));
//		}

		private void ThreadLoop()
		{
			var token = this._cts.Token;
			try
			{
				foreach (var action in this._pendingActions.GetConsumingEnumerable(token)) Execute(action);
			}
			finally
			{
				this.Dispose();
			}
		}

		private static void Execute(Action action)
		{
			try
			{
				action();
			}
			catch
			{
				// ignored
			}
		}
	}
}
