namespace Sardonyx.Framework.Core.Exceptions.ProblemDetails
{
    public sealed class NotFoundExceptionProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public NotFoundExceptionProblemDetails(NotFoundException exception)
        {
            Title = exception.Title;
            Status = exception.StatusCode;
            Detail = exception.Message;
        }
    }
}
