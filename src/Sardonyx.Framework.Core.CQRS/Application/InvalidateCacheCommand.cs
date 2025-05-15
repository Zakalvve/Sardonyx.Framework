using Sardonyx.Framework.Core.Caching;

namespace Sardonyx.Framework.Core.CQRS.Application
{
    public class InvalidateCacheCommand : ICommand
    {
        public InvalidateCacheCommand()
        {
            InvalidationOperation = (int)CacheInvalidationOperation.Purge;
        }
        public InvalidateCacheCommand(string key)
        {
            InvalidationOperation = (int)CacheInvalidationOperation.KeyEquals;
            InvalidKeyParts = new List<string> { key };
        }

        public InvalidateCacheCommand(List<string> invalidKeyParts)
        {
            InvalidationOperation = (int)CacheInvalidationOperation.KeyContainsAny;
            InvalidKeyParts = invalidKeyParts;
        }

        public int InvalidationOperation { get; set; }
        public List<string> InvalidKeyParts { get; set; } = new();
    }
}
