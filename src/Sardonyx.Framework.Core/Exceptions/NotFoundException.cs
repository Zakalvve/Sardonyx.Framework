using Microsoft.AspNetCore.Http;

namespace Sardonyx.Framework.Core.Exceptions
{
    public class NotFoundException : HttpResponseException
    {
        public override int StatusCode => StatusCodes.Status404NotFound;
        public override string Title => "Resource not found";

        public NotFoundException(string resourceName, object key)
            : base($"{resourceName} with key '{key}' was not found.") { }
    }
}
