using AutoMapper;
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
    public class ForexSessionController : ControllerBase
    {
        private readonly ForexSessionMap _forexSessionMap;
        private readonly IMapper _mapper;

        public ForexSessionController(IMapper mapper,ForexSessionMap forexSessionMap)
        {   
            _forexSessionMap = forexSessionMap;
            _mapper = mapper;
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var sessions=await _forexSessionMap.GetLiveSessions();
            var sessionsDTO = sessions.Select((session)=>_mapper.Map<ForexSessionDTO>(session)).ToList();
            var sessionsVar = new 
            { 
                sessions = sessionsDTO
            };
            
            return Ok(sessionsVar);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id)
        {
            var sessionsVar = new 
            { 
                sessions=await _forexSessionMap.GetLiveSession(id)
            };
            return Ok(sessionsVar);
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
            return Ok(accountsbypair);
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ForexSessionInDTO[] sessions)
        {
            await _forexSessionMap.SaveSessions(sessions);
            return Ok();
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
