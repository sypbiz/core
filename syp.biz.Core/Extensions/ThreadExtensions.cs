using System.Threading;

namespace syp.biz.Core.Extensions
{
    public static class ThreadExtensions
    {
        /// <summary>
        /// Sets the name of the current <see cref="Thread"/> to <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The new thread name.</param>
        public static bool TrySetName(string name)
        {
            return TrySetName(Thread.CurrentThread, name);
        }

        /// <summary>
        /// Sets the name of <paramref name="thread"/> to <paramref name="name"/>.
        /// </summary>
        /// <param name="thread">The <see cref="Thread"/> to apply the new name to.</param>
        /// <param name="name">The new thread name.</param>
        public static bool TrySetName(this Thread thread, string name)
        {
            if (thread == null || thread.Name != null) return false;
            thread.Name = name;
            return true;
        }

        /// <summary>
        /// Checks if the thread is stopped.
        /// </summary>
        /// <param name="thread">The <see cref="Thread"/> to check</param>
        /// <returns><c>true</c> if stopped, otherwise <c>false</c>.</returns>
        public static bool IsStopped(this Thread thread) => !thread.IsAlive;

        /// <summary>
        /// Checks if the thread is running as a Single Thread Apartment (STA).
        /// </summary>
        /// <param name="thread">The <see cref="Thread"/> to check</param>
        /// <returns><c>true</c> if running as STA, otherwise <c>false</c>.</returns>
        public static bool IsSTA(this Thread thread) => thread.GetApartmentState() == ApartmentState.STA;
    }
}
