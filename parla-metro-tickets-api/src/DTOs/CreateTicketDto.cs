using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace parla_metro_tickets_api.src.DTOs
{
    public class CreateTicketDto
    {   
        [Required]
        public string IdPassenger { get; set; }  = null!;
 
        [Required]
        public DateTime Date { get; set;} = DateTime.Now;

        [Required]
        [RegularExpression(@"Ida|Vuelta", ErrorMessage = "El tipo debe ser uno de los valores 'Ida|Vuelta'.")]
        public string Type { get; set; }  = null!;

        [Required]
        [RegularExpression(@"Activo|Usado|Caducado", ErrorMessage = "El estado debe ser uno de los valores 'Activo|Usado|Caducado'.")]
        public string Status { get; set; }  = "Activo";

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto pagado debe ser mayor que cero.")]
        public decimal AmountPaid { get; set; }
    }
}