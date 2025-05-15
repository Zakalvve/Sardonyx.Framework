using Microsoft.AspNetCore.Http;
using Sardonyx.Framework.Core.Exceptions;
using System.Security.Claims;

namespace Sardonyx.Framework.Core.Http
{
    public interface IExecutionContextAccessor
    {
        bool IsAvailable { get; }
        int UserId { get; }
    }

    internal class ExecutionContextAccessor : IExecutionContextAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExecutionContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int UserId
        {
            get
            {
                if (_httpContextAccessor
                    .HttpContext?
                    .User?
                    .FindFirst(ClaimTypes.NameIdentifier)?
                    .Value != null)
                {
                    return Convert.ToInt32(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                }

                string reason = "Unknown reason.";

                if (_httpContextAccessor.HttpContext == null) reason = "HttpContext is null";
                else if (_httpContextAccessor.HttpContext.User == null) reason = "ClaimsPrinciple is null";
                else if ((_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null) == null) reason = "NameIdentifier is missing or null";

                throw new UnauthorizedException($"User context is not available. {reason}");
            }
        }

        public bool IsAvailable => _httpContextAccessor.HttpContext != null;
    }
}
