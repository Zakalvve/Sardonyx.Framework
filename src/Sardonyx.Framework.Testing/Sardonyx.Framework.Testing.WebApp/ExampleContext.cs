using Microsoft.EntityFrameworkCore;
using Sardonyx.Framework.Core.CQRS.Data;
using Sardonyx.Framework.Core.CQRS.Domain;
using Sardonyx.Framework.Core.Data;

namespace Sardonyx.Framework.Testing.WebApp
{
    public class ExampleContext : BaseDbContext, ICQRSContext
    {
        public ExampleContext(DbContextOptions<ExampleContext> options, IEnumerable<IEntityModelConfiguration> configurations) : base(options, configurations)
        {
        }
        public DbSet<InternalCommand> InternalCommands { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
