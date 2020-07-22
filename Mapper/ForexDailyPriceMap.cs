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
    public class ForexDailyPriceMap
    {
        private readonly IMapper _mapper;
        private readonly DbContext _context = null;
         public ForexDailyPriceMap(IMapper mapper,IOptions<Settings> settings)
        {
            _mapper = mapper;
            _context = new DbContext(settings);;
        }

       

        public async Task AddDailyPrice(ForexDailyPriceDTO item)
        {
            var dailyPriceLatest = _mapper.Map<ForexDailyPriceMongo>(item);
            dailyPriceLatest.Id = item.Pair + item.Date;
           

            var findDailyPrice = await _context.DailyPrices.FindAsync(x => x.Id == dailyPriceLatest.Id);
            if(findDailyPrice!=null)
            {
                await _context.DailyPrices.DeleteOneAsync(x=>x.Id==dailyPriceLatest.Id);
                await _context.DailyPrices.InsertOneAsync(dailyPriceLatest); 
            }
            else
            {
                await _context.DailyPrices.InsertOneAsync(dailyPriceLatest);
            }
            
        }

        public async Task<ForexDailyPriceDTO> GetDailyPrice(string pair,string date)
        {
            DateTime min =  DateTime.ParseExact(date,"yyyyMMdd",CultureInfo.InvariantCulture);
            DateTime max = min.AddDays(1);
            var dailyPriceMongo = await _context.DailyPrices
                    .Find(x => x.Pair == pair)
                    .ToListAsync();
            var firstDailyPrice = dailyPriceMongo.Find(x => x.Datetime.ToString("yyyyMMdd")==date);
            return _mapper.Map<ForexDailyPriceDTO>(firstDailyPrice);
        }

        public async Task<List<ForexDailyPriceDTO>> GetPriceRange(string pair,string startdate,string enddate)
        {
            DateTime min =  DateTime.ParseExact(startdate,"yyyyMMdd",CultureInfo.InvariantCulture);
            DateTime max = DateTime.ParseExact(enddate,"yyyyMMdd",CultureInfo.InvariantCulture);
            var dailyPriceMongo = await _context.DailyPrices
                    .Find(x => x.Pair == pair && x.Datetime>=min && x.Datetime < max)
                    .ToListAsync();
            //var firstDailyPrice = dailyPriceMongo.Find(x => x.Datetime>=min && x.Datetime < max);
            return _mapper.Map<List<ForexDailyPriceDTO>>(dailyPriceMongo);
        }

        public async Task<List<ForexDailyPrice>> GetPriceRangeInternal(string pair,string startdate,string enddate)
        {
            DateTime min =  DateTime.ParseExact(startdate,"yyyy-MM-dd",CultureInfo.InvariantCulture);
            DateTime max = DateTime.ParseExact(enddate,"yyyy-MM-dd",CultureInfo.InvariantCulture);
            var dailyPriceMongo = await _context.DailyPrices
                    .Find(x => x.Pair == pair && x.Datetime>=min && x.Datetime <= max)
                    .ToListAsync();
            //var firstDailyPrice = dailyPriceMongo.Find(x => x.Datetime>=min && x.Datetime < max);
            return _mapper.Map<List<ForexDailyPrice>>(dailyPriceMongo);
        }

        public async Task<ForexDailyPriceDTO> GetLatestDailyPrice(string pair)
        {
           
            var dailyPriceMongo = await _context.DailyPrices
                    .Find(x => x.Pair == pair)
                    .SortByDescending(x => x.Datetime)
                    .Limit(1)
                    .SingleAsync();
           
            return _mapper.Map<ForexDailyPriceDTO>(dailyPriceMongo);
        }

        public async Task AddDailyPrices(IEnumerable<ForexDailyPriceDTO> item)
        {
            foreach(var price in item)
            {
                await AddDailyPrice(price);
            }    
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