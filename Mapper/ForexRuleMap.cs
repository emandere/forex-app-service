using System;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using forex_app_service.Domain;
using forex_app_service.Models;
using forex_app_service.Domain.Rules;
namespace forex_app_service.Mapper
{
    public class ForexRuleMap
    {
        private readonly IMapper _mapper;
        private readonly ForexDailyPriceMap _dailyPriceMap;
        private readonly ForexIndicatorMap  _indicatorMap;
        private readonly DbContext _context = null;
        public ForexRuleMap(IMapper mapper,IOptions<Settings> settings,ForexDailyPriceMap dailyPriceMap, ForexIndicatorMap indicatorMap)
        {
            _mapper = mapper;
            _context = new DbContext(settings);
            _dailyPriceMap = dailyPriceMap;
            _indicatorMap = indicatorMap;
        }

   

        public async Task<ForexRuleDTO> GetRule(string rule,string pair, int window,string endDate)
        {
            DateTime max =  DateTime.ParseExact(endDate,"yyyy-MM-dd",CultureInfo.InvariantCulture);
            DateTime min = max.AddDays(-window);

            string startDate = min.ToString("yyyy-MM-dd");
            var prices = await _dailyPriceMap.GetPriceRangeInternal(pair,startDate,endDate);
            var ind = await _indicatorMap.GetIndicator(pair,new RSIOverbought70().Indicator(),endDate,window);
            var ruleResult = new ForexRuleDTO()
            {
                IsMet = new RSIOverbought70().IsMet(prices),
                window = prices.Count,
                RuleName = "RSIOverbought",
                indicator = ind.Indicator
            };

            return ruleResult;
        }
    }
}