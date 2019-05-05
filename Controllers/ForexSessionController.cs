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
    public class ForexController : ControllerBase
    {
        private readonly ForexPriceMap _forexPriceMap;
        public ForexController(ForexPriceMap forexPriceMap)
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
            return Ok(JsonConvert.SerializeObject(pricesVar));
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
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
