using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace parla_metro_tickets_api.src.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly IMongoCollection<Ticket> _tickets;

        public TicketRepository(MongoDbContext context)
        {
            _tickets = context.GetCollection<Ticket>("Tickets");
        }
 
        public async Task<Ticket> CreateAsync(Ticket ticket)
        {
            await _tickets.InsertOneAsync(ticket);
            return ticket;
        }
 
        public async Task<Ticket?> GetByIdAsync(int id)
        {
            var ticket = await _tickets.Find(t => t.Id == id && !t.IsDeleted)
                .Project(t => new 
                {
                    t.Id,
                    t.IdPassenger,
                    t.Date,
                    t.Type,
                    t.AmountPaid,
                })
                .FirstOrDefaultAsync();
            return ticket;
        }
 
        public async Task<IEnumerable<Ticket>> GetAllAsync()
        {
            return await _tickets.Find(ticket => !ticket.IsDeleted)
                .Project(ticket => new 
                {
                    ticket.Id,
                    ticket.IdPassenger,
                    ticket.Date,
                    ticket.Type,
                    ticket.Status,
                    ticket.AmountPaid,
                })
                .FirstOrDefaultAsync();
        }
      
        public async Task<Ticket?> UpdateAsync(int id, Ticket ticket)
        {
            var existingTicket = await GetByIdAsync(id);

            //Verifica si el ticket existe.
            if (existingTicket == null)
            {
                return null;
            }

            //No se permite actualizar un ticket eliminado.
            if (existingTicket.IsDeleted)
            {
                throw new Exception("El ticket ha sido eliminado y no se puede actualizar.");
            }

            //No se permite reactivar un ticket caducado.
            if (existingTicket.Status.ToLower() == "caducado" && ticket.Status.ToLower() == "activo")
            {
                throw new Exception("No se puede reactivar un ticket caducado.");
            }

            //No se permite cambiar la Id del ticket.
            if (existingTicket.Id != ticket.Id)
            {
                throw new Exception("No se puede cambiar la Id del ticket.");
            }

            //No se permite cambiar la Id del pasajero.
            if (existingTicket.IdPassenger != ticket.IdPassenger)
            {
                throw new Exception("No se puede cambiar la Id del pasajero.");
            }

            //No se permite cambiar el IsDeleted.
            if (existingTicket.IsDeleted != ticket.IsDeleted)
            {
                throw new Exception("No se puede cambiar el estado de eliminación de un ticket.");
            }

            await _tickets.ReplaceOneAsync(t => t.Id == id, ticket);
            return ticket;
        }
      
        public async Task<Ticket?> DeleteAsync(int id)
        {
            var ticket = await GetByIdAsync(id);
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