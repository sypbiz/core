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

        /// <summary>
        /// Converts <paramref name="unixEpochTime"/> to <see cref="DateTime"/>.
        /// </summary>
        /// <param name="unixEpochTime">The Epoch Unix time to convert.</param>
        /// <returns>A <see cref="DateTime"/> representation of <paramref name="unixEpochTime"/>.</returns>
        public static DateTime ToDateTime(this long unixEpochTime) => Epoch + TimeSpan.FromMilliseconds(unixEpochTime);
    }
}
