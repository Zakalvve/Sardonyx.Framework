using System.Linq.Expressions;

namespace Sardonyx.Framework.Core.Domain
{
    public interface ISpecification<T> where T : Entity
    {
        Expression<Func<T, bool>> Criteria { get; }
        Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; }
        string[] Includes { get; }
        int? Take { get; }
        int? Skip { get; }
    }
}
