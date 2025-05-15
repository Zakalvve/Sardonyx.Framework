using static Sardonyx.Framework.Core.Caching.CachingService;

namespace Sardonyx.Framework.Core.Caching
{
    public interface ICachingService
    {
        /// <summary>
        /// Removes all entries from the cache.
        /// </summary>
        void Empty();

        /// <summary>
        /// Removes the cache entry associated with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key of the cache entry to remove.</param>
        void EmptyByKey(string key);

        /// <summary>
        /// Removes all cache entries for which the specified <paramref name="predicate"/> returns <c>true</c>.
        /// </summary>
        /// <param name="predicate">A function that evaluates whether a given key should be removed.</param>
        void EmptyByPredicate(Func<string, bool> predicate);

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
        Task<T?> GetResultFromCache<T>(
            string key,
            AsyncFactory<T> fallback,
            Func<T, bool>? cachingPredicate = null,
            int? recordLimit = null,
            TimeSpan? cacheDuration = null,
            TimeSpan? slidingExpirationDuration = null,
            CancellationToken cancellationToken = default
        );
    }
}