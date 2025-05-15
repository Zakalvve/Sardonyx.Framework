using System.Runtime.Serialization;

namespace Sardonyx.Framework.Core.Exceptions
{
    public abstract class HttpResponseException : Exception
    {
        public abstract int StatusCode { get; }
        public abstract string Title { get; }

        public HttpResponseException()
        {
        }
        protected HttpResponseException(string message) : base(message) { }

        protected HttpResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public HttpResponseException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
