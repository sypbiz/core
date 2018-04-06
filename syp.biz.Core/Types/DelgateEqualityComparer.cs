using System.Collections.Generic;

namespace syp.biz.Core.Types
{
	public class DelgateEqualityComparer<T> : IEqualityComparer<T>
	{
		private readonly HashDelegate _hashDelegate;
		private readonly EqualsDelegate _equalsDelegate;

		/// <summary>
		/// Returns a hash code for the specified object.
		/// </summary>
		/// <param name="obj">The <see cref="T"/> for which a hash code is to be returned.</param>
		/// <returns>A hash code for the specified object.</returns>
		public delegate int HashDelegate(T obj);

		/// <summary>Determines whether the specified objects are equal.</summary>
		/// <param name="x">The first object of type <see cref="T"/> to compare.</param>
		/// <param name="y">The second object of type <see cref="T"/> to compare.</param>
		/// <returns><c>true</c> if the specified objects are equal; otherwise, <c>false</c>.</returns>
		public delegate bool EqualsDelegate(T x, T y);

		public DelgateEqualityComparer(HashDelegate hashDelegate, EqualsDelegate equalsDelegate)
		{
			this._hashDelegate = hashDelegate;
			this._equalsDelegate = equalsDelegate;
		}

		public DelgateEqualityComparer(HashDelegate hashDelegate) : this(hashDelegate, (x, y) => AreEqual(x, y, hashDelegate)) { }

		#region Implementation of IEqualityComparer<in T>
		/// <summary>Determines whether the specified objects are equal.</summary>
		/// <param name="x">The first object of type <see cref="T"/> to compare.</param>
		/// <param name="y">The second object of type <see cref="T"/> to compare.</param>
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
			if (x == null && y == null) return true;
			if (x == null || y == null) return false;
			return hashDelegate(x) == hashDelegate(y);
		}
	}
}