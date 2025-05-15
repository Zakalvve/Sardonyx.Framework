using Microsoft.EntityFrameworkCore;
using Sardonyx.Framework.Core.CQRS.Domain;
using Sardonyx.Framework.Core.Data;

namespace Sardonyx.Framework.Core.CQRS.Data
{
    internal sealed class CQRSModelConfiguration : IEntityModelConfiguration
    {
        public void Configure(ModelBuilder builder, DbContext context)
        {
            if (context is not ICQRSContext)
                throw new InvalidOperationException("To apply a CQRS configuration your DbContext must implement ICQRSContext");

            builder.ApplyConfiguration(new InternalCommandTypeConfiguration());
        }
    }
}
