using System;
using System.Text;

namespace syp.biz.Core.Extensions
{
    /// <summary>
    /// Collection of <see cref="string"/> extension methods.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Determines whether <paramref name="org"/> and <paramref name="value"/> have the same value using <see cref="StringComparison.InvariantCultureIgnoreCase"/>.
        /// </summary>
        /// <param name="org">The string to compare.</param>
        /// <param name="value">The string to compare to.</param>
        /// <returns>true if the value of the <paramref name="value">value</paramref> parameter is the same as <paramref name="org"/>; otherwise, false.</returns>
        public static bool EqualsIgnoreCase(this string org, string value) => org.Equals(value, StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        /// Indicates whether a specified string is null, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>true if the <paramref name="value">value</paramref> parameter is null or <see cref="F:System.String.Empty"></see>, or if <paramref name="value">value</paramref> consists exclusively of white-space characters.</returns>
        public static bool IsNullOrWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);

        /// <summary>
        /// Indicates that <paramref name="value"/> is not null, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>false if the <paramref name="value">value</paramref> parameter is null or <see cref="F:System.String.Empty"></see>, or if <paramref name="value">value</paramref> consists exclusively of white-space characters.</returns>
        /// <remarks>This is an inverse of <see cref="IsNullOrWhiteSpace"/>.</remarks>
        public static bool HasContent(this string value) => !string.IsNullOrWhiteSpace(value);

        /// <summary>
        /// Indicates whether the specified string is null or an <see cref="F:System.String.Empty"></see> string.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>true if the <paramref name="value">value</paramref> parameter is null or an empty string (""); otherwise, false.</returns>
        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

        /// <summary>
        /// A wrapper to <see cref="object.ToString"/>, which returns <paramref name="defaultValue"/> in case <paramref name="obj"/> is <c>null</c>.
        /// </summary>
        /// <param name="obj">The object to stringify.</param>
        /// <param name="defaultValue">A default string to return in case <paramref name="obj"/> is <c>null</c>.</param>
        /// <returns>The stringified <paramref name="obj"/>, or <paramref name="defaultValue"/> if <paramref name="obj"/> is <c>null</c>.</returns>
        public static string ToString(this object obj, string defaultValue) => obj?.ToString() ?? defaultValue;

        /// <summary>
        /// Truncates <paramref name="value"/> to a maximum length of <paramref name="maxLength"/>.
        /// </summary>
        /// <param name="value">The string to truncate.</param>
        /// <param name="maxLength">The maximum length of the returned value.</param>
        /// <returns>
        /// The original <paramref name="value"/> in case it is <c>null</c> or its length is equal or less than <paramref name="maxLength"/>,<br/>
        /// or a truncated string of <paramref name="maxLength"/> characters.
        /// </returns>
        public static string Truncate(this string value, int maxLength) => value?.Substring(0, Math.Min(value.Length, maxLength));

        /// <summary>
        /// Converts <paramref name="value"/> to a Base64 representation.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <param name="encoding">Optional. The encoding to use in the conversion. Default is <see cref="Encoding.UTF8"/>.</param>
        /// <returns>A Base64 encoded string of <paramref name="value"/>.</returns>
        public static string EncodeBase64(this string value, Encoding encoding = null) => Convert.ToBase64String((encoding ?? Encoding.UTF8).GetBytes(value));

        /// <summary>
        /// Converts <paramref name="base64"/> representation to its string value.
        /// </summary>
        /// <param name="base64">A Base64 encoded string.</param>
        /// <param name="encoding">Optional. The encoding to use in the conversion. Default is <see cref="Encoding.UTF8"/>.</param>
        /// <returns>A Base64 decoded string of <paramref name="base64"/>.</returns>
        public static string DecodeBase64(this string base64, Encoding encoding = null) => (encoding ?? Encoding.UTF8).GetString(Convert.FromBase64String(base64));
    }
}
