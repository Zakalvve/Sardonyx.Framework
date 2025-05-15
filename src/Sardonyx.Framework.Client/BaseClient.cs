using Newtonsoft.Json;
using Sardonyx.Framework.Core.Caching;
using Sardonyx.Framework.Core.Http;

namespace Sardonyx.Framework.Client
{
    public interface IBaseClient { }
    public abstract class BaseClient : IBaseClient
    {
        private readonly HttpClient _client;
        private readonly ICachingService? _cache;

        protected string BaseUrl { get; init; }
        internal ICachingService? Cache => _cache;
        protected CachingOptions CacheOptions { get; init; }

        public BaseClient(HttpClient client, ICachingService? cache, string baseUrl, CachingOptions? cacheOptions = null)
        {
            _client = client;
            _cache = cache;

            BaseUrl = baseUrl;
            CacheOptions = cacheOptions ?? new();
        }

        public virtual HttpRequestMessage BuildRequest(HttpMethod method, string endpoint, List<KeyValuePair<string, string>>? headers = null)
        {
            HttpRequestMessage request = new HttpRequestMessage(method, new Uri(endpoint));

            if (headers != null)
            {
                headers.ForEach(header =>
                {
                    request.Headers.Add(header.Key, header.Value);
                });
            }

            return request;
        }

        public async Task<HttpResponseMessage> SendRequestAsync<T>(HttpRequestMessage request, T? data = null) where T : class
        {
            string? jsonString = null;

            if (data != null)
            {
                jsonString = JsonConvert.SerializeObject(data);
            }

            return await SendRequestAsync(request, jsonString);
        }

        public async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request, string? jsonString = null)
        {
            if (jsonString != null)
            {
                var body = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
                request.Content = body;
            }

            return await SendRequestAsync(request);
        }

        public async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request)
        {
            var response = await _client.SendAsync(request);

            return response;
        }

        public async Task<T> DeserializeJsonResponse<T>(HttpResponseMessage response) where T : class
        {
            var jsonString = await response.Content.ReadAsStringAsync();

            var content = JsonConvert.DeserializeObject<T>(jsonString);

            if (content == null) throw new JsonSerializationException();

            return content;
        }

        public async Task<string> DeserializeJsonResponse(HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }

        public HttpClientResult GetResult(Exception exception, string? faultMessage = null)
        {
            return new HttpClientResult(exception, faultMessage);
        }

        public HttpClientResult GetResult(HttpResponseMessage response)
        {
            return new HttpClientResult(response);
        }

        public HttpClientResult<T> GetResult<T>(Exception exception, string? faultMessage = null)
        {
            return new HttpClientResult<T>(exception, faultMessage);
        }

        public HttpClientResult<T> GetResult<T>(HttpResponseMessage response, T? data)
        {
            return new HttpClientResult<T>(response, data);
        }

        public async Task<HttpClientResult<T>> GetCachedResult<T>(string key, AsyncFactory<HttpClientResult<T>> fallback, int? recordLimit = null)
        {
            if (!CacheOptions.EnableCaching || _cache is null)
                return await fallback();

            return await _cache.GetResultFromCache(key, fallback, (result) =>
            {
                return result.IsSuccessful && result.Result != null;
            },
            recordLimit,
            CacheOptions.CacheDuration,
            CacheOptions.SlidingExpirationCacheDuration);
        }
    }

    public abstract class BaseAuthClient : BaseClient
    {
        public BaseAuthClient(HttpClient client, ICachingService cache, string baseUrl, CachingOptions? cacheOptions = null) : base(client, cache, baseUrl, cacheOptions) { }

        public abstract List<KeyValuePair<string, string>> AddAuthorization(List<KeyValuePair<string, string>>? headers = null);

        public virtual HttpRequestMessage BuildRequest(HttpMethod method, string endpoint, bool requiresAuthorization = false, List<KeyValuePair<string, string>>? headers = null)
        {
            if (requiresAuthorization)
            {
                headers = AddAuthorization(headers);
            }

            return BuildRequest(method, endpoint, headers);
        }
    }
}
