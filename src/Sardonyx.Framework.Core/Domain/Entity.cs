using Sardonyx.Framework.Core.Application;
using Sardonyx.Framework.Core.Exceptions;

namespace Sardonyx.Framework.Core.Domain
{
    public class Entity
    {
        public void CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken())
            {
                throw new BusinessRuleValidationException(rule);
            }
        }

        public async Task CheckRuleAsync(IBusinessRuleAsync rule)
        {
            if (await rule.IsBrokenAsync())
            {
                throw new BusinessRuleValidationException(rule);
            }
        }
    }
}
