using Sardonyx.Framework.Core.CQRS.Data;

namespace Sardonyx.Framework.Core.CQRS.Infrastructure
{
    internal sealed class UnitOfWork
    {
        private readonly ICQRSContext _context;
        private readonly ICommandDispatcher _dispatcher;

        public UnitOfWork(ICQRSContext context, ICommandDispatcher dispatcher)
        {
            _context = context;
            _dispatcher = dispatcher;
        }

        public async Task Commit()
        {
            await _dispatcher.DispatchCommandsAsync();
            await _context.SaveChangesAsync();
        }
    }
}
