using System.Collections.Generic;
using forex_app_service.Domain;
namespace forex_app_service.Domain.Rules
{
    public interface IRule
    {
        bool IsMet(IEnumerable<ForexDailyPrice> window);
    }
}