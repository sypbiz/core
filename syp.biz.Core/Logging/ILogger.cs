using System;

namespace syp.biz.Core.Logging
{
	/// <summary>
	/// Provides facilities for logging.
	/// </summary>
	public interface ILogger
	{
		bool LogDebugLevel { get; set; }
		bool LogInfoLevel { get; set; }
		bool LogErrorLevel { get; set; }

		/// <summary>
		/// Logs as debug.
		/// </summary>
		/// <param name="message">The message to be logged.</param>
		/// <param name="args">Any object to be logged with the message. The object will be stringified.</param>
		void Debug(string message, params object[] args);

		/// <summary>
		/// Logs as debug. Resolves <paramref name="lazyArgs"/> only if Debug is enabled.
		/// </summary>
		/// <param name="message">The message to be logged.</param>
		/// <param name="lazyArgs">Any object resolver to be logged with the message. The object will be stringified.</param>
		void LazyDebug(string message, params Func<object>[] lazyArgs);

		/// <summary>
		/// Logs as informational.
		/// </summary>
		/// <param name="message">The message to be logged.</param>
		/// <param name="args">Any object to be logged with the message. The object will be stringified.</param>
		void Info(string message, params object[] args);

		/// <summary>
		/// Logs as informational. Resolves <paramref name="lazyArgs"/> only if Info is enabled.
		/// </summary>
		/// <param name="message">The message to be logged.</param>
		/// <param name="lazyArgs">Any object resolver to be logged with the message. The object will be stringified.</param>
		void LazyInfo(string message, params Func<object>[] lazyArgs);

		/// <summary>
		/// Logs as error.
		/// </summary>
		/// <param name="message">The message to be logged.</param>
		/// <param name="args">Any object to be logged with the message. The object will be stringified.</param>
		void Error(string message, params object[] args);

		/// <summary>
		/// Logs as error. Resolves <paramref name="lazyArgs"/> only if Error is enabled.
		/// </summary>
		/// <param name="message">The message to be logged.</param>
		/// <param name="lazyArgs">Any object resolver to be logged with the message. The object will be stringified.</param>
		void LazyError(string message, params Func<object>[] lazyArgs);
	}
}