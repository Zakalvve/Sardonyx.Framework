namespace Sardonyx.Framework.Core.Http
{
    public readonly struct HttpClientResult
    {
        public HttpResponseMessage? HttpResponse { get; init; }
        public Exception? Exception { get; init; }
        public bool IsSuccessful { get; init; }
        public string? FaultMessage { get; init; }

        public HttpClientResult(Exception exception, string? faultMessage)
        {
            IsSuccessful = false;
            HttpResponse = null;
            Exception = exception;
            FaultMessage = faultMessage ?? exception.Message;
        }

        public HttpClientResult(HttpResponseMessage response)
        {
            IsSuccessful = response.IsSuccessStatusCode;
            HttpResponse = response;
            Exception = null;
            FaultMessage = IsSuccessful ? null : $"Client responded with fault code: {response.StatusCode}";
        }
    }

    public readonly struct HttpClientResult<TData>
    {
        public HttpResponseMessage? HttpResponse { get; init; }
        public Exception? Exception { get; init; }
        public bool IsSuccessful { get; init; }
        public string? FaultMessage { get; init; }
        public TData? Result { get; init; }

        public HttpClientResult(Exception exception, string? faultMessage)
        {
            Result = default;
            IsSuccessful = false;
            HttpResponse = null;
            Exception = exception;
            FaultMessage = faultMessage ?? exception.Message;
        }

        public HttpClientResult(HttpResponseMessage response, TData? data)
        {
            IsSuccessful = response.IsSuccessStatusCode && data != null;
            HttpResponse = response;
            Result = IsSuccessful ? data : default;
            Exception = null;
            FaultMessage = IsSuccessful ? null : $"Client responded with fault code: {response.StatusCode}";
        }

        public HttpClientResult(TData data)
        {
            IsSuccessful = data != null;
            HttpResponse = null;
            Result = IsSuccessful ? data : default;
            Exception = null;
            FaultMessage = IsSuccessful ? null : "Result cannot be null";
        }
    }
}
