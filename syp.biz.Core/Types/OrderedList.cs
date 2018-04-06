using System;
using System.Collections.Generic;

namespace syp.biz.Core.Types
{
	/// <summary>
	/// Wrapper class for <see cref="SortedList{TKey,TValue}"/>
	/// </summary>
	/// <typeparam name="T">The type of element to store and order by.</typeparam>
	public sealed class OrderedList<T> : SortedList<T, T>
	{
		public OrderedList(DelegateComparer<T>.CompareDelegate comparer) : base(new DelegateComparer<T>(GetEqualBypass(comparer))) { }

		private static DelegateComparer<T>.CompareDelegate GetEqualBypass(DelegateComparer<T>.CompareDelegate comparer)
		{
			return (x, y) =>
			{
				var result = comparer(x, y);
				return result != 0 ? result : Comparer<int>.Default.Compare(x.GetHashCode(), y.GetHashCode());
			};
		}
		/// <summary>
		/// Adds an element into the <see cref="OrderedList{T}"/>.
		/// </summary>
		/// <param name="item">The value of the element to add.</param>
		/// <exception cref="ArgumentNullException">Item is null.</exception>
		/// <exception cref="ArgumentException">An element with the same key already exists in the <see cref="OrderedList{T}"/></exception>
		public void Add(T item)
		{
			this.Add(item, item);
		}
	}
}