using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace parla_metro_tickets_api.src.DTOs
{
    public class GetAllTicketsDto
    {
        public string Id { get; set; } = null!;
        
        public string IdPassenger { get; set; }  = null!;
 
        public DateTime Date { get; set;}

        public string Type { get; set; }  = null!;
    
        public string Status { get; set; }  = null!;

        public decimal AmountPaid { get; set; }
    }
}