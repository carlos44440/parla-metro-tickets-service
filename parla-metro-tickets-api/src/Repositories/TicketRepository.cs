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
            var tickets = await _tickets.Find(t => t.IdPassenger == newTicket.IdPassenger && !t.IsDeleted).ToListAsync();

            if (tickets != null && tickets.Count > 0)
            {
                for (int i = 0; i < tickets.Count; i++)
                {
                    if (tickets[i].Status.ToLower() == newTicket.Status.ToLower() && tickets[i].Type.ToLower() == newTicket.Type.ToLower() &&
                        tickets[i].Date.Date == newTicket.Date.Date && tickets[i].AmountPaid == newTicket.AmountPaid)
                    {
                        throw new Exception("Ya existe un ticket con los mismos datos.");
                    }
                }
            }

            var ticket = new Tickets
            {
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
 
        public async Task<GetTicketByIdDto?> GetByIdAsync(Guid ticketId)
        {
            var ticket = await _tickets.Find(t => t.TicketID == ticketId && !t.IsDeleted).FirstOrDefaultAsync();
            if (ticket == null)
            {
                return null;
            }

            var ticketDto = new GetTicketByIdDto
            {
                TicketID = ticket.TicketID,
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
                TicketID = ticket.TicketID,
                IdPassenger = ticket.IdPassenger,
                Date = ticket.Date,
                Type = ticket.Type,
                Status = ticket.Status,
                AmountPaid = ticket.AmountPaid
            });
        }

        public async Task<Tickets?> UpdateAsync(Guid ticketId, UpdateTicketDto updatedTicket)
        {
            var existingTicket = await _tickets.Find(t => t.TicketID == ticketId && !t.IsDeleted).FirstOrDefaultAsync();

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

            if (existingTicket.Status.ToLower() == "caducado" && updatedTicket.Status.ToLower() == "usado")
            {
                throw new Exception("No se permite cambiar el estado de un ticket caducado.");
            }

            if (existingTicket.Status.ToLower() == "usado" && updatedTicket.Status.ToLower() == "activo")
            {
                throw new Exception("No se puede reactivar un ticket usado.");

            }

            var tickets = await _tickets.Find(t => t.IdPassenger == existingTicket.IdPassenger && !t.IsDeleted).ToListAsync();

            if (tickets != null && tickets.Count > 0)
            {
                for (int i = 0; i < tickets.Count; i++)
                {
                    if (tickets[i].Status.ToLower() == updatedTicket.Status.ToLower() && tickets[i].Type.ToLower() == updatedTicket.Type.ToLower() &&
                        tickets[i].Date.Date == updatedTicket.Date.Date && tickets[i].AmountPaid == updatedTicket.AmountPaid)
                    {
                        throw new Exception("Ya existe un ticket con los mismos datos.");
                    }
                }
            }

            existingTicket.Date = updatedTicket.Date;
            existingTicket.Type = updatedTicket.Type;
            existingTicket.Status = updatedTicket.Status;
            existingTicket.AmountPaid = updatedTicket.AmountPaid;

            await _tickets.ReplaceOneAsync(t => t.TicketID == ticketId, existingTicket);
            return existingTicket;
        }
      
        public async Task<Tickets?> DeleteAsync(Guid ticketId)
        {
            var ticket = await _tickets.Find(t => t.TicketID == ticketId && !t.IsDeleted).FirstOrDefaultAsync();
            if (ticket == null)
            {
                return null;
            }
            ticket.IsDeleted = true;
            await _tickets.ReplaceOneAsync(t => t.TicketID == ticketId, ticket);
            return ticket;
        }
    }
}