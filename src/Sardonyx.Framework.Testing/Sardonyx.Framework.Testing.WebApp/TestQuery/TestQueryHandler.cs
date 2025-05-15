using Sardonyx.Framework.Core.CQRS.Application;

namespace Sardonyx.Framework.Testing.WebApp.TestQuery
{
    public class TestQueryHandler : IQueryHandler<TestQuery, string>
    {
        public Task<string> Handle(TestQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(request.Result);
        }
    }
}
