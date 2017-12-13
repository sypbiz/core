using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace syp.biz.Core.Extensions
{
    /// <summary>
    /// Collection of <see cref="System.Reflection"/> extension methods.
    /// </summary>
    public static class ReflectionExtensions
    {
        private static readonly Type CollectionType = typeof(ICollection);

        /// <summary>
        /// Gets the <see cref="Version"/> of the <see cref="Assembly"/> where <paramref name="type"/> is defined.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to get the version of.</param>
        /// <returns>The <see cref="Version"/> of the <see cref="Assembly"/> where <paramref name="type"/> is defined.</returns>
        public static Version GetAssemblyVersion(this Type type) => type.Assembly.GetName().Version;

        /// <summary>
        /// Checks if <paramref name="type"/> is a concrete class.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to check.</param>
        /// <returns><c>true</c> if <paramref name="type"/> is a value type or a non-abstract class, otherwise <c>false</c>.</returns>
        public static bool IsConcrete(this Type type) => type.IsClass && !type.IsAbstract || type.IsValueType;

        /// <summary>
        /// Checks if <paramref name="type"/> implements <see cref="ICollection"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to check.</param>
        /// <returns><c>true</c> if <paramref name="type"/> implements <see cref="ICollection"/>, otherwise <c>false</c>.</returns>
        public static bool IsCollection(this Type type) => CollectionType.IsAssignableFrom(type);

        /// <summary>
        /// Get all the types in <paramref name="assembly"/> that <typeparamref name="T"/> can be assign to.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <param name="assembly">The assembly where <typeparamref name="T"/> can be found.</param>
        /// <returns>An enumeration of Types from <paramref name="assembly"/>, which <typeparamref name="T"/> can be assign to.</returns>
        public static IEnumerable<Type> GetTypes<T>(this Assembly assembly)
        {
            return assembly.GetTypes().Where(t => !t.IsAbstract && typeof(T).IsAssignableFrom(t));
        }

        /// <summary>
        /// Create instances of type <typeparamref name="T"/> of the provided <paramref name="types"/>.
        /// </summary>
        /// <typeparam name="T">The type to instantiate.</typeparam>
        /// <param name="types">The types enumeration.</param>
        /// <returns>An enumeration of instances of type <typeparamref name="T"/>.</returns>
        public static IEnumerable<T> Activate<T>(this IEnumerable<Type> types) => types.Select(Activator.CreateInstance).OfType<T>();

        /// <summary>
        /// Get instances of type <typeparamref name="T"/> of types in <paramref name="assembly"/>.
        /// </summary>
        /// <typeparam name="T">The type to instantiate.</typeparam>
        /// <param name="assembly">The assembly to search.</param>
        /// <returns>An enumeration of instances of type <typeparamref name="T"/> from <paramref name="assembly"/>.</returns>
        public static IEnumerable<T> GetInstances<T>(this Assembly assembly) => assembly.GetTypes<T>().Activate<T>();
    }
}
