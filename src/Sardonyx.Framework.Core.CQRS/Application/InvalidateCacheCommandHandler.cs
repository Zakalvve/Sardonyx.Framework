using Sardonyx.Framework.Core.Caching;

namespace Sardonyx.Framework.Core.CQRS.Application
{
    internal class InvalidateCacheCommandHandler : ICommandHandler<InvalidateCacheCommand>
    {
        private readonly ICachingService _cachingService;

        public InvalidateCacheCommandHandler(ICachingService cachingService)
        {
            _cachingService = cachingService;
        }
        public Task Handle(InvalidateCacheCommand command, CancellationToken cancellationToken)
        {
            if (command.InvalidationOperation == (int)CacheInvalidationOperation.Purge)
            {
                _cachingService.Empty();
            }
            else if (command.InvalidationOperation == (int)CacheInvalidationOperation.KeyEquals)
            {
                _cachingService.EmptyByKey(command.InvalidKeyParts.FirstOrDefault() ?? string.Empty);
            }
            else if (command.InvalidationOperation == (int)CacheInvalidationOperation.KeyContainsAny)
            {
                _cachingService.EmptyByPredicate(key => command.InvalidKeyParts.Any(part => key.Contains(part)));
            }
            else if (command.InvalidationOperation == (int)CacheInvalidationOperation.KeyContainsAll)
            {
                _cachingService.EmptyByPredicate(key => command.InvalidKeyParts.All(part => key.Contains(part)));
            }

            return Task.CompletedTask;
        }
    }
}
