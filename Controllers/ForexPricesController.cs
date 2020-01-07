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
    public class ForexPricesController : ControllerBase
    {
        private readonly ForexPriceMap _forexPriceMap;
        public ForexPricesController(ForexPriceMap forexPriceMap)
        {   
            _forexPriceMap = forexPriceMap;
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var pricesVar = new 
            { 
                prices=await _forexPriceMap.GetLatestPrices()
            };
            return Ok(pricesVar);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ForexPriceDTO price)
        {
            await _forexPriceMap.AddPrice(price);
            return Ok("success");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
