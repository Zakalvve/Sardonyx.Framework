using MediatR;

namespace Sardonyx.Framework.Core.CQRS.Application
{
    public interface IInternalCommand { }
    public interface ICommand<out TResult> : IRequest<TResult>, IInternalCommand { }
    public interface ICommand : IRequest, IInternalCommand { }
}
