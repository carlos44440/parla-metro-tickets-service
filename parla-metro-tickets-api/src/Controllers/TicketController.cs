using Microsoft.AspNetCore.Mvc;
using parla_metro_tickets_api.src.DTOs;
using parla_metro_tickets_api.src.Helper;
using parla_metro_tickets_api.src.Interfaces;

namespace parla_metro_tickets_api.src.Controllers
{
    // Controlador encargado de gestionar las operaciones CRUD de Tickets
    public class TicketController : ControllerBase
    {
        // Repositorio de tickets inyectado mediante dependencia
        private readonly ITicketRepository _ticketRepository;

        // Constructor que recibe el repositorio
        public TicketController(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        // POST: api/tickets
        // Crea un nuevo ticket
        [HttpPost("api/tickets")]
        public async Task<IActionResult> CreateTicket([FromForm] CreateTicketDto newTicket)
        {
            // Verifica que el modelo sea válido antes de continuar
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            try
            {
                // Llama al repositorio para crear el ticket
                var createdTicket = await _ticketRepository.CreateAsync(newTicket);
                return Ok(createdTicket);
            }
            catch (Exception ex)
            {
                // Devuelve error 500 si ocurre una excepción
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/tickets/{id}
        // Obtiene un ticket por su identificador
        [HttpGet("api/tickets/{id}")]
        public async Task<IActionResult> GetTicketById(Guid id)
        {
            try
            {
                var ticket = await _ticketRepository.GetByIdAsync(id);
                if (ticket == null) return NotFound(new { message = "Ticket no encontrado." });
                return Ok(ticket);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/tickets
        // Obtiene todos los tickets, con soporte para filtros y paginación
        [HttpGet("api/tickets")]
        public async Task<IActionResult> GetAllTickets([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var tickets = await _ticketRepository.GetAllAsync(query);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // PUT: api/tickets/{id}
        // Actualiza un ticket existente
        [HttpPut("api/tickets/{id}")]
        public async Task<IActionResult> UpdateTicket(Guid id, [FromForm] UpdateTicketDto updatedTicket)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var ticket = await _ticketRepository.UpdateAsync(id, updatedTicket);
                if (ticket == null) return NotFound(new { message = "Ticket no encontrado." });
                return Ok(ticket);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // DELETE: api/tickets/{id}
        // Elimina un ticket existente
        [HttpDelete("api/tickets/{id}")]
        public async Task<IActionResult> DeleteTicket(Guid id)
        {
            try
            {
                var ticket = await _ticketRepository.DeleteAsync(id);
                if (ticket == null) return NotFound(new { message = "Ticket no encontrado." });
                return Ok(ticket);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
