using Microsoft.AspNetCore.Http;

namespace Sardonyx.Framework.Core.Exceptions.ProblemDetails
{
    public sealed class BusinessRuleValidationExceptionProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public BusinessRuleValidationExceptionProblemDetails(BusinessRuleValidationException exception)
        {
            Title = "Business rule broken";
            Status = StatusCodes.Status409Conflict;
            Detail = exception.Message;
        }
    }
}
