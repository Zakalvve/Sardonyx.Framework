namespace Sardonyx.Framework.Core.Exceptions.ProblemDetails
{
    public sealed class UnauthorizedExceptionProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public UnauthorizedExceptionProblemDetails(UnauthorizedException exception)
        {
            Title = exception.Title;
            Status = exception.StatusCode;
            Detail = exception.Message;
        }
    }
}
