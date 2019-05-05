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
        public IMongoCollection<ForexPriceMongo> Prices
        {
            get
            {
                return _database.GetCollection<ForexPriceMongo>("rawpriceslatest");
            }
        }
    }

}