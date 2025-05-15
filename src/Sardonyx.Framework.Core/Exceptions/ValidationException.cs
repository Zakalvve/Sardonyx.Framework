using Microsoft.AspNetCore.Http;

namespace Sardonyx.Framework.Core.Exceptions
{
    public class ValidationException : HttpResponseException
    {
        public override int StatusCode => StatusCodes.Status422UnprocessableEntity;
        public override string Title => "Validation failed";

        public IDictionary<string, string[]> Errors { get; }

        public ValidationException(IDictionary<string, string[]> errors)
            : base("One or more validation errors occurred.")
        {
            Errors = errors;
        }
    }
}
