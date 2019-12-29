using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using forex_app_service.Mapper;
using forex_app_service.Models;

namespace forex_app_service.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ForexPricesIndicatorController : ControllerBase
    {
        private readonly ForexPriceIndicatorMap _forexPriceIndicatorMap;
        private readonly IMapper _mapper;
        public ForexPricesIndicatorController(IMapper mapper,ForexPriceIndicatorMap forexPriceIndicatorMap)
        {   
            _forexPriceIndicatorMap = forexPriceIndicatorMap;
            _mapper = mapper;
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
            var prices=await _forexPriceIndicatorMap.GetLatestPrices(indicator);
            var pricesDTO = prices.Select((price)=>_mapper.Map<ForexPriceIndicatorDTO>(price)).ToList();
            var pricesVar = new 
            { 
                prices = pricesDTO
            };
            return Ok(pricesVar);
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
