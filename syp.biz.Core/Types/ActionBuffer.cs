using System;
using System.Collections.Concurrent;
using System.Threading;
using syp.biz.Core.Extensions;

namespace syp.biz.Core.Types
{
	public class ActionBuffer : IDisposable
	{
		private readonly BlockingCollection<Action> _pendingActions = new BlockingCollection<Action>();
		private readonly CancellationTokenSource _cts = new CancellationTokenSource();
		private readonly Thread _thread;
		private bool _disposed;

		public ActionBuffer() : this(null) { }

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
