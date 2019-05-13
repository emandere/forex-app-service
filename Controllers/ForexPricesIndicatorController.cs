using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using forex_app_service.Mapper;

namespace forex_app_service.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ForexPricesIndicatorController : ControllerBase
    {
        private readonly ForexPriceIndicatorMap _forexPriceIndicatorMap;
        public ForexPricesIndicatorController(ForexPriceIndicatorMap forexPriceIndicatorMap)
        {   
            _forexPriceIndicatorMap = forexPriceIndicatorMap;
        }
        // GET api/values
        [HttpGet]
        public  ActionResult<string> Get()
        {
            return "value1";
        }

        // GET api/values/5
        [HttpGet("{indicator}")]
        public async Task<ActionResult> Get(string indicator)
        {
            var pricesVar = new 
            { 
                prices=await _forexPriceIndicatorMap.GetLatestPrices(indicator)
            };
            return Ok(JsonConvert.SerializeObject(pricesVar));
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
