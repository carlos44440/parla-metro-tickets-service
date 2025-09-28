using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace parla_metro_tickets_api.src.Models
{
    public class Tickets
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("ticketID")]
        [BsonRepresentation(BsonType.String)]
        public Guid TicketID { get; set; } = Guid.NewGuid();
        
        [BsonElement("idPassenger")]
        public string IdPassenger { get; set; } = null!;

        [BsonElement("date")]
        public DateTime Date { get; set;}

        [BsonElement("type")]
        public string Type { get; set; } = null!;

        [BsonElement("status")]
        public string Status { get; set; } = null!;

        [BsonElement("amountPaid")]
        public decimal AmountPaid { get; set; }

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; }
    }
}