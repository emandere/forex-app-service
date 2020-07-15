using System;
using System.Text.Json.Serialization;
namespace forex_app_service.Models
{
    public partial class ForexQuotesDTO
    {
        [JsonPropertyName("instrument")]
        public string Instrument { get; set; }

        [JsonPropertyName("granularity")]
        public string Granularity { get; set; }

        [JsonPropertyName("candles")]
        public Candle[] Candles { get; set; }
    }

    public partial class Candle
    {
        [JsonPropertyName("complete")]
        public bool Complete { get; set; }

        [JsonPropertyName("volume")]
        public long Volume { get; set; }

        [JsonPropertyName("time")]
        public string Time { get; set; }

        [JsonPropertyName("bid")]
        public Ask Bid { get; set; }

        [JsonPropertyName("ask")]
        public Ask Ask { get; set; }
    }

    public partial class Ask
    {
        [JsonPropertyName("o")]
        public string O { get; set; }

        [JsonPropertyName("h")]
        public string H { get; set; }

        [JsonPropertyName("l")]
        public string L { get; set; }

        [JsonPropertyName("c")]
        public string C { get; set; }
    }

}
