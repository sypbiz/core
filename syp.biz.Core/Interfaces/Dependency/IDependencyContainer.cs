using System;
using System.Collections.Generic;

namespace syp.biz.Core.Interfaces.Dependency
{
	/// <summary>
	/// Contains a set of types to be added to the injector.
	/// </summary>
	public interface IDependencyContainer
	{
		/// <summary>
		/// Gets the set of types of this container.
		/// </summary>
		/// <returns>A set of types for this container.</returns>
		IEnumerable<Type> GetTypes();
	}
}
