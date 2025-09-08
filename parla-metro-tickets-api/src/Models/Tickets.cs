using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace parla_metro_tickets_api.src.Models
{
    public class Tickets
    {
        public int Id { get; set; }
        public string IdPassenger { get; set; }
        public DateTime Date { get; set;}
        public string Type { get; set; }
        public string Status { get; set; }
        public decimal AmountPaid { get; set; }
        public bool IsDeleted { get; set; }
    }
}