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
    public class ForexRuleController : ControllerBase
    {
        private readonly ForexRuleMap _forexRuleMap;
        public ForexRuleController(ForexRuleMap forexRuleMap)
        {   
            _forexRuleMap = forexRuleMap;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok("wait");
        }

        [HttpGet("{rule}/{pair}/{enddate}/{window}")]
        public async Task<ActionResult> Get(string rule,string pair,string enddate,int window)
        {
            var ruleResult = await _forexRuleMap.GetRule(rule,pair,window,enddate);
            return Ok(ruleResult);
        }
    }
}
