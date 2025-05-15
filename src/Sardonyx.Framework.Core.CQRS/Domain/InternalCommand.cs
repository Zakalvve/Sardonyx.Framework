using Newtonsoft.Json;
using Sardonyx.Framework.Core.CQRS.Application;

namespace Sardonyx.Framework.Core.CQRS.Domain
{
    public sealed class InternalCommand
    {
        public Guid Id { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsProcessed { get; set; }
        public DateTime? DateProcessed { get; set; }
        public DateTime? DateScheduled { get; set; }
        public int MaxRetries { get; set; }
        public int RetryIntervalSeconds { get; set; }
        public int Tries { get; set; }
        public string CommandType { get; set; }
        public string CommandData { get; set; }
        public string? Message { get; set; }


        private InternalCommand() { /* For Entity Framework */ }
        private InternalCommand(Guid id, string commandType, string commandData, DateTime? dateScheduled, int maxRetries, int retryIntervalSeconds)
        {
            Id = id;
            DateAdded = DateTime.UtcNow;
            IsProcessed = false;
            DateProcessed = null;
            DateScheduled = dateScheduled;
            MaxRetries = maxRetries;
            RetryIntervalSeconds = retryIntervalSeconds;
            Tries = 0;
            CommandType = commandType;
            CommandData = commandData;
            Message = null;
        }

        public static InternalCommand Create(Guid id, IInternalCommand command, DateTime? dateScheduled = null, int maxRetries = 3, int retryIntervalSeconds = 1)
        {
            return new InternalCommand(id, command.GetType().FullName, JsonConvert.SerializeObject(command), dateScheduled, maxRetries, retryIntervalSeconds);
        }

        public ICommand GetCommand()
        {
            try
            {
                var type = Type.GetType(CommandType);

                if (type == null)
                    throw new InvalidOperationException($"{nameof(InternalCommand)}.{nameof(GetCommand)}: Serialized type '{CommandType}' cannot be found in assembly");

                var command = JsonConvert.DeserializeObject(CommandData, type);

                if (command is not ICommand typedCommand)
                    throw new InvalidCastException($"{nameof(InternalCommand)}.{nameof(GetCommand)}: Deserialized type does not implement ICommand");

                return typedCommand;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"{nameof(InternalCommand)}.{nameof(GetCommand)}: Error during deserialization - {ex.Message}", ex);
            }
        }

        public void ProcessCommandSuccess()
        {
            Tries++;
            ProcessCommand(true, "Command processed successfully");
        }
        public void ProcessCommandFailure(Exception? ex = null)
        {
            Tries++;

            bool shouldRetry = Tries < MaxRetries;
            string message = $"Command processing failed. {(shouldRetry ? "Retry scheduled" : "Max retries reached. Command processing aborted.")} {(ex != null ? $"Reason: {ex.Message}. StackTrace: {ex.StackTrace}" : string.Empty)}";

            ProcessCommand(!shouldRetry, message);

            if (shouldRetry)
            {
                DateScheduled = DateTime.UtcNow.AddSeconds(RetryIntervalSeconds);
            }
        }

        private void ProcessCommand(bool isProcessed, string message)
        {
            if (isProcessed)
            {
                DateProcessed = DateTime.UtcNow;
                IsProcessed = true;
            }

            Message = message;
        }
    }
}
