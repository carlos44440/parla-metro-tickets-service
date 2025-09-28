using System;
using System.ComponentModel.DataAnnotations;

namespace parla_metro_tickets_api.src.DTOs
{
    // DTO usado para actualizar la información de un ticket existente
    public class UpdateTicketDto
    {
        // Fecha del ticket (requerida), por defecto se asigna la fecha actual
        [Required]
        public DateTime Date { get; set;} = DateTime.Now;

        // Tipo de ticket: "Ida" o "Vuelta" (requerido)
        [Required]
        [RegularExpression(@"Ida|Vuelta", ErrorMessage = "El tipo debe ser uno de los valores 'Ida|Vuelta'.")]
        public string Type { get; set; } = null!;

        // Estado del ticket: "Activo", "Usado" o "Caducado" (requerido)
        [Required]
        [RegularExpression(@"Activo|Usado|Caducado", ErrorMessage = "El estado debe ser uno de los valores 'Activo|Usado|Caducado'.")]
        public string Status { get; set; } = null!;

        // Monto pagado por el ticket (requerido, mayor que 0)
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto pagado debe ser mayor que cero.")]
        public decimal AmountPaid { get; set; }
    }
}
