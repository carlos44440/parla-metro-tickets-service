using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using parla_metro_tickets_api.src.Interfaces;
using parla_metro_tickets_api.src.Models;
using parla_metro_tickets_api.src.DTOs;
using MongoDB.Driver;
using parla_metro_tickets_api.src.Data;

namespace parla_metro_tickets_api.src.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly IMongoCollection<Tickets> _tickets;

        public TicketRepository(MongoDbContext context)
        {
            _tickets = context.GetCollection<Tickets>("tickets");
        }

        public async Task<Tickets> CreateAsync(CreateTicketDto newTicket)
        {
            var ticket = new Tickets
            {
                Id = newTicket.Id,
                IdPassenger = newTicket.IdPassenger,
                Date = newTicket.Date,
                Type = newTicket.Type,
                Status = newTicket.Status,
                AmountPaid = newTicket.AmountPaid,
                IsDeleted = false
            };
            await _tickets.InsertOneAsync(ticket);
            return ticket;
        }
 
        public async Task<GetTicketByIdDto?> GetByIdAsync(string id)
        {
            var ticket = await _tickets.Find(t => t.Id == id && !t.IsDeleted).FirstOrDefaultAsync();
            if (ticket == null)
            {
                return null;
            }

            var ticketDto = new GetTicketByIdDto
            {
                Id = ticket.Id,
                IdPassenger = ticket.IdPassenger,
                Date = ticket.Date,
                Type = ticket.Type,
                AmountPaid = ticket.AmountPaid
            };
            return ticketDto;
        }

        public async Task<IEnumerable<GetAllTicketsDto>> GetAllAsync()
        {
            var tickets = await _tickets.Find(ticket => !ticket.IsDeleted).ToListAsync();
            if (tickets == null || tickets.Count == 0)
            {
                return Enumerable.Empty<GetAllTicketsDto>();
            }

            return tickets.Select(ticket => new GetAllTicketsDto
            {
                Id = ticket.Id,
                IdPassenger = ticket.IdPassenger,
                Date = ticket.Date,
                Type = ticket.Type,
                Status = ticket.Status,
                AmountPaid = ticket.AmountPaid
            });
        }

        public async Task<Tickets?> UpdateAsync(string id, UpdateTicketDto updatedTicket)
        {
            var existingTicket = await _tickets.Find(t => t.Id == id && !t.IsDeleted).FirstOrDefaultAsync();

            //Verifica si el ticket existe.
            if (existingTicket == null)
            {
                return null;
            }

            //No se permite reactivar un ticket caducado.
            if (existingTicket.Status.ToLower() == "caducado" && updatedTicket.Status.ToLower() == "activo")
            {
                throw new Exception("No se puede reactivar un ticket caducado.");
            }

            existingTicket.Date = updatedTicket.Date;
            existingTicket.Type = updatedTicket.Type;
            existingTicket.Status = updatedTicket.Status;
            existingTicket.AmountPaid = updatedTicket.AmountPaid;

            await _tickets.ReplaceOneAsync(t => t.Id == id, existingTicket);
            return existingTicket;
        }
      
        public async Task<Tickets?> DeleteAsync(string id)
        {
            var ticket = await _tickets.Find(t => t.Id == id && !t.IsDeleted).FirstOrDefaultAsync();
            if (ticket == null)
            {
                return null;
            }
            ticket.IsDeleted = true;
            await _tickets.ReplaceOneAsync(t => t.Id == id, ticket);
            return ticket;
        }
    }
}