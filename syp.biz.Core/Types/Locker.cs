using System;
using System.Diagnostics;
using System.Threading;

namespace syp.biz.Core.Types
{
	public class Locker<T> where T : class
	{
		public Locker(T lockObject)
		{
			this.LockObject = lockObject ?? throw new ArgumentNullException(nameof(lockObject));
		}

		public T LockObject { get; }

		[DebuggerStepThrough]
		public void Run(Action action)
		{
			lock (this.LockObject)
			{
				action();
			}
		}

		[DebuggerStepThrough]
		public bool TryRun(Action action) => this.TryRun(action, Timeout.InfiniteTimeSpan);

		[DebuggerStepThrough]
		public bool TryRun(Action action, TimeSpan timeout)
		{
			if (!Monitor.TryEnter(this.LockObject, timeout)) return false;

			try
			{
				action();
				return true;
			}
			finally
			{
				Monitor.Exit(this.LockObject);
			}
		}

		[DebuggerStepThrough]
		public TResult Run<TResult>(Func<TResult> function)
		{
			lock (this.LockObject)
			{
				return function();
			}
		}

		[DebuggerStepThrough]
		public bool TryRun<TResult>(Func<TResult> function, out TResult result) => this.TryRun(function, Timeout.InfiniteTimeSpan, out result);

		[DebuggerStepThrough]
		public bool TryRun<TResult>(Func<TResult> function, TimeSpan timeout, out TResult result)
		{
			if (!Monitor.TryEnter(this.LockObject, timeout))
			{
				result = default;
				return false;
			}

			try
			{
				result = function();
				return true;
			}
			finally
			{
				Monitor.Exit(this.LockObject);
			}
		}

		public void Lock() => Monitor.Enter(this.LockObject);
		public void Unlock() => Monitor.Exit(this.LockObject);

		public void Wait() => this.Run(() => Monitor.Wait(this.LockObject));
		public bool Wait(TimeSpan timeout) => this.Run(() => Monitor.Wait(this.LockObject, timeout));

		public void PulseOne() => this.Run(() => Monitor.Pulse(this.LockObject));
		public void PulseAll() => this.Run(() => Monitor.PulseAll(this.LockObject));

		public UsageLock GetUsageLock() => new UsageLock(this.LockObject);

		public struct UsageLock : IDisposable
		{
			private readonly T _lockObject;

			internal UsageLock(T lockObject)
			{
				this._lockObject = lockObject;
				Monitor.Enter(this._lockObject);
			}

			#region IDisposable
			/// <inheritdoc />
			/// <summary>Unlocks the <see cref="T:syp.biz.Core.Types.Locker`1.UsageLock" />.</summary>
			public void Dispose() => Monitor.Exit(this._lockObject);
			#endregion IDisposable
		}
	}

	public class Locker : Locker<object>
	{
		public Locker(object lockObject) : base(lockObject ?? new object()) { }

		public Locker() : base(new object()) { }
	}
}
