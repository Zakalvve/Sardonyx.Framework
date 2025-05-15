using Sardonyx.Framework.Core.Caching;
using Sardonyx.Framework.Core.CQRS.Application;

namespace Sardonyx.Framework.Testing.WebApp.TestQuery
{
    public class TestQuery : CachedQuery<string>
    {
        public TestQuery(string result) : base(nameof(TestQuery))
        {
            Result = result;
        }

        [CacheKeyPrefix]
        public string ResultCachePrefix => nameof(String);

        public string Result { get; set; }
    }
}
