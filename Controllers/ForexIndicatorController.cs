using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using forex_app_service.Mapper;

namespace forex_app_service.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ForexIndicatorController : ControllerBase
    {
        private readonly ForexIndicatorMap _forexIndicatorMap;
        public ForexIndicatorController(ForexIndicatorMap forexIndicatorMap)
        {   
            _forexIndicatorMap = forexIndicatorMap;
        }
        // GET api/values
        [HttpGet]
        public  ActionResult<string> Get()
        {
            return "value1";
        }

        // GET api/values/5
        [HttpGet("{indicator}/{pair}/{enddate}/{duration}")]
        public async Task<ActionResult> Get(string indicator,string pair,string enddate, int duration)
        {
            var pricesVar = new 
            { 
                prices=await _forexIndicatorMap.GetIndicator(pair,indicator,enddate,duration)
            };
            return Ok(pricesVar);
        }
    }
}