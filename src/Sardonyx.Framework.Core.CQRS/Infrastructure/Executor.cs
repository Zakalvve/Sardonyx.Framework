using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sardonyx.Framework.Core.CQRS.Application;

namespace Sardonyx.Framework.Core.CQRS.Infrastructure
{
    internal sealed class Executor : IExecutor
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public Executor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command)
        {
            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            return await mediator.Send(command);
        }

        public async Task ExecuteCommandAsync(ICommand command)
        {
            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(command);
        }

        public async Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
        {
            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            return await mediator.Send(query);
        }
    }
}
