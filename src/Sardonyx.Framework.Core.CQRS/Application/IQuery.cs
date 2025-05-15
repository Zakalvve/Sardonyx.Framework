using MediatR;

namespace Sardonyx.Framework.Core.CQRS.Application
{
    public interface IQuery<out TResult> : IRequest<TResult> { }
}
