namespace Sardonyx.Framework.Core.CQRS.Infrastructure
{
    internal interface ICommandDispatcher
    {
        Task DispatchCommandsAsync();
    }
}