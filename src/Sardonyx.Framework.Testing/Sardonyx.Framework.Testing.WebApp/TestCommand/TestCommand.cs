using Sardonyx.Framework.Core.CQRS.Application;

namespace Sardonyx.Framework.Testing.WebApp.TestCommand
{
    public class TestCommand : ICommand<string>
    {
        public TestCommand(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}
