namespace Sardonyx.Framework.Client
{
    public class CachingOptions
    {
        public bool EnableCaching { get; set; } = false;
        public TimeSpan CacheDuration { get; set; } = TimeSpan.FromMinutes(1);
        public TimeSpan SlidingExpirationCacheDuration { get; set; } = TimeSpan.FromMinutes(1);
    }
}
