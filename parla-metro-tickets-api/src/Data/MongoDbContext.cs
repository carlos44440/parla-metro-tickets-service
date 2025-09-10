using MongoDB.Driver;

namespace parla_metro_tickets_api.src.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoClient _client;

        public MongoDbContext(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
            _client = client;
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}