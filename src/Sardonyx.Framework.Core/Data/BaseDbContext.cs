using Microsoft.EntityFrameworkCore;

namespace Sardonyx.Framework.Core.Data
{
    public abstract class BaseDbContext : DbContext, ICoreDbContext
    {
        private readonly IEnumerable<IEntityModelConfiguration> _configurations;

        public BaseDbContext(DbContextOptions options, IEnumerable<IEntityModelConfiguration> configurations) : base(options)
        {
            _configurations = configurations;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach(var config in _configurations)
            {
                config.Configure(builder, this);
            }
        }
    }
}
