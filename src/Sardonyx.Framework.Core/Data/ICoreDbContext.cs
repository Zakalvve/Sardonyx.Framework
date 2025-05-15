using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Sardonyx.Framework.Core.Data
{
    public interface ICoreDbContext
    {
        ChangeTracker ChangeTracker { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
