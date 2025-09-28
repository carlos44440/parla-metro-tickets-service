using MongoDB.Driver;

namespace parla_metro_tickets_api.src.Data
{
    // Clase que representa el contexto de conexión con MongoDB
    public class MongoDbContext
    {
        // Referencia a la base de datos MongoDB
        private readonly IMongoDatabase _database;

        // Cliente de conexión a MongoDB
        private readonly IMongoClient _client;

        // Constructor que inicializa la conexión con MongoDB
        public MongoDbContext(MongoDbSettings settings)
        {
            try
            {
                // Configuración del cliente MongoDB a partir del connection string
                var clientSettings = MongoClientSettings.FromConnectionString(settings.ConnectionString);
                
                // Tiempo máximo para encontrar el servidor
                clientSettings.ServerSelectionTimeout = TimeSpan.FromSeconds(30);
                
                // Tiempo máximo para establecer la conexión
                clientSettings.ConnectTimeout = TimeSpan.FromSeconds(30);
                
                // Inicializa el cliente con la configuración definida
                _client = new MongoClient(clientSettings);
                
                // Obtiene la base de datos especificada en la configuración
                _database = _client.GetDatabase(settings.DatabaseName);
                
                // Verifica la conexión listando los nombres de las bases de datos
                _client.ListDatabaseNames();
            }
            catch (Exception ex)
            {
                // Captura errores de conexión y los muestra en consola
                Console.WriteLine($"Error de conexión MongoDB: {ex.Message}");
                throw;
            }
        }

        // Método genérico para obtener una colección de MongoDB
        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}
