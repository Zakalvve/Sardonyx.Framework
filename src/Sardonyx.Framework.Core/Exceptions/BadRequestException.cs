using Microsoft.AspNetCore.Http;

namespace Sardonyx.Framework.Core.Exceptions
{
    public class BadRequestException : HttpResponseException
    {
        public override int StatusCode => StatusCodes.Status400BadRequest;
        public override string Title => "Bad Request";

        public BadRequestException(string message = "The request was invalid or improperly formed.")
            : base(message) { }
    }
}
