using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using forex_app_service.Domain;
using forex_app_service.Models;
namespace forex_app_service.Mapper
{
    public class ForexPriceMap
    {
        private readonly IMapper _mapper;
        private readonly DbContext _context = null;
         public ForexPriceMap(IMapper mapper,IOptions<Settings> settings)
        {
            _mapper = mapper;
            _context = new DbContext(settings);;
        }

        public async Task<List<ForexPrice>> GetLatestPrices()
        {
            var result = await _context.Prices.Find(_=>true).ToListAsync();
            return result.Select((priceMongo)=>_mapper.Map<ForexPrice>(priceMongo)).ToList();
        }

        public async Task AddPrice(ForexPriceDTO item)
        {
        
            await _context.Prices.InsertOneAsync(_mapper.Map<ForexPriceMongo>(item));
            
        }

        public async Task SaveRealTimePrice(string instrument,ForexPriceDTO item)
        {
            var priceLatest = _mapper.Map<ForexPriceMongo>(item);
            priceLatest.Id = instrument + item.Time;
            //await _context.LatestPrices.ReplaceOneAsync(x=>x.Instrument==instrument,priceLatest);
            await _context.LatestPrices.DeleteOneAsync(x=>x.Instrument==instrument);
            await _context.LatestPrices.InsertOneAsync(priceLatest);
            
        }
    }    
}