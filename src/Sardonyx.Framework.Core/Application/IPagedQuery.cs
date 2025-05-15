namespace Sardonyx.Framework.Core.Application
{
    public interface IPagedQuery
    {
        int PageIndex { get; }
        int ItemsPerPage { get; }
    }
}
