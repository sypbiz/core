using System.Collections.Generic;

namespace syp.biz.Core.Types
{
	/// <summary>
	/// An implementation of <see cref="IEqualityComparer{T}"/> which uses a provided delegates instead of implementing a class.
	/// </summary>
	/// <typeparam name="T">The type for the <see cref="IEqualityComparer{T}"/>.</typeparam>
	public class DelegateEqualityComparer<T> : IEqualityComparer<T>
	{
		/// <summary>
		/// A default <see cref="DelegateEqualityComparer{T}"/> which uses the objects' <see cref="object.GetHashCode"/> function.
		/// </summary>
		public static readonly IEqualityComparer<T> Default = new DelegateEqualityComparer<T>(o => o.GetHashCode());
		private readonly HashDelegate _hashDelegate;
		private readonly EqualsDelegate _equalsDelegate;

		/// <summary>
		/// Returns a hash code for the specified object.
		/// </summary>
		/// <param name="obj">The type for which a hash code is to be returned.</param>
		/// <returns>A hash code for the specified object.</returns>
		public delegate int HashDelegate(T obj);

		/// <summary>Determines whether the specified objects are equal.</summary>
		/// <param name="x">The first object of to compare.</param>
		/// <param name="y">The second object of to compare.</param>
		/// <returns><c>true</c> if the specified objects are equal; otherwise, <c>false</c>.</returns>
		public delegate bool EqualsDelegate(T x, T y);

		/// <summary>
		/// Constructs a new <see cref="DelegateEqualityComparer{T}"/> with the provided <paramref name="hashDelegate"/> and <paramref name="equalsDelegate"/> delegates.
		/// </summary>
		/// <param name="hashDelegate">A function to compute a hash code for a given <typeparamref name="T"/>.</param>
		/// <param name="equalsDelegate">A function to compare two <typeparamref name="T"/>s.</param>
		public DelegateEqualityComparer(HashDelegate hashDelegate, EqualsDelegate equalsDelegate)
		{
			this._hashDelegate = hashDelegate;
			this._equalsDelegate = equalsDelegate;
		}

		/// <summary>
		/// Constructs a new <see cref="DelegateEqualityComparer{T}"/> with the provided <paramref name="hashDelegate"/> delegate.
		/// </summary>
		/// <param name="hashDelegate">A function to compute a hash code for a given <typeparamref name="T"/>.</param>
		/// <remarks>
		/// Uses the <paramref name="hashDelegate"/> to resolve equality if the two <typeparamref name="T"/> are not reference equal.
		/// </remarks>
		public DelegateEqualityComparer(HashDelegate hashDelegate) : this(hashDelegate, (x, y) => AreEqual(x, y, hashDelegate)) { }

		#region Implementation of IEqualityComparer<in T>
		/// <summary>Determines whether the specified objects are equal.</summary>
		/// <param name="x">The first object of to compare.</param>
		/// <param name="y">The second object of to compare.</param>
		/// <returns><c>true</c> if the specified objects are equal; otherwise, <c>false</c>.</returns>
		public bool Equals(T x, T y) => this._equalsDelegate(x, y);

		/// <summary>Returns a hash code for the specified object.</summary>
		/// <param name="obj">The <see cref="T:System.Object" /> for which a hash code is to be returned.</param>
		/// <returns>A hash code for the specified object.</returns>
		/// <exception cref="T:System.ArgumentNullException">The type of <paramref name="obj" /> is a reference type and <paramref name="obj" /> is null.</exception>
		public int GetHashCode(T obj) => this._hashDelegate(obj);
		#endregion Implementation of IEqualityComparer<in T>

		private static bool AreEqual(T x, T y, HashDelegate hashDelegate)
		{
//			if (x == null && y == null) return true;
//			if (x == null || y == null) return false;
			if (ReferenceEquals(x, y) || ReferenceEquals(x, default) && ReferenceEquals(y, default)) return true;
			if (ReferenceEquals(x, default(T)) || ReferenceEquals(y, default(T))) return false;
			return hashDelegate(x) == hashDelegate(y);
		}
	}
}