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
    public class ForexDebugController : ControllerBase
    {
        private readonly ForexIndicatorMap _forexIndicatorMap;
        public ForexDebugController(ForexIndicatorMap forexIndicatorMap)
        {   
            _forexIndicatorMap = forexIndicatorMap;
        }
        // GET api/values
        [HttpGet]
        public  ActionResult<string> Get()
        {
            return "value1";
        }

        [HttpGet("{indicator}/{pair}/{enddate}/{duration}")]
        public async Task<ActionResult> Get(string indicator,string pair,string enddate, int duration)
        {
            var pricesVar = new 
            { 
                prices=await _forexIndicatorMap.GetDebugInfo(pair,indicator,enddate,duration)
            };
            return Ok(pricesVar);
        }
    }
}