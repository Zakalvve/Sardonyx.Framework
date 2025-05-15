namespace Sardonyx.Framework.Core.Exceptions.ProblemDetails
{
    public sealed class ForbiddenExceptionProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public ForbiddenExceptionProblemDetails(ForbiddenException exception)
        {
            Title = exception.Title;
            Status = exception.StatusCode;
            Detail = exception.Message;
        }
    }
}
