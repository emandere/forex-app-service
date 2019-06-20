using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using forex_app_service.Domain;
using forex_app_service.Domain.Indicators;

using forex_app_service.Models;
namespace forex_app_service.Mapper
{
    public class ForexIndicatorMap
    {
        private readonly IMapper _mapper;
        private readonly DbContext _context = null;
        public ForexIndicatorMap(IMapper mapper,IOptions<Settings> settings)
        {
            _mapper = mapper;
            _context = new DbContext(settings);
        }

        public async Task<ForexIndicator> GetIndicator(string pair,string indicator,string enddt,int duration)
        {
            DateTime enddate =  DateTime.Parse(enddt);
            DateTime startdt = enddate.AddDays(-duration).ToLocalTime();

            string startdate = startdt.ToString("yyyy-MM-dd");
           
            

            var result = await _context.DailyPrices
                                       .Find( x => x.Pair == pair
                                                && x.Datetime > startdt
                                                && x.Datetime < enddate)
                                      
                                       .ToListAsync();
            double indValue = 0; 
            switch(indicator)
            {
                case "BelowBollinger":
                    indValue = Stats.BollingerLower(result.Select(z => z.Close).ToList());
                    break;
                default:
                    break;    
            }                          
            
            ForexIndicator ind = new ForexIndicator
            {
                StartDate = startdate,
                EndDate = enddt,
                Indicator = Stats.BollingerLower(result.Select(z => z.Close).ToList()),
                IndicatorDisplay = indicator.ToString()

            };

            return ind;                           

        }   
    }
}