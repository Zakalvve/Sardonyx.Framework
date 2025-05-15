using Sardonyx.Framework.Core.CQRS.Data;

namespace Sardonyx.Framework.Core.CQRS.Infrastructure
{
    internal sealed class CommandDispatcher : ICommandDispatcher
    {
        private readonly ICQRSContext _context;
        private readonly ICommandAccessor _accessor;

        public CommandDispatcher(ICQRSContext context, ICommandAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }

        public async Task DispatchCommandsAsync()
        {
            var commands = _accessor.GetAllCommands();

            await _context.InternalCommands.AddRangeAsync(commands);

            _accessor.ClearAllCommands();
        }
    }
}
