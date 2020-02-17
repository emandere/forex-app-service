using MongoDB.Driver;
using Microsoft.Extensions.Options;
using forex_app_service.Models;
namespace forex_app_service.Mapper
{
    public class DbContext
    {
        private readonly IMongoDatabase _database = null;
        public DbContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }
        public IMongoCollection<ForexRealPriceMongo> Prices
        {
            get
            {
                return _database.GetCollection<ForexRealPriceMongo>("rawprices");
            }
        }

        public IMongoCollection<ForexPriceMongo> LatestPrices
        {
            get
            {
                return _database.GetCollection<ForexPriceMongo>("rawpriceslatest");
            }
        }

        public IMongoCollection<ForexDailyPriceMongo> DailyPrices
        {
            get
            {
                return _database.GetCollection<ForexDailyPriceMongo>("forexdailyprices");
            }
        }

        public IMongoCollection<ForexSessionMongo> ForexSessions
        {
            get
            {
                return _database.GetCollection<ForexSessionMongo>("session");
            }
        }
    }

}