using System.Text.Json.Serialization;
namespace forex_app_service.Models
{
    public  class ForexOpenTradesDTO
    {
        [JsonPropertyName("trades")]
        public ForexOpenTradeDTO [] Trades { get; set; }
    }

    public class ForexOpenTradeDTO
    {
        [JsonPropertyName("instrument")]
        public string Instrument { get; set; }

        [JsonPropertyName("initialUnits")]
        public string Units { get; set; }
    }
}
