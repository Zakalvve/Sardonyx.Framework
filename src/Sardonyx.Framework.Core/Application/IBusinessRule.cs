namespace Sardonyx.Framework.Core.Application
{
    public interface IBusinessRuleMessage
    {
        string Message { get; }
    }
    public interface IBusinessRule : IBusinessRuleMessage
    {
        bool IsBroken();
    }

    public interface IBusinessRuleAsync : IBusinessRuleMessage
    {
        Task<bool> IsBrokenAsync();
    }
}
