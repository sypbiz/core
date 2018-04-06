using syp.biz.Core.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace syp.biz.Core.Types
{
    /// <summary>
    /// A concurrent cached set, which caches <typeparamref name="T"/> items and expires them if not updated during the expiration period.
    /// </summary>
    /// <typeparam name="T">The item's type.</typeparam>
    public class TimedCache<T> : IDisposable
    {
        /// <summary>
        /// Fires when an item is expired (has not been updated within the given expiration duration).
        /// </summary>
        public event EventHandler<T> ItemExpired;

        /// <summary>
        /// Fires when a new item has been added the the cache.
        /// </summary>
        public event EventHandler<T> ItemAdded;

        private readonly ConcurrentDictionary<T, CacheInfo> _cache;
        private readonly TimeSpan _expirationDuration;
        private readonly Timer _expirationTimer;

        /// <summary>
        /// Creates a new instance of <see cref="TimedCache{T}"/>.
        /// </summary>
        /// <param name="expirationDuration">The duration an item lives insdide the cache before expiring.</param>
        /// <remarks>Uses the <see cref="EqualityComparer{T}.Default"/> comparer.</remarks>
        public TimedCache(TimeSpan expirationDuration) : this(expirationDuration, EqualityComparer<T>.Default) { }

        /// <summary>
        /// Creates a new instance of <see cref="TimedCache{T}"/>.
        /// </summary>
        /// <param name="expirationDuration">The duration an item lives insdide the cache before expiring.</param>
        /// <param name="comparer">A <see cref="IEqualityComparer{T}"/> to use in order to check for the existance of an item in the cache.</param>
        public TimedCache(TimeSpan expirationDuration, IEqualityComparer<T> comparer)
        {
            ValidateExpirationCheckInterval(expirationDuration);
            this._cache = new ConcurrentDictionary<T, CacheInfo>(comparer);
            this._expirationDuration = expirationDuration;
            this._expirationTimer = new Timer(this.ExpireCheck, null, this._expirationDuration, this._expirationDuration);
        }

        #region IDisposable
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() => this._expirationTimer.Dispose();
        #endregion IDisposable

        /// <summary>
        /// Pauses the expiration check timer.
        /// </summary>
        public void PauseChecking() => this._expirationTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);

        /// <summary>
        /// Resumes the expiration check timer.
        /// </summary>
        public void ResumeChecking() => this._expirationTimer.Change(this._expirationDuration, this._expirationDuration);

        /// <summary>
        /// Adds or updates an item in the cache.
        /// </summary>
        /// <param name="item">The item to cache or update.</param>
        public void Cache(T item) => this.Cache(item, null, null);

        /// <summary>
        /// Adds or updates an item in the cache.
        /// </summary>
        /// <param name="item">The item to cache or update.</param>
        /// <param name="onItemAdded">Triggered if the item was added to the cache (did not exist in the cache).</param>
        /// <param name="onItemUpdated">Triggered if the item was updaed in the cache (existed in the cache).</param>
        /// <exception cref="ArgumentNullException">If <paramref name="item"/> is <c>null</c>.</exception>
        public void Cache(T item, Action<T> onItemAdded, Action<T> onItemUpdated)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            var added = false;
            CacheInfo addValueFactory(T absent)
            {
                added = true;
                if (this.ItemAdded != null) Task.Factory.StartNew(() => this.ItemAdded(this, absent));
                return new CacheInfo();
            }

            this._cache.AddOrUpdate(item, addValueFactory, UpdateValueFactory);

            if (added)
            {
                onItemAdded?.Invoke(item);
            }
            else
            {
                onItemUpdated?.Invoke(item);
            }
        }

        private static CacheInfo UpdateValueFactory(T existing, CacheInfo existingCacheInfo) => existingCacheInfo.Touch();

        private static void ValidateExpirationCheckInterval(TimeSpan expirationCheckInterval)
        {
            if (expirationCheckInterval > TimeSpan.Zero) return;

            throw new ArgumentOutOfRangeException(nameof(expirationCheckInterval), expirationCheckInterval,
                "Expiration check interval must be a positive non-zero timespan");
        }

        private void ExpireCheck(object @null)
        {
            void expireItem(KeyValuePair<T, CacheInfo> item)
            {
                if (!this._cache.TryRemove(item.Key, out var _)) return;
                if (this.ItemExpired != null) Task.Factory.StartNew(() => this.ItemExpired(this, item.Key));
            }

            try
            {
                var now = DateTime.Now;
                var expired = this._cache.Where(item => now - item.Value.Timestamp >= this._expirationDuration).ToArray();
                expired.ForEach(expireItem);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        [DebuggerDisplay("{DebugDisplay}")]
        private class CacheInfo
        {
            public DateTime Timestamp { get; private set; } = DateTime.Now;

            public CacheInfo Touch()
            {
                this.Timestamp = DateTime.Now;
                return this;
            }

            private string DebugDisplay => (DateTime.Now - this.Timestamp).ToString("g");
        }
    }
}
