namespace parla_metro_tickets_api.src.Data
{
    // Clase que almacena la configuración necesaria para conectarse a MongoDB
    public class MongoDbSettings
    {
        // Cadena de conexión a MongoDB
        public string ConnectionString { get; set; }  = null!;

        // Nombre de la base de datos a utilizar
        public string DatabaseName { get; set; }  = null!;
    }
}
