using System;
using System.Text.Json.Serialization;
namespace forex_app_service.Models
{
    public class ForexTradeDTO
    {
        [JsonPropertyName("pair")]
        public string Pair { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; }
    }
}