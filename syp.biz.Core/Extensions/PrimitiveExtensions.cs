using System;

namespace syp.biz.Core.Extensions
{
    public static class PrimitiveExtensions
    {
        /// <summary>
        /// Returns lowercase string representation of <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="bool"/> value to stringify.</param>
        /// <returns>A lowercase string representation of <paramref name="value"/>.</returns>
        public static string ToLower(this bool value) => value ? "true" : "false";

        /// <summary>
        /// Creates a new <see cref="IntPtr"/> with the <paramref name="integer"/>'s value.
        /// </summary>
        /// <param name="integer">The value for the <see cref="IntPtr"/>.</param>
        /// <returns>A new <see cref="IntPtr"/> with the <paramref name="integer"/>'s value.</returns>
        public static IntPtr AsIntPtr(this int integer) => new IntPtr(integer);

        /// <summary>
        /// Creates a new <see cref="IntPtr"/> with the <paramref name="long"/>'s value.
        /// </summary>
        /// <param name="long">The value for the <see cref="IntPtr"/>.</param>
        /// <returns>A new <see cref="IntPtr"/> with the <paramref name="long"/>'s value.</returns>
        public static IntPtr AsIntPtr(this long @long) => new IntPtr(@long);
    }
}
