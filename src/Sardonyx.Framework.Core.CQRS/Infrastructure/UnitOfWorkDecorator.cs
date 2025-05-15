using MediatR;
using Sardonyx.Framework.Core.CQRS.Application;
using Sardonyx.Framework.Core.CQRS.Data;

namespace Sardonyx.Framework.Core.CQRS.Infrastructure
{
    internal sealed class UnitOfWorkDecorator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        private readonly ICQRSContext _context;
        private readonly ICommandDispatcher _dispatcher;

        public UnitOfWorkDecorator(ICQRSContext context, ICommandDispatcher dispatcher)
        {
            _context = context;
            _dispatcher = dispatcher;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var result = await next();

            var uow = new UnitOfWork(_context, _dispatcher);
            await uow.Commit();

            return result;
        }
    }
}
