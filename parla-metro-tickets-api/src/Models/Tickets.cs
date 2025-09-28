using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace parla_metro_tickets_api.src.Models
{
    // Modelo que representa un ticket en MongoDB
    public class Tickets
    {
        // Identificador único interno de MongoDB
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        // Identificador único del ticket en formato GUID
        [BsonElement("ticketID")]
        [BsonRepresentation(BsonType.String)]
        public Guid TicketID { get; set; } = Guid.NewGuid();
        
        // Identificador del pasajero
        [BsonElement("idPassenger")]
        public string IdPassenger { get; set; } = null!;

        // Fecha del ticket
        [BsonElement("date")]
        public DateTime Date { get; set;}

        // Tipo de ticket: "Ida" o "Vuelta"
        [BsonElement("type")]
        public string Type { get; set; } = null!;

        // Estado del ticket: "Activo", "Usado" o "Caducado"
        [BsonElement("status")]
        public string Status { get; set; } = null!;

        // Monto pagado por el ticket
        [BsonElement("amountPaid")]
        public decimal AmountPaid { get; set; }

        // Marca si el ticket ha sido eliminado lógicamente
        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
