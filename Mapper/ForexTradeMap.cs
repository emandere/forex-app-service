using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Net.Http;
using forex_app_service.Models;
using System.Text.Json;
using System.Net.Http.Headers;

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
            string url = $"https://api-fxtrade.oanda.com/v3/accounts/{_settings.Value.ForexAccount}/openTrades";
            return await GetAsync<ForexOpenTradesDTO>(url);
        }

        private async Task<T> GetAsync<T>(string url)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.Value.Token);
            var responseBody = await client.GetStringAsync(url);
            var data = JsonSerializer.Deserialize<T>(responseBody);
            return data;
        }
    }
}