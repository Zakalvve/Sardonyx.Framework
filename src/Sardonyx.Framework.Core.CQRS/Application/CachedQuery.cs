using Sardonyx.Framework.Core.Caching;
using Sardonyx.Framework.Core.CQRS.Infrastructure;

namespace Sardonyx.Framework.Core.CQRS.Application
{
    public abstract class CachedQuery<TResult> : ICachedQuery<TResult>
    {
        protected CachedQuery(string keyPrefix, int? recordLimit = null, Func<TResult, bool>? cachePredicate = null, TimeSpan? cacheDuration = null, TimeSpan? slidingExpirationDuration = null)
        {
            KeyPrefix = keyPrefix;
            RecordLimit = recordLimit;
            CachePredicate = cachePredicate;
            CacheDuration = cacheDuration;
            SlidingExpirationDuration = slidingExpirationDuration;
        }

        [CacheKeyPrefix]
        public string KeyPrefix { get; }

        [CacheKeyIgnore]
        public int? RecordLimit { get; set; }

        [CacheKeyIgnore]
        public Func<TResult, bool>? CachePredicate { get; set; }

        [CacheKeyIgnore]
        public TimeSpan? CacheDuration { get; set; }

        [CacheKeyIgnore]
        public TimeSpan? SlidingExpirationDuration { get; set; }
    }
}
