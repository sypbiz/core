using System;
using System.Diagnostics;
using System.Threading;

namespace syp.biz.Core.Types
{
	/// <summary>
	/// A locking wrapper class.
	/// </summary>
	/// <typeparam name="T">The type to use for the locker.</typeparam>
	public class Locker<T> where T : class
	{
		/// <summary>
		/// Constructs a <see cref="Locker{T}"/> with the provided <paramref name="lockObject"/>.
		/// </summary>
		/// <param name="lockObject">The object to use for locking.</param>
		/// <exception cref="ArgumentNullException">Thrown if the <paramref name="lockObject"/> is <c>null</c>.</exception>
		public Locker(T lockObject) => this.LockObject = lockObject ?? throw new ArgumentNullException(nameof(lockObject));

		/// <summary>
		/// Gets the lock object.
		/// </summary>
		public T LockObject { get; }

		/// <summary>
		/// Executes the <paramref name="action"/> in a lock.
		/// </summary>
		/// <param name="action">The action to execute in the lock.</param>
		/// <remarks>
		/// Will wait to acquire the lock indefinitely.
		/// </remarks>
		[DebuggerStepThrough]
		public void Run(Action action)
		{
			 lock (this.LockObject) action();
		}

		/// <summary>
		/// Attempts to acquire an exclusive lock on lock object in order to execute the <paramref name="action"/>.
		/// </summary>
		/// <param name="action">The action to execute.</param>
		/// <returns><c>true</c> if the current thread acquired the lock; otherwise, <c>false</c>.</returns>
		/// <remarks>
		/// The method returns if was not able to acquire the lock immediately.
		/// </remarks>
		[DebuggerStepThrough]
		public bool TryRun(Action action) => this.TryRun(action, TimeSpan.Zero);

		/// <summary>
		/// Attempts, for the specified <paramref name="timeout"/>, to acquire an exclusive lock on lock object in order to execute the <paramref name="action"/>.
		/// </summary>
		/// <param name="action">The action to execute.</param>
		/// <param name="timeout">The amount of time to wait for acquiring the lock.</param>
		/// <returns><c>true</c> if the current thread acquired the lock; otherwise, <c>false</c>.</returns>
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

		/// <summary>
		/// Executes the <paramref name="function"/> in a lock.
		/// </summary>
		/// <typeparam name="TResult">The type of the return value of the method.</typeparam>
		/// <param name="function">The function to execute in the lock.</param>
		/// <returns>The result of <paramref name="function"/>.</returns>
		[DebuggerStepThrough]
		public TResult Run<TResult>(Func<TResult> function)
		{
			lock (this.LockObject)
			{
				return function();
			}
		}

		/// <summary>
		/// Attempts to acquire an exclusive lock on lock object in order to execute the <paramref name="function"/>.
		/// </summary>
		/// <param name="function">The function to execute.</param>
		/// <param name="result">The result of the <paramref name="function"/>.</param>
		/// <returns><c>true</c> if the current thread acquired the lock; otherwise, <c>false</c>.</returns>
		/// <remarks>
		/// The method returns if was not able to acquire the lock immediately.
		/// </remarks>
		[DebuggerStepThrough]
		public bool TryRun<TResult>(Func<TResult> function, out TResult result) => this.TryRun(function, TimeSpan.Zero, out result);

		/// <summary>
		/// Attempts, for the specified <paramref name="timeout"/>, to acquire an exclusive lock on lock object in order to execute the <paramref name="function"/>.
		/// </summary>
		/// <param name="function">The function to execute.</param>
		/// <param name="timeout">The amount of time to wait for acquiring the lock.</param>
		/// <param name="result">The result of the <paramref name="function"/>.</param>
		/// <returns><c>true</c> if the current thread acquired the lock; otherwise, <c>false</c>.</returns>
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

		/// <summary>
		/// Acquires an exclusive lock on the lock object.
		/// </summary>
		public void Lock() => Monitor.Enter(this.LockObject);

		/// <summary>
		/// Releases an exclusive lock on the lock object
		/// </summary>
		public void Unlock() => Monitor.Exit(this.LockObject);

		/// <summary>
		/// Releases the lock on the lock object and blocks the current thread until it reacquires the lock.
		/// </summary>
		public void Wait() => this.Run(() => Monitor.Wait(this.LockObject));

		/// <summary>
		/// Releases the lock on the lock object and blocks the current thread until it reacquires the lock.<br/>
		/// If the specified <paramref name="timeout"/> interval elapses, the thread enters the ready queue.
		/// </summary>
		/// <param name="timeout">A <see cref="TimeSpan"/> representing the amount of time to wait before the thread enters the ready queue.</param>
		/// <returns>
		/// <c>true</c> if the lock was reacquired before the specified <paramref name="timeout"/> elapsed; <c>false</c> if the lock was reacquired after the specified <paramref name="timeout"/> elapsed.<br/>
		/// The method does not return until the lock is reacquired.
		/// </returns>
		public bool Wait(TimeSpan timeout) => this.Run(() => Monitor.Wait(this.LockObject, timeout));

		/// <summary>
		/// Notifies a thread in the waiting queue of a change in the lock object's state.
		/// </summary>
		public void PulseOne() => this.Run(() => Monitor.Pulse(this.LockObject));

		/// <summary>
		/// Notifies all waiting threads of a change in the lock object's state.
		/// </summary>
		public void PulseAll() => this.Run(() => Monitor.PulseAll(this.LockObject));

		/// <summary>
		/// Get a <see cref="UsageLock"/> for the lock object.
		/// </summary>
		/// <returns>A <see cref="UsageLock"/> for the lock object.</returns>
		public UsageLock GetUsageLock() => new UsageLock(this.LockObject);

		/// <summary>
		/// A disposable wrapper for the lock object.
		/// </summary>
		/// <remarks>
		/// The lock object will be locked until the <see cref="UsageLock"/> is disposed.
		/// </remarks>
		public struct UsageLock : IDisposable
		{
			private readonly T _lockObject;

			internal UsageLock(T lockObject)
			{
				this._lockObject = lockObject;
				Monitor.Enter(this._lockObject);
			}

			#region IDisposable
			/// <summary>Unlocks the <see cref="UsageLock"/>'s lock object.</summary>
			public void Dispose() => Monitor.Exit(this._lockObject);
			#endregion IDisposable
		}
	}

	/// <summary>
	/// A default non-generic <see cref="Locker"/>.
	/// </summary>
	public class Locker : Locker<object>
	{
		/// <summary>
		/// Constructs a <see cref="Locker"/> with the provided <paramref name="lockObject"/>.
		/// </summary>
		/// <param name="lockObject">The object to use for locking. If <c>null</c>, will use a new <see cref="object"/>.</param>
		public Locker(object lockObject) : base(lockObject ?? new object()) { }

		/// <summary>
		/// Constructs a <see cref="Locker"/> with a default new <see cref="object"/>.
		/// </summary>
		public Locker() : base(new object()) { }
	}
}
