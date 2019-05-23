using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using forex_app_service.Domain;
using forex_app_service.Models;
namespace forex_app_service.Mapper
{
    public class ForexPriceIndicatorMap
    {
        private readonly IMapper _mapper;
        private readonly DbContext _context = null;
        private readonly string _forexAppServiceBase;
         public ForexPriceIndicatorMap(IMapper mapper,IOptions<Settings> settings)
        {
            _mapper = mapper;
            _context = new DbContext(settings);
            _forexAppServiceBase = settings.Value.ForexAppServiceBase;
        }

        public async Task<List<ForexPriceIndicator>> GetLatestPrices(string indicator)
        {
            var result = await _context.Prices.Find(_=>true).ToListAsync();
            var indicatorTasks = result.Select(x => GetIndicator(x.Instrument,indicator));

            var indicators = await Task.WhenAll(indicatorTasks);

            var forexPrices = result.Select((priceMongo)=>_mapper.Map<ForexPriceIndicator>(priceMongo)).ToList();
            foreach(var ind in indicators)
            {
                forexPrices.Find(x => x.Instrument == ind[1]).Indicator = ind[0];
            }

            return forexPrices;

        }

        private async Task<List<string>> GetIndicator(string pair,string indicator)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"{_forexAppServiceBase}/dailyindicator/{indicator}/14/{pair}/20190513");
                string responseBody = await response.Content.ReadAsStringAsync();
                var indicatorList = JsonConvert.DeserializeObject<List<string>>(responseBody);
                indicatorList.Add(pair);
                return indicatorList;
            }
        }
    }    
}