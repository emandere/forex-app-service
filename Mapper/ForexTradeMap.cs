using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Net.Http;
using forex_app_service.Models;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Text;

namespace forex_app_service.Mapper
{
    public class ForexTradeMap
    {
        static readonly HttpClient client = new HttpClient();
        private readonly IOptions<Settings> _settings;
        public ForexTradeMap(IOptions<Settings> settings)
        {
           _settings = settings;
        }

        public async Task<ForexOpenTradesDTO> GetOpenTrades()
        {
            string url = $"{_settings.Value.URL}/v3/accounts/{_settings.Value.ForexAccount}/openTrades";
            return await GetAsync<ForexOpenTradesDTO>(url);
        }

        public async Task<HttpResponseMessage> ExecuteTrade(ForexTradeDTO tradeIn)
        {
            string precision = tradeIn.Pair == "USD_JPY" ? "N2" : "N4";
            int position = tradeIn.Long ? 1 : -1;
            var tradeOut = new ForexRealTradeDto
            {
                Order = new RealOrder
                {
                    Units = (tradeIn.Units * position).ToString(),
                    Instrument = tradeIn.Pair,
                    TimeInForce = "FOK",
                    Type = "MARKET",
                    PositionFill = "DEFAULT",
                    StopLossOnFill = new OnFill
                    {
                        Price = tradeIn.StopLoss.ToString(precision)
                    },
                    TakeProfitOnFill = new OnFill
                    {
                        Price = tradeIn.TakeProfit.ToString(precision)
                    }
                }
            };
            string url = $"{_settings.Value.URL}/v3/accounts/{_settings.Value.ForexAccount}/orders";
            return await PostAsync<ForexRealTradeDto>(tradeOut,url);
        }

        private async Task<T> GetAsync<T>(string url)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.Value.Token);
            var responseBody = await client.GetStringAsync(url);
            var data = JsonSerializer.Deserialize<T>(responseBody);
            return data;
        }

        private async Task<HttpResponseMessage> PostAsync<T>(T dto,string url)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.Value.Token);
            var stringPrice= JsonSerializer.Serialize<T>(dto);
            var stringPriceContent = new StringContent(stringPrice,UnicodeEncoding.UTF8,"application/json");
            var responsePriceBody = await client.PostAsync(url,stringPriceContent);
            return responsePriceBody;
        }
    }
}