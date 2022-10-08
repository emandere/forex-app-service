using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using forex_app_service.Mapper;
using forex_app_service.Models;

namespace forex_app_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly UploadMap _uploadMap;
       

        public UploadController(UploadMap uploadMap, IOptions<Settings> settings)
        {   
            _uploadMap = uploadMap;
        }

        public async Task<ActionResult> Post([FromBody] IEnumerable<ForexDailyPriceDTO> prices)
        {
            await _uploadMap.UploadDailyPrice(prices);
            return Ok("success");
        }
    }
}