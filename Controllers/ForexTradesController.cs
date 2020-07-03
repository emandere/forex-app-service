using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using forex_app_service.Mapper;
using forex_app_service.Models;
namespace forex_app_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForexTradeController : ControllerBase
    {
        private readonly ForexTradeMap _forexTradeMap;

        public ForexTradeController(ForexTradeMap forexTradeMap)
        {   
            _forexTradeMap = forexTradeMap;
        }

        [HttpGet]
        public async Task<ActionResult> Get(string id)
        {
            var openTrades = await _forexTradeMap.GetOpenTrades();
            return Ok(openTrades);

        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ForexTradeDTO trade)
        {
            await _forexTradeMap.ExecuteTrade(trade);
            return Ok();
        }
    }
}