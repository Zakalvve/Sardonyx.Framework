using Sardonyx.Framework.Core.CQRS.Application;

namespace Sardonyx.Framework.Core.CQRS.Infrastructure
{
    internal interface ICachedQuery<TResult> : IQuery<TResult>
    {
        string KeyPrefix { get; }
        int? RecordLimit { get; set; }
        Func<TResult, bool>? CachePredicate { get; set; }
        TimeSpan? CacheDuration { get; set; }
        TimeSpan? SlidingExpirationDuration { get; set; }
    }
}
