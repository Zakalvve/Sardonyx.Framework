using MediatR;
using Sardonyx.Framework.Core.Caching;

namespace Sardonyx.Framework.Core.CQRS.Infrastructure
{
    internal class CachingDecorator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICachedQuery<TResponse>
    {
        private readonly ICachingService _cache;

        public CachingDecorator(ICachingService cache)
        {
            _cache = cache;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var key = CacheKeyFactory.CreateKey(request);

            var result = await _cache.GetResultFromCache(key, ct => next(ct), request.CachePredicate, request.RecordLimit, request.CacheDuration, request.SlidingExpirationDuration, cancellationToken);

            return result;
        }
    }
}
