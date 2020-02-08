using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using forex_app_service.Domain;
using forex_app_service.Models;
namespace forex_app_service.Mapper
{
    public class ForexSessionMap
    {
        private readonly IMapper _mapper;
        private readonly DbContext _context = null;
         public ForexSessionMap(IMapper mapper,IOptions<Settings> settings)
        {
            _mapper = mapper;
            _context = new DbContext(settings);;
        }

        public async Task<List<ForexSession>> GetLiveSessions()
        {
            var result = await _context.ForexSessions.Find(x=>x.SessionType=="SessionType.live").ToListAsync();
            return result.Select((sessionMongo)=>_mapper.Map<ForexSession>(sessionMongo)).ToList();
        }

        public async Task<List<ForexSession>> GetLiveSession(string sessionId)
        {
            var result = await _context.ForexSessions.Find(x=>x.Id == sessionId).ToListAsync();
            return result.Select((sessionMongo)=>_mapper.Map<ForexSession>(sessionMongo)).ToList();
        }

        public async Task SaveSessions(IEnumerable<ForexSessionInDTO> sessions)
        {
            foreach(var session in sessions)
            {
                var sessionIn = _mapper.Map<ForexSessionDTO>(session);  
                var sessionModel = _mapper.Map<ForexSession>(sessionIn);    
                var sessionMongo = _mapper.Map<ForexSessionMongo>(sessionModel);
                sessionMongo.idinfo = sessionIn.Id;
                sessionMongo.ExperimentId="NoExperiment";
                var replace =await  _context.ForexSessions.ReplaceOneAsync(sess => sess.Id==sessionMongo.Id,sessionMongo);
            }
        }
    }    
}