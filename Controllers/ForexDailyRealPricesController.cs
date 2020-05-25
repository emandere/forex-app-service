using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using forex_app_service.Mapper;
using forex_app_service.Models;

namespace forex_app_service.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ForexDailyRealPricesController : ControllerBase
    {
        private readonly ForexPriceMap _forexPriceMap;
        public ForexDailyRealPricesController(ForexPriceMap forexPriceMap)
        {   
            _forexPriceMap = forexPriceMap;
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] IEnumerable<ForexPriceDTO> prices)
        {
            await _forexPriceMap.AddPrices(prices);
            return Ok("success");
        }

        [HttpGet("{date}")]
        public async Task<ActionResult> Get(string date)
        {
            var dailyRealPrices = await _forexPriceMap.GetPrices(date);
            var dailyPrice = new 
            {
                prices = dailyRealPrices
            };
            return Ok(dailyPrice);
        }

        [HttpGet("{pair}/{date}")]
        public async Task<ActionResult> Get(string pair,string date)
        {
            var dailyRealPrices = await _forexPriceMap.GetPrices(pair,date);
            var dailyPrice = new 
            {
                prices = dailyRealPrices
            };
            return Ok(dailyPrice);
        }

       
    }
}
