namespace Sardonyx.Framework.Core.Exceptions.ProblemDetails
{
    public sealed class ValidationExceptionProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationExceptionProblemDetails(ValidationException exception)
        {
            Title = exception.Title;
            Status = exception.StatusCode;
            Detail = exception.Message;
            Errors = exception.Errors;
        }
    }
}
