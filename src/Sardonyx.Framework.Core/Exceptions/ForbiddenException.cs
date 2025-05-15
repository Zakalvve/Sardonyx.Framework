using Microsoft.AspNetCore.Http;

namespace Sardonyx.Framework.Core.Exceptions
{
    public class ForbiddenException : HttpResponseException
    {
        public override int StatusCode => StatusCodes.Status403Forbidden;
        public override string Title => "Forbidden";

        public ForbiddenException(string message = "Access denied.")
            : base(message) { }
    }
}
