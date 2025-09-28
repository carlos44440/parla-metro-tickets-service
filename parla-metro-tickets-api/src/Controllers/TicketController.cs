using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using parla_metro_tickets_api.src.DTOs;
using parla_metro_tickets_api.src.Helper;
using parla_metro_tickets_api.src.Interfaces;

namespace parla_metro_tickets_api.src.Controllers
{
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketController(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        [HttpPost("api/tickets")]
        public async Task<IActionResult> CreateTicket([FromForm] CreateTicketDto newTicket)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            try
            {
                var createdTicket = await _ticketRepository.CreateAsync(newTicket);
                return Ok(createdTicket);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

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