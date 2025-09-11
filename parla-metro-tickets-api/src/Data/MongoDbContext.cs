using MongoDB.Driver;

namespace parla_metro_tickets_api.src.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoClient _client;

        public MongoDbContext(MongoDbSettings settings)
        {
            try
            {
                var clientSettings = MongoClientSettings.FromConnectionString(settings.ConnectionString);
                clientSettings.ServerSelectionTimeout = TimeSpan.FromSeconds(30);
                clientSettings.ConnectTimeout = TimeSpan.FromSeconds(30);
                
                _client = new MongoClient(clientSettings);
                _database = _client.GetDatabase(settings.DatabaseName);
                
                _client.ListDatabaseNames();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de conexión MongoDB: {ex.Message}");
                throw;
            }
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}