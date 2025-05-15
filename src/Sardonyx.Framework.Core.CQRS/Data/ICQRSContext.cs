using Microsoft.EntityFrameworkCore;
using Sardonyx.Framework.Core.CQRS.Domain;
using Sardonyx.Framework.Core.Data;

namespace Sardonyx.Framework.Core.CQRS.Data
{
    public interface ICQRSContext : ICoreDbContext
    {
        public DbSet<InternalCommand> InternalCommands { get; set; }
    }
}
