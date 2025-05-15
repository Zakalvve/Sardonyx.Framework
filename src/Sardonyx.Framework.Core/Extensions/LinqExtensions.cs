using Sardonyx.Framework.Core.Application;

namespace Sardonyx.Framework.Core.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<T> AppendPagination<T>(this IQueryable<T> queryable, IPagedQuery query)
        {
            int skip = Math.Max(query.ItemsPerPage * query.PageIndex, 0);
            int itemsPerPage = Math.Max(query.ItemsPerPage, 1);

            return queryable.Skip(skip).Take(itemsPerPage);
        }
    }
}
