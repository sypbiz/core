using System;

namespace syp.biz.Core.Logging
{
	/// <summary>
	/// Provides facilities to build a <see cref="ILogger"/>.
	/// </summary>
	public interface ILoggerFactory
	{
		/// <summary>
		/// Builds a logger for the given <paramref name="name"/>.
		/// </summary>
		/// <param name="name">The name of the logger.</param>
		/// <returns>A logger for the given <paramref name="name"/>.</returns>
		ILogger Build(string name);

		/// <summary>
		/// Builds a logger for the given <paramref name="type"/>. Uses the full name of the type.
		/// </summary>
		/// <param name="type">The type for which to build the logger.</param>
		/// <returns>A logger for the given <paramref name="type"/>.</returns>
		ILogger Build(Type type);

		/// <summary>
		/// Builds a logger for the given <typeparamref name="T"/>. Uses the full name of the type.
		/// </summary>
		/// <typeparam name="T">The type for which to build the logger.</typeparam>
		/// <returns>A logger for the given <typeparamref name="T"/>.</returns>
		ILogger Build<T>();
	}
}
