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

        public async Task<List<ForexPrice>> GetLatestPrices(string indicator)
        {
            var result = await _context.Prices.Find(_=>true).ToListAsync();
            return result.Select((priceMongo)=>_mapper.Map<ForexPrice>(priceMongo)).ToList();
        }
    }    
}