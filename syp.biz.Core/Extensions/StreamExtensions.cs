using System.IO;
using System.Threading.Tasks;

namespace syp.biz.Core.Extensions
{
    /// <summary>
    /// Collection of <see cref="Stream"/> extension methods
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Reads all bytes of <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">A <see cref="Stream"/> to read all bytes of.</param>
        /// <returns>All the bytes of <paramref name="stream"/>.</returns>
        public static byte[] ReadAllBytes(this Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Asynchronously reads all bytes of <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">A <see cref="Stream"/> to read all bytes of.</param>
        /// <returns>All the bytes of <paramref name="stream"/>.</returns>
        public static async Task<byte[]> ReadAllBytesAsync(this Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
