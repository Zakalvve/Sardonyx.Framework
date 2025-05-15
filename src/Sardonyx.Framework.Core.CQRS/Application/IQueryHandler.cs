using MediatR;

namespace Sardonyx.Framework.Core.CQRS.Application
{
    public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    { }
}
