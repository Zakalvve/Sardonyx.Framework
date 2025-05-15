using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace Sardonyx.Framework.Core.Caching
{
    /// <summary>
    /// Provides an in-memory caching mechanism for asynchronous data retrieval,
    /// with optional conditional caching and built-in support for automatic key tracking and invalidation.
    /// </summary>
    /// <remarks>
    /// This service stores results of asynchronous operations in an <see cref="IMemoryCache"/> instance and ensures
    /// that cache entries are only added if caching is enabled and a configurable predicate evaluates to <c>true</c>.
    ///
    /// Cached entries are automatically tracked and removed from an internal key registry upon eviction,
    /// enabling precise cache invalidation by key or matching pattern.
    /// </remarks>
    public class CachingService : ICachingService
    {
        private readonly IMemoryCache _cache;
        private PostEvictionDelegate _postEvictionDelegate;

        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private static ConcurrentDictionary<string, byte> _activeKeys = new ConcurrentDictionary<string, byte>();

        public CachingService(IMemoryCache cache)
        {
            _cache = cache;
            _postEvictionDelegate = OnEviction;
        }

        /// <summary>
        /// Attempts to retrieve a value from the in-memory cache using the specified <paramref name="key"/>.
        /// If the entry is not found, the provided <paramref name="fallback"/> function is executed to fetch the data.
        /// The result is then optionally cached if it satisfies the <paramref name="cachingPredicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the result to cache.</typeparam>
        /// <param name="key">The cache key used to reference the stored result.</param>
        /// <param name="fallback">An asynchronous function that provides the data if no cache entry exists.</param>
        /// <param name="cachingPredicate">
        /// An optional predicate that determines whether the fetched result should be cached.
        /// If <c>null</c>, the result will always be cached when not <c>null</c>.
        /// </param>
        /// <param name="recordLimit">
        /// An optional size value to associate with the cache entry, used if a size limit is configured.
        /// </param>
        /// <returns>
        /// The cached or freshly fetched result, or <c>null</c> if the fallback returns <c>null</c>.
        /// </returns>
        public async Task<T?> GetResultFromCache<T>(
            string key,
            AsyncFactory<T> fallback,
            Func<T, bool>? cachingPredicate = null,
            int? recordLimit = null,
            TimeSpan? cacheDuration = null,
            TimeSpan? slidingExpirationDuration = null,
            CancellationToken cancellationToken = default)
        {
            T? result;

            // If the cache is populated simply retrieve those values
            if (_cache.TryGetValue(key, out result))
                return result;

            try
            {
                // Wait for the semaphore to be unlocked
                await _semaphore.WaitAsync();

                if (!_cache.TryGetValue(key, out result))
                {
                    result = await fallback(cancellationToken);

                    if (result is not null && (cachingPredicate?.Invoke(result) ?? true))
                    {
                        var cacheOptions = new MemoryCacheEntryOptions
                        {
                            AbsoluteExpiration = cacheDuration.HasValue ? DateTime.UtcNow + cacheDuration : null,
                            SlidingExpiration = slidingExpirationDuration,
                            Size = recordLimit
                        };

                        var callbackRegistration = new PostEvictionCallbackRegistration()
                        {
                            EvictionCallback = _postEvictionDelegate
                        };

                        cacheOptions.PostEvictionCallbacks.Add(callbackRegistration);

                        _cache.Set(key, result, cacheOptions);
                        _activeKeys.TryAdd(key, 0);
                    }
                }
            }
            finally
            {
                _semaphore.Release();
            }

            return result;
        }

        /// <summary>
        /// Callback invoked when a cache entry is evicted. Updates <see cref="_activeKeys"/> to reflect the eviction.
        /// </summary>
        /// <param name="key">The key of the cache entry that was evicted.</param>
        /// <param name="value">The value of the cache entry that was evicted.</param>
        /// <param name="reason">The reason the cache entry was evicted.</param>
        private void OnEviction(object key, object? value, EvictionReason reason, object? state)
        {
            string? validKey = key.ToString();

            if (string.IsNullOrEmpty(validKey)) return;

            if (_activeKeys.ContainsKey(validKey))
            {
                _activeKeys.TryRemove(validKey, out _);
            }
        }

        /// <summary>
        /// Removes the cache entry associated with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key of the cache entry to remove.</param>
        public void EmptyByKey(string key)
        {
            _cache.Remove(key);
            _activeKeys.TryRemove(key, out _);
        }

        /// <summary>
        /// Removes all cache entries for which the specified <paramref name="predicate"/> returns <c>true</c>.
        /// </summary>
        /// <param name="predicate">A function that evaluates whether a given key should be removed.</param>
        public void EmptyByPredicate(Func<string, bool> predicate)
        {
            var matchingKeys = _activeKeys.Keys.Where(key => predicate(key));

            foreach (string key in matchingKeys)
            {
                EmptyByKey(key);
            }
        }

        /// <summary>
        /// Removes all entries from the cache.
        /// </summary>
        public void Empty()
        {
            var allKeys = _activeKeys.Keys.ToArray();

            foreach (string key in allKeys)
            {
                EmptyByKey(key);
            }
        }
    }

    public delegate Task<T> AsyncFactory<T>(CancellationToken cancellationToken = default);
}