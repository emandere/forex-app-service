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
    public class ForexDailyPricesController : ControllerBase
    {
        private readonly ForexDailyPriceMap _forexDailyPriceMap;
        public ForexDailyPricesController(ForexDailyPriceMap forexDailyPriceMap)
        {   
            _forexDailyPriceMap = forexDailyPriceMap;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok("wait");
        }

        // GET api/values/5
        [HttpGet("{pair}/{date}")]
        public async Task<ActionResult> Get(string pair,string date)
        {
            var dailyPrice = await _forexDailyPriceMap.GetDailyPrice(pair,date);
            return Ok(dailyPrice);
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] IEnumerable<ForexDailyPriceDTO> prices)
        {
            await _forexDailyPriceMap.AddDailyPrices(prices);
            return Ok("success");
        }

        // PUT api/values/5
        [HttpPut("{instrument}")]
        public async Task<ActionResult> Put(string instrument, [FromBody] ForexPriceDTO price)
        {
            await _forexDailyPriceMap.SaveRealTimePrice(instrument,price);
            return Ok("success");
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
