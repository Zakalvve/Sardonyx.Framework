using Sardonyx.Framework.Core.CQRS.Application;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sardonyx.Framework.Core.CQRS.Domain
{
    public class Entity : Sardonyx.Framework.Core.Domain.Entity
    {
        private List<InternalCommand> _commands = new List<InternalCommand>();

        [NotMapped]
        public ReadOnlyCollection<InternalCommand> Commands => _commands.AsReadOnly();

        public void AddCommand(IInternalCommand command, DateTime? dateScheduled = null, int maxRetries = 3, int retryIntervalSeconds = 1)
        {
            _commands ??= [];

            _commands.Add(InternalCommand.Create(Guid.NewGuid(), command, dateScheduled, maxRetries, retryIntervalSeconds));
        }

        public void ClearCommands()
        {
            _commands?.Clear();
        }
    }
}
