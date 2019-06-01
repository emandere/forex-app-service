using System;
using System.Collections.Generic;
using System.Linq;

namespace forex_app_service.Domain.Indicators
{
    public class Stats
    {
        public static double Average(IEnumerable<double> x)
        {
            double sum = x.Aggregate((t,e)=>t+e);
            double xavg = sum / x.Count();
            return xavg;
        }
        public static double StdDev(IEnumerable<double> x)
        {
           double sumsquared = x.Select(t=>t*t).Aggregate((t,e)=>t+e);
           double stdDev = Math.Sqrt((sumsquared/x.Count()) - Average(x)*Average(x));
           return stdDev;   
        }
    }
}