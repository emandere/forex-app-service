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
        private readonly DbContext _context = null;
        public ForexRuleMap(IMapper mapper,IOptions<Settings> settings,ForexDailyPriceMap dailyPriceMap)
        {
            _mapper = mapper;
            _context = new DbContext(settings);
            _dailyPriceMap = dailyPriceMap;
        }

        public async Task<ForexRuleDTO> GetRule(string rule,string pair, string startDate,string endDate)
        {
            var prices = await _dailyPriceMap.GetPriceRangeInternal(pair,startDate,endDate);
            var ruleResult = new ForexRuleDTO()
            {
                IsMet = new RSIOverbought70().IsMet(prices),
                window = prices.Count,
                RuleName = "RSIOverbought"
            };

            return ruleResult;
        }
    }
}