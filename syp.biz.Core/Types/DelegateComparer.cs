using System.Collections.Generic;

namespace syp.biz.Core.Types
{
	/// <summary>
	/// An implementation of <see cref="IComparer{T}"/> which uses a provided delegate instead of implementing a class.
	/// </summary>
	/// <typeparam name="T">The type for the <see cref="IComparer{T}"/>.</typeparam>
	public class DelegateComparer<T> : IComparer<T>
	{
		/// <summary>
		/// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
		/// </summary>
		/// <param name="x">The first object to compare.</param>
		/// <param name="y">The second object to compare.</param>
		/// <returns>
		/// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />,  as shown in the following table:<br/>
		/// Value Meaning:<br/>
		/// <c>value &lt; 0</c>: <paramref name="x" /> is less than <paramref name="y" />.<br/>
		/// <c>value = 0</c>: <paramref name="x" /> equals <paramref name="y" />.<br/>
		/// <c>value &gt; 0</c>: <paramref name="x" /> is greater than <paramref name="y" />.
		/// </returns>
		public delegate int CompareDelegate(T x, T y);

		private readonly CompareDelegate _comparer;

		/// <summary>
		/// Constructs a new <see cref="DelegateComparer{T}"/> with the provided <paramref name="comparer"/> delegate.
		/// </summary>
		/// <param name="comparer">A function to compare two <typeparamref name="T"/>s.B</param>
		public DelegateComparer(CompareDelegate comparer) => this._comparer = comparer;

		#region Implementation of IComparer<in T>
		/// <summary>Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.</summary>
		/// <param name="x">The first object to compare.</param>
		/// <param name="y">The second object to compare.</param>
		/// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.</returns>
		public int Compare(T x, T y) => this._comparer(x, y);
		#endregion Implementation of IComparer<in T>
	}
}
