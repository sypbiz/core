using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace syp.biz.Core.Extensions
{
	/// <summary>
	/// Collection of <see cref="Enumerable"/> and <see cref="IEnumerable{T}"/> extension methods.
	/// </summary>
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Executes <paramref name="action"/> for each item in <paramref name="items"/>.
		/// </summary>
		/// <typeparam name="T">The type of items in <paramref name="items"/>.</typeparam>
		/// <param name="items">An enumeration of items to execute on.</param>
		/// <param name="action">The action to execute on <paramref name="items"/>.</param>
		public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
		{
			foreach (var item in items) action(item);
		}

		/// <summary>
		/// Yields each item in <paramref name="items"/> to be manipulated in-stream.
		/// </summary>
		/// <typeparam name="T">The type of items in <paramref name="items"/>.</typeparam>
		/// <param name="items">An enumeration of items to yield.</param>
		/// <param name="yielded">A function to perform on each yielded item.</param>
		/// <returns>An enumeration of yielded items.</returns>
		public static IEnumerable<T> Yield<T>(this IEnumerable<T> items, Func<T, T> yielded) => items.Select(yielded);

		/// <summary>
		/// Filters <paramref name="items"/> for non-nulls.
		/// </summary>
		/// <typeparam name="T">The type of items in <paramref name="items"/>.</typeparam>
		/// <param name="items">An enumeration of items to filter.</param>
		/// <returns>An enumeration of non-null items.</returns>
		public static IEnumerable<T> NotNulls<T>(this IEnumerable<T> items) where T : class => items.Where(item => item != null);

		/// <summary>
		/// Checks if all <paramref name="items"/> are <c>true</c>.
		/// </summary>
		/// <param name="items">An enumeration of booleans to check.</param>
		/// <returns><c>true</c> if all <paramref name="items"/> are <c>true</c>, otherwise <c>false</c>.</returns>
		public static bool AllTrue(this IEnumerable<bool> items) => items.All(item => item);

		/// <summary>
		/// Checks if any of <paramref name="items"/> are <c>false</c>.
		/// </summary>
		/// <param name="items">An enumeration of booleans to check.</param>
		/// <returns><c>true</c> if any of <paramref name="items"/> is <c>false</c>, otherwise <c>true</c>.</returns>
		public static bool AnyFalse(this IEnumerable<bool> items) => items.Any(item => !item);

		/// <summary>
		/// Creates async tasks from <paramref name="items"/> via <paramref name="taskBuilder"/> and awaits for all tasks to complete.
		/// </summary>
		/// <typeparam name="TIn">The type of items in <paramref name="items"/>.</typeparam>
		/// <typeparam name="TOut">The type of returned result from the async task.</typeparam>
		/// <param name="items">An enumeration of items to process.</param>
		/// <param name="taskBuilder">A delegate which builds an async task for an item to produce a <typeparamref name="TOut"/>.</param>
		/// <returns>A task which completes when all built tasks have completed.</returns>
		public static async Task<IEnumerable<TOut>> WhenAll<TIn, TOut>(this IEnumerable<TIn> items, Func<TIn, Task<TOut>> taskBuilder)
		{
			return await Task.WhenAll(items.Select(taskBuilder));
		}
	}
}
