using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace syp.biz.Core.Helpers
{
    /// <summary>
    /// Helper class to work with <see cref="Enum"/>s.
    /// </summary>
    /// <typeparam name="T">The <see cref="Enum"/> type.</typeparam>
    public sealed class EnumHelper<T> where T : struct, IConvertible
    {
        private static readonly Dictionary<T, string> Names;
        private static readonly Dictionary<string, T> Values;

        static EnumHelper()
        {
            if (!typeof(T).IsEnum) throw new ArgumentException($"{typeof(T).Name} is not a valid Enum", nameof(T));
            Names = BuildNamesDictionary(typeof(T));
            Values = Names
                .Union(Names.Keys.ToDictionary(k => k, k => k.ToString(CultureInfo.InvariantCulture)))
                .ToDictionary(n => n.Value, n => n.Key);
        }

        private static Dictionary<T, string> BuildNamesDictionary(Type type)
        {
            return type.GetFields(BindingFlags.Static | BindingFlags.Public)
#pragma warning disable IDE0037 // Use inferred member name
                .Select(f => new
                {
                    Value = (T)f.GetValue(null),
                    // ReSharper disable once RedundantAnonymousTypePropertyName
                    Description = f.GetCustomAttribute<DescriptionAttribute>()?.Description
                })
#pragma warning restore IDE0037 // Use inferred member name
                .ToDictionary(f => f.Value, f => f.Description ?? f.Value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Resolves the representing the <paramref name="enumValue"/>.<br/>
        /// Uses the <see cref="DescriptionAttribute"/> of the <paramref name="enumValue"/> if defined.
        /// </summary>
        /// <param name="enumValue">The value of the <see cref="Enum"/> to be resolved.</param>
        /// <returns>A string representing <paramref name="enumValue"/>.</returns>
        public string Resolve(T enumValue) => Names[enumValue];

        /// <summary>
        /// Resolves the <see cref="Enum"/> value represented by <paramref name="stringValue"/>.<br/>
        /// Uses the <see cref="DescriptionAttribute"/> of the <see cref="Enum"/> if defined.
        /// </summary>
        /// <param name="stringValue">The string value of the <see cref="Enum"/> to be resolved.</param>
        /// <returns>A <see cref="Enum"/> value represented by <paramref name="stringValue"/>.</returns>
        public T Resolve(string stringValue) => Values[stringValue];
    }
}
