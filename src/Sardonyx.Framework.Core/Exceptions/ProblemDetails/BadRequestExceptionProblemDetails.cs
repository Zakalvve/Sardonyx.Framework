namespace Sardonyx.Framework.Core.Exceptions.ProblemDetails
{
    public sealed class BadRequestExceptionProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public BadRequestExceptionProblemDetails(BadRequestException exception)
        {
            Title = exception.Title;
            Status = exception.StatusCode;
            Detail = exception.Message;
        }
    }
}
