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
            var result = await _context.ForexSessions.Find(x=>x.SessionType=="SessionType.live" || x.SessionType=="live").ToListAsync();
            return result.Select((sessionMongo)=>_mapper.Map<ForexSession>(sessionMongo)).ToList();
        }

        public async Task<List<ForexSession>> GetLiveSession(string sessionId)
        {
            var result = await _context.ForexSessions.Find(x=>x.Id == sessionId).ToListAsync();
            return result.Select((sessionMongo)=>_mapper.Map<ForexSession>(sessionMongo)).ToList();
        }

        public async Task ExecuteTrade(string sessionId, ForexTradeDTO trade)
        {
            var sessionList = await GetLiveSession(sessionId);
            sessionList[0].ExecuteTrade(trade.Pair,trade.Price,trade.Units,trade.StopLoss,trade.TakeProfit,trade.Long,trade.Date);
            await UpdateSessionHelper(sessionList[0]);
        }

        public async Task UpdateSession(string sessionId, ForexPriceDTO price)
        {
            var sessionList = await GetLiveSession(sessionId);
            sessionList[0].UpdateSession(price.Instrument,price.Bid,price.Ask,price.Time.ToString());
            await UpdateSessionHelper(sessionList[0]);
            //sessionList[0].ExecuteTrade(trade.Pair,trade.Price,trade.Units,trade.StopLoss,trade.TakeProfit);
        }

        public async Task UpdateSessionHelper(ForexSession sess)
        {
            var sessionMongo = _mapper.Map<ForexSessionMongo>(sess);
            var replace =await  _context.ForexSessions.ReplaceOneAsync(sess => sess.Id==sessionMongo.Id,sessionMongo);
        }

        public  async Task DeleteSession(string id)
        {
            await _context.ForexSessions.DeleteOneAsync(sess => sess.Id == id);
        }

        public async Task SaveSessions(ForexSessionsDTO session)
        {
            foreach(var sessionIn in session.sessions)
            {
                //var sessionIn = _mapper.Map<ForexSessionDTO>(session);  
                var sessionModel = _mapper.Map<ForexSession>(sessionIn);    
                var sessionMongo = _mapper.Map<ForexSessionMongo>(sessionModel);
                sessionMongo.idinfo = sessionIn.Id;
                sessionMongo.ExperimentId="NoExperiment";
                var findSession =  await _context.ForexSessions.CountDocumentsAsync(x=>x.Id==sessionMongo.Id);    
                if(findSession == 0)
                {
                    Console.WriteLine($"Adding Session {sessionIn.Id}");
                    await _context.ForexSessions.InsertOneAsync( _mapper.Map<ForexSessionMongo>(sessionMongo));
                }
                else
                {
                    Console.WriteLine($"Updating Session {sessionIn.Id}");
                    var replace =await  _context.ForexSessions.ReplaceOneAsync(sess => sess.Id==sessionMongo.Id,sessionMongo);
                }

            }
        }
    }    
}