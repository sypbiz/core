using System;

namespace syp.biz.Core.Logging
{
	/// <summary>
	/// An empty implementation of <see cref="ILogger"/> which ignores all logging.
	/// </summary>
	public class NullLogger : ILogger
	{
		#region Implementation of ILogger
		public bool LogDebugLevel { get; set; } = false;
		public bool LogInfoLevel { get; set; } = false;
		public bool LogErrorLevel { get; set; } = false;

		/// <summary>
		/// Does not log as debug.
		/// </summary>
		/// <param name="message">The message to be ignored.</param>
		/// <param name="args">Any object to be ignored with the message.</param>
		public void Debug(string message, params object[] args) {}

		/// <summary>
		/// Does not log as debug. Does not resolve <paramref name="lazyArgs"/>.
		/// </summary>
		/// <param name="message">The message to be ignored.</param>
		/// <param name="lazyArgs">Any object resolver to be ignored with the message.</param>
		public void LazyDebug(string message, params Func<object>[] lazyArgs) { }

		/// <summary>
		/// Does not log as informational.
		/// </summary>
		/// <param name="message">The message to be ignored.</param>
		/// <param name="args">Any object to be ignored with the message.</param>
		public void Info(string message, params object[] args) { }

		/// <summary>
		/// Does not log as informational. Does not resolve <paramref name="lazyArgs"/>.
		/// </summary>
		/// <param name="message">The message to be ignored.</param>
		/// <param name="lazyArgs">Any object resolver to be ignored with the message.</param>
		public void LazyInfo(string message, params Func<object>[] lazyArgs) { }

		/// <summary>
		/// Does not log as error.
		/// </summary>
		/// <param name="message">The message to be ignored.</param>
		/// <param name="args">Any object to be ignored with the message.</param>
		public void Error(string message, params object[] args) { }

		/// <summary>
		/// Does not log as error. Does not resolve <paramref name="lazyArgs"/>.
		/// </summary>
		/// <param name="message">The message to be ignored.</param>
		/// <param name="lazyArgs">Any object resolver to be ignored with the message.</param>
		public void LazyError(string message, params Func<object>[] lazyArgs) { }
		#endregion Implementation of ILogger
	}
}
