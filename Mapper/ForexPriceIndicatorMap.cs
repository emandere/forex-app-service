using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using forex_app_service.Domain;
using forex_app_service.Models;
namespace forex_app_service.Mapper
{
    public class ForexPriceIndicatorMap
    {
        private readonly IMapper _mapper;
        private readonly ForexIndicatorMap _indicatorMap;
        private readonly DbContext _context = null;
        private readonly string _forexAppServiceBase;
         public ForexPriceIndicatorMap(IMapper mapper,IOptions<Settings> settings,ForexIndicatorMap indicatorMap)
        {
            _mapper = mapper;
            _context = new DbContext(settings);
            _forexAppServiceBase = settings.Value.ForexAppServiceBase;
            _indicatorMap = indicatorMap;
        }

        public async Task<List<ForexPriceIndicator>> GetLatestPrices(string indicator)
        {
            var result = await _context.LatestPrices.Find(_=>true).ToListAsync();
            var indicatorTasks = result.Select(x => GetIndicator(x.Instrument,indicator,x.Time));

            var indicators = await Task.WhenAll(indicatorTasks);

            var forexPrices = result.Select((priceMongo)=>_mapper.Map<ForexPriceIndicator>(priceMongo)).ToList();
            foreach(var ind in indicators)
            {
                forexPrices.Find(x => x.Instrument == ind.Pair).Indicator = ind.Indicator;
                forexPrices.Find(x => x.Instrument == ind.Pair).IndicatorDisplay = ind.IndicatorDisplay;
            }

            return forexPrices.OrderBy( x => x.Instrument).ToList();

        }

        private async Task<ForexIndicator> GetIndicator(string pair,string indicator,DateTime latestDate)
        {
            string currentDate = latestDate.ToString("yyyy-MM-dd");
            return await _indicatorMap.GetIndicator(pair,indicator,currentDate,14);
        }
    }    
}