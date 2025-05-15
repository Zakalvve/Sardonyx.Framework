using Sardonyx.Framework.Core.Application;

namespace Sardonyx.Framework.Core.Exceptions
{
    public sealed class BusinessRuleValidationException : Exception
    {
        public IBusinessRuleMessage BrokenRuleMessage { get; }

        public string Details { get; }

        public BusinessRuleValidationException(IBusinessRuleMessage brokenRuleMessage)
            : base(brokenRuleMessage.Message)
        {
            BrokenRuleMessage = brokenRuleMessage;
            Details = BrokenRuleMessage.Message;
        }

        public override string ToString()
        {
            return $"{BrokenRuleMessage.GetType().FullName}: {BrokenRuleMessage.Message}";
        }
    }
}
