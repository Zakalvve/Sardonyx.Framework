namespace Sardonyx.Framework.Core.Caching
{
    public enum CacheInvalidationOperation
    {
        Purge = 0,
        KeyEquals = 10,
        KeyContainsAny = 20,
        KeyContainsAll = 30
    }
}
