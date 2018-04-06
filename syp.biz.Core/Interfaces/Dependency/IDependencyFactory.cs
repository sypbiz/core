using System;
using System.Collections.Generic;

namespace syp.biz.Core.Interfaces.Dependency
{
	/// <summary>
	/// Provides facilities to auto-build instances with injected dependencies.
	/// </summary>
	public interface IDependencyFactory
	{
		/// <summary>
		/// Creates an instance of <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of instance to create.</typeparam>
		/// <returns>An instance of <typeparamref name="T"/>.</returns>
		T CreateInstance<T>();

		/// <summary>
		/// Create an instance of <paramref name="type"/>.
		/// </summary>
		/// <param name="type">The type of instance to create.</param>
		/// <returns>An instance of <paramref name="type"/>.</returns>
		object CreateInstance(Type type);

		/// <summary>
		/// Creates instances of the given <paramref name="types"/>.
		/// </summary>
		/// <param name="types">An <see cref="IEnumerable{T}"/> of types to create.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> of instance for the given <paramref name="types"/>.</returns>
		IEnumerable<object> CreateInstances(IEnumerable<Type> types);

		/// <summary>
		/// Creates instances of the given <paramref name="types"/>.
		/// </summary>
		/// <typeparam name="T">The type of instances to create.</typeparam>
		/// <param name="types">An <see cref="IEnumerable{T}"/> of types to create.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> of instance of type <typeparamref name="T"/>.</returns>
		IEnumerable<T> CreateInstances<T>(IEnumerable<Type> types);
//
//		/// <summary>
//		/// Creates an instance via the <paramref name="constructor"/>.
//		/// </summary>
//		/// <typeparam name="T">The type to cast the result to.</typeparam>
//		/// <param name="constructor">The <see cref="ConstructorInfo"/> to invoke.</param>
//		/// <returns>A casted instance of the <paramref name="constructor"/>.</returns>
//		T CreateInstance<T>(ConstructorInfo constructor);
//
//		/// <summary>
//		/// Creates an instance via the <paramref name="constructor"/>.
//		/// </summary>
//		/// <param name="constructor">The <see cref="ConstructorInfo"/> to invoke.</param>
//		/// <returns>An instance of the <paramref name="constructor"/>.</returns>
//		object CreateInstance(ConstructorInfo constructor);
	}
}