namespace Sardonyx.Framework.Core.Domain
{
    public interface IRepository<T> where T : Entity
    {
        Task<T?> GetByIdAsync(object id);
        Task<IReadOnlyList<T>> ListAsync();
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);
    }
}
