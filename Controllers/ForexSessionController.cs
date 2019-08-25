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
    public class ForexSessionController : ControllerBase
    {
        private readonly ForexSessionMap _forexSessionMap;
        public ForexSessionController(ForexSessionMap forexSessionMap)
        {   
            _forexSessionMap = forexSessionMap;
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var sessionsVar = new 
            { 
                sessions=await _forexSessionMap.GetLiveSessions()
            };
            return Ok(JsonConvert.SerializeObject(sessionsVar));
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id)
        {
            var sessionsVar = new 
            { 
                sessions=await _forexSessionMap.GetLiveSession(id)
            };
            return Ok(JsonConvert.SerializeObject(sessionsVar));
        }

        // GET api/values/5
        [HttpGet("{id}/{pair}")]

        public async Task<ActionResult> Get(string id,string pair)
        {
            var sessions=await _forexSessionMap.GetLiveSession(id);
            var accountsbypair = sessions.Select(x=>x.GetAccountByPair(pair)).ToArray();
            var AccountByPair = new 
            { 
                accounts = accountsbypair
            };
            return Ok(JsonConvert.SerializeObject(accountsbypair));
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
