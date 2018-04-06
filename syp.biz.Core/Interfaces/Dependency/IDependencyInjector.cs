using System;
using System.Collections.Generic;

namespace syp.biz.Core.Interfaces.Dependency
{
	/// <summary>
	/// Provides facilities to register and resolve dependencies.
	/// </summary>
	public interface IDependencyInjector
	{
		/// <summary>
		/// A bound <see cref="IDependencyFactory"/>.
		/// </summary>
		IDependencyFactory Factory { get; }

		/// <summary>
		/// Registers a dependency <see cref="IDependencyResolver{T}"/> for a type.
		/// </summary>
		/// <typeparam name="T">The dependency type for which this <paramref name="resolver"/> resolves.</typeparam>
		/// <param name="resolver">The dependency <see cref="IDependencyResolver{T}"/> for this type.</param>
		void RegisterResolver<T>(IDependencyResolver<T> resolver);

		/// <summary>
		/// Registers a dependency <see cref="IDependencyResolver"/> for a type.
		/// </summary>
		/// <param name="type">The dependency type for which this <paramref name="resolver"/> resolves.</param>
		/// <param name="resolver">The dependency <see cref="IDependencyResolver"/> for this type.</param>
		void RegisterResolver(Type type, IDependencyResolver resolver);

		/// <summary>
		/// Registers a singleton dependency value.
		/// </summary>
		/// <typeparam name="T">The dependency value's type.</typeparam>
		/// <param name="singletonValue">The dependency value to register.</param>
		void RegisterSingleton<T>(T singletonValue);

		/// <summary>
		/// Registers a lazy dependency factory. The lazy dependency factory generates a dependency value upon the first resolution request.
		/// </summary>
		/// <typeparam name="T">The type of dependency value the dependency factory generates.</typeparam>
		/// <param name="factory">The lazy dependency value factory to register.</param>
		void RegisterFactory<T>(Func<T> factory);

		/// <summary>
		/// Registers a dependency type.
		/// </summary>
		/// <param name="type">The dependency type to register.</param>
		void RegisterType(Type type);

		/// <summary>
		/// Registers dependency types.
		/// </summary>
		/// <param name="types">The dependency types to register.</param>
		void RegisterTypes(IEnumerable<Type> types);

		/// <summary>
		/// Registers a container of dependency types.
		/// </summary>
		/// <param name="container">The dependency types to register.</param>
		void RegisterContainer(IDependencyContainer container);

		/// <summary>
		/// Resolves a dependency.
		/// </summary>
		/// <typeparam name="T">The dependency type to resolve.</typeparam>
		/// <returns>An instance of the dependency type to be resolved.</returns>
		/// <exception cref="Exception">In case there is no resolver for the dependency type.</exception>
		T Resolve<T>();

		/// <summary>
		/// Resolves a dependency.
		/// </summary>
		/// <param name="type">The dependency type to resolve.</param>
		/// <returns>An instance of the dependency type to be resolved.</returns>
		/// <exception cref="Exception">In case there is no resolver for the dependency type.</exception>
		object Resolve(Type type);

		/// <summary>
		/// Tries to resolve a dependency.
		/// </summary>
		/// <typeparam name="T">The dependency type to resolve.</typeparam>
		/// <param name="dependency">An instance of the dependency type to be resolved in case the resolution was successful.</param>
		/// <returns>A <see cref="bool"/> denoting the success of the resolution.</returns>
		bool TryResolve<T>(out T dependency);

		/// <summary>
		/// Resolve all dependencies implementing <paramref name="type"/>.
		/// </summary>
		/// <param name="type">The dependency type to resolve.</param>
		/// <returns>A <see cref="ICollection{T}"/> of instances of the dependency type to be resolved.</returns>
		/// <remarks>In case no dependency instances are available an empty <see cref="IComparer{T}"/> will be returned.</remarks>
		ICollection<object> ResolveAll(Type type);

		/// <summary>
		/// Resolve all dependencies implementing <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The dependency type to resolve.</typeparam>
		/// <returns>A <see cref="ICollection{T}"/> of <typeparamref name="T"/> instances of to be resolved.</returns>
		/// <remarks>In case no dependency instances are available an empty <see cref="IComparer{T}"/> will be returned.</remarks>
		ICollection<T> ResolveAll<T>();
	}
}
