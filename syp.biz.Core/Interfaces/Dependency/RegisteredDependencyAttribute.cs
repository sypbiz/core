using System;

namespace syp.biz.Core.Interfaces.Dependency
{
	/// <summary>
	/// Attributed <c>public</c> classes and structs will be added to the dependency injection container.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
	public sealed class RegisteredDependencyAttribute : Attribute
	{
		/// <summary>
		/// If defined, will register the dependency as the provided type, otherwise (<c>null</c>) will register the dependency as its own type.
		/// </summary>
		public Type RegisterAs { get; set; }

		/// <summary>
		/// Determines whether the <see cref="RegisteredDependencyAttribute"/> is applied to <paramref name="type"/>.
		/// </summary>
		/// <param name="type">The type to check.</param>
		/// <returns><c>true</c> if defined with <see cref="RegisteredDependencyAttribute"/>, otherwise <c>false</c>.</returns>
		public static bool IsDefined(Type type)
		{
			return GetCustomAttribute(type, typeof(RegisteredDependencyAttribute)) != null;
		}

		/// <summary>
		/// Transforms a type to its <see cref="RegisterAs"/> type if applicable.
		/// </summary>
		/// <param name="type">The type to transform.</param>
		/// <returns>The transformed type if <see cref="RegisterAs"/> is defined, otherwise the original <paramref name="type"/>.</returns>
		public static Type Transform(Type type)
		{
			var att = GetCustomAttribute(type, typeof(RegisteredDependencyAttribute)) as RegisteredDependencyAttribute;
			return att?.RegisterAs ?? type;
		}
	}
}
