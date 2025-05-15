using Microsoft.AspNetCore.Http;

namespace Sardonyx.Framework.Core.Exceptions
{
    public class UnauthorizedException : HttpResponseException
    {
        public override int StatusCode => StatusCodes.Status401Unauthorized;
        public override string Title => "Unauthorized";

        public UnauthorizedException(string message = "You are not authorized.")
            : base(message) { }
    }
}
