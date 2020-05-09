using System.Collections.Generic;
using System.Linq;
using forex_app_service.Domain.Indicators;
namespace forex_app_service.Domain.Rules
{
    public class RSIOverbought70:IRule
    {
        public bool IsMet(IEnumerable<ForexDailyPrice> window)
        {
            if(Stats.RSI(window.Select(z=> new List<double>{z.Open,z.Close})) > 70 )
                return true;
            else
                return false;
        }
    }
}
