using System.ComponentModel.DataAnnotations;

namespace parla_metro_tickets_api.src.DTOs
{
    // DTO usado para crear un nuevo Ticket
    public class CreateTicketDto
    {   
        // Identificador del pasajero (requerido)
        [Required]
        public string IdPassenger { get; set; }  = null!;
 
        // Fecha en que se genera el ticket (requerido)
        [Required]
        public DateTime Date { get; set;}

        // Tipo de ticket: puede ser solo "Ida" o "Vuelta" (requerido)
        [Required]
        [RegularExpression(@"Ida|Vuelta", ErrorMessage = "El tipo debe ser uno de los valores 'Ida|Vuelta'.")]
        public string Type { get; set; }  = null!;

        // Estado del ticket: puede ser "Activo", "Usado" o "Caducado" (requerido)
        // Por defecto se asigna "Activo"
        [Required]
        [RegularExpression(@"Activo|Usado|Caducado", ErrorMessage = "El estado debe ser uno de los valores 'Activo|Usado|Caducado'.")]
        public string Status { get; set; }  = "Activo";

        // Monto pagado por el ticket (requerido, debe ser mayor que 0)
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto pagado debe ser mayor que cero.")]
        public decimal AmountPaid { get; set; }
    }
}
