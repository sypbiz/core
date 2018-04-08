using System.Collections.Generic;

namespace syp.biz.Core.Extensions
{
    /// <summary>
    /// A collection of dictionary related extension methods.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Safely updates <paramref name="key"/> with <paramref name="value"/>; fails silently on errors.
        /// </summary>
        /// <typeparam name="TKey">The type of <paramref name="key"/>.</typeparam>
        /// <typeparam name="TValue">The type of <paramref name="value"/>.</typeparam>
        /// <param name="dictionary">The dictionary to modify.</param>
        /// <param name="key">The key to modify.</param>
        /// <param name="value">The value to set to <paramref name="key"/></param>
        /// <param name="generateNewDictionary">If set to <c>true</c>, will create new <see cref="IDictionary{TKey,TValue}"/> if original is <c>null</c>, otherwise will ignore and return <c>null</c>.</param>
        /// <returns>The modified <paramref name="dictionary"/>.</returns>
        public static IDictionary<TKey, TValue> SafeUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value, bool generateNewDictionary = true)
        {
            try
            {
                if (dictionary == null)
                {
                    if (!generateNewDictionary) return null;
                    dictionary = new Dictionary<TKey, TValue>();
                }

                dictionary[key] = value;
                return dictionary;
            }
            catch
            {
                return dictionary;
            }
        }

        /// <summary>
        /// Safely gets the value of <paramref name="key"/> from <paramref name="dictionary"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the <paramref name="dictionary"/>'s key.</typeparam>
        /// <typeparam name="TValue">The type of the <paramref name="dictionary"/>'s value.</typeparam>
        /// <param name="dictionary">The dictionary to query.</param>
        /// <param name="key">The key to retrieve.</param>
        /// <param name="defaultValue">Optional. A value to return in case <paramref name="dictionary"/> is <c>null</c> or <paramref name="key"/> is not found. Default is default(<typeparamref name="TValue"/>).</param>
        /// <returns>The value associated with <paramref name="key"/>, or <paramref name="defaultValue"/> if <paramref name="dictionary"/> is <c>null</c> or <paramref name="key"/> is not found.</returns>
        public static TValue SafeGet<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default)
        {
            return dictionary != null && dictionary.TryGetValue(key, out var value)
                ? value
                : defaultValue;
        }

        /// <summary>
        /// Deconstructs a <see cref="KeyValuePair{TKey,TValue}"/> to <paramref name="key"/> and <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of <paramref name="key"/>.</typeparam>
        /// <typeparam name="TValue">The type of <paramref name="value"/>.</typeparam>
        /// <param name="keyValuePair">The <see cref="KeyValuePair{TKey,TValue}"/> to deconstruct.</param>
        /// <param name="key">The <paramref name="keyValuePair"/>'s key.</param>
        /// <param name="value">The <paramref name="keyValuePair"/>'s value.</param>
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> keyValuePair, out TKey key, out TValue value)
        {
            key = keyValuePair.Key;
            value = keyValuePair.Value;
        }
    }
}
