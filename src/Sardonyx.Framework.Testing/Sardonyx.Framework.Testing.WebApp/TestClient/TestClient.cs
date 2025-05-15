using Sardonyx.Framework.Client;
using Sardonyx.Framework.Core.Caching;
using Sardonyx.Framework.Core.Http;

namespace Sardonyx.Framework.Testing.WebApp.TestClient
{
    public interface ITestClient : IBaseClient
    {
        IExampleService Examples { get; set; }
    }

    public class TestClient : BaseClient, ITestClient
    {
        public IExampleService Examples { get; set; }
        public TestClient(HttpClient client, ICachingService cache, CachingOptions? cacheOptions = null) : base(client, cache, "baseUrl", cacheOptions)
        {
            Examples = new ExampleService(this);
        }
    }

    public interface IExampleService
    {
        Task<HttpClientResult> Get();
    }

    public class ExampleService : BaseService, IExampleService
    {
        private readonly TestClient client;

        public ExampleService(TestClient client) : base(client)
        {
            this.client = client;
        }

        public async Task<HttpClientResult> Get()
        {
            return await Task.FromResult(client.GetResult(new Exception()));
        }
    }
}
