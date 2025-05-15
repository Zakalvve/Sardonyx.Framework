using Sardonyx.Framework.Core.CQRS.Data;
using Sardonyx.Framework.Core.CQRS.Domain;

namespace Sardonyx.Framework.Core.CQRS.Infrastructure
{
    internal sealed class CommandAccessor : ICommandAccessor
    {
        private readonly ICQRSContext _context;

        public CommandAccessor(ICQRSContext context)
        {
            _context = context;
        }

        public List<InternalCommand> GetAllCommands()
        {
            var temp = _context.ChangeTracker
                .Entries<Entity>();

            var entityEntries = _context.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Commands != null && x.Entity.Commands.Any()).ToList();

            return entityEntries
               .SelectMany(entry => entry.Entity.Commands)
               .ToList();
        }

        public void ClearAllCommands()
        {
            var entityEntries = _context.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Commands != null && x.Entity.Commands.Any()).ToList();

            entityEntries
                .ForEach(entry => entry.Entity.ClearCommands());
        }
    }
}
