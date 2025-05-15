using Microsoft.EntityFrameworkCore;

namespace Sardonyx.Framework.Core.Data
{
    public interface IEntityModelConfiguration
    {
        void Configure(ModelBuilder builder, DbContext context);
    }
}
