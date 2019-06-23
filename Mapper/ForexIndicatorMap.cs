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
            string indValueDisplay = indValue.ToString(); 
            switch(indicator)
            {
                case "BelowBollinger":
                    indValue = Stats.BollingerLower(result.Select(z => z.Close).ToList());
                    indValueDisplay = indValue.ToString();
                    break;
                case "RSI":
                    indValue = Stats.RSI(result.Select(z=> new List<double>{z.Open,z.Close}));
                    indValueDisplay = Convert.ToInt32(indValue).ToString();
                    break;
                default:
                    break;    
            }                          
            
            ForexIndicator ind = new ForexIndicator
            {
                Pair = pair,
                StartDate = startdate,
                EndDate = enddt,
                Indicator = indValue,
                IndicatorDisplay = indValueDisplay

            };

            return ind;                           

        }   
    }
}