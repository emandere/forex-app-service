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
        //Mental Models
        public async Task<List<ForexPriceDTO>> GetLatestPrices()
        {
            var result = await _context.LatestPrices.Find(_=>true).ToListAsync();
            return result.Select((priceMongo)=>_mapper.Map<ForexPriceDTO>(priceMongo)).ToList();
        }

        public async Task AddPrice(ForexPriceDTO item)
        {
        
            await _context.Prices.InsertOneAsync(_mapper.Map<ForexRealPriceMongo>(item));
            
        }

        public async Task AddPrices(IEnumerable<ForexPriceDTO> items)
        {
            foreach(var price in items)
            {
                var findPrice =  await _context.Prices.CountDocumentsAsync(x=>x.Instrument==price.Instrument && x.Time == price.UTCTime);    
                if(findPrice == 0)
                {
                    await _context.Prices.InsertOneAsync( _mapper.Map<ForexRealPriceMongo>(price));
                }
             
            }
            
        }

        public async Task<List<ForexPriceDTO>> GetPrices(string date)
        {
            var startDate = DateTime.ParseExact(date,"yyyyMMdd",CultureInfo.InvariantCulture);
            var endDate = startDate.AddDays(1);
            var pricesMongo = await _context.Prices.Find(x => x.Time>=startDate && x.Time<endDate).ToListAsync();
            return _mapper.Map<List<ForexPriceDTO>>(pricesMongo);
        }

         public async Task<List<ForexPriceDTO>> GetPrices(string pair,string date)
        {
            var startDate = DateTime.ParseExact(date,"yyyyMMdd",CultureInfo.InvariantCulture);
            var endDate = startDate.AddDays(1);
            var pricesMongo = await _context.Prices.Find(x => x.Instrument == pair && x.Time>=startDate && x.Time<endDate).ToListAsync();
            return _mapper.Map<List<ForexPriceDTO>>(pricesMongo);
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