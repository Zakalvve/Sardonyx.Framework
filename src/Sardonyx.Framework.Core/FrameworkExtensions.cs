using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace Sardonyx.Framework.Core
{
    public static class FrameworkExtensions
    {
        public static FrameworkBuilder AddSardonyx(this WebApplicationBuilder builder)
        {
            return new FrameworkBuilder(builder).AddCore();
        }
        public static FrameworkBuilder AddSardonyx<TContext>(this WebApplicationBuilder builder)
            where TContext : DbContext
        {
            return new FrameworkBuilder(builder, typeof(TContext)).AddCore();
        }
    }

    // TODO: Need to consider how app.use calls are made where they are required after building.
}
