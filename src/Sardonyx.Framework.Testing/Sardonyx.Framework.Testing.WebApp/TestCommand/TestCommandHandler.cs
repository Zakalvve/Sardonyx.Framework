using Sardonyx.Framework.Core.CQRS.Application;

namespace Sardonyx.Framework.Testing.WebApp.TestCommand
{
    public class TestCommandHandler : ICommandHandler<TestCommand, string>
    {
        public Task<string> Handle(TestCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(request.Message);
        }
    }
}
