using Sardonyx.Framework.Core.CQRS.Domain;

namespace Sardonyx.Framework.Core.CQRS.Infrastructure
{
    internal interface ICommandAccessor
    {
        void ClearAllCommands();
        List<InternalCommand> GetAllCommands();
    }
}