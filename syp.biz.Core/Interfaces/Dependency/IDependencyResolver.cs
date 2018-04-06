namespace syp.biz.Core.Interfaces.Dependency
{
	/// <summary>
	/// Provides a facility to hold and resolve a specific dependency.
	/// </summary>
	public interface IDependencyResolver
	{
		/// <summary>
		/// Resolves a dependency.
		/// </summary>
		/// <returns>The dependecy to be resolved.</returns>
		object Resolve();
	}

	/// <summary>
	/// Provides a facility to hold and resolve a specific dependency of type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type of the resolvable dependency.</typeparam>
	public interface IDependencyResolver<out T> : IDependencyResolver
	{
		/// <summary>
		/// Resolves a dependency.
		/// </summary>
		/// <returns>The dependecy to be resolved.</returns>
		new T Resolve();
	}
}