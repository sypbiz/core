using System;

namespace syp.biz.Core.Extensions
{
    /// <summary>
    /// Collection of <see cref="DateTime"/> extension methods.
    /// </summary>
    public static class DateTimeExtensions
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Converts <paramref name="dateTime"/> to EPOCH Unix time.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> to convert.</param>
        /// <returns>The <paramref name="dateTime"/>'s EPOCH Unix time.</returns>
        public static long ToUnixTime(this DateTime dateTime) => (long)(dateTime - Epoch).TotalMilliseconds;
    }
}
