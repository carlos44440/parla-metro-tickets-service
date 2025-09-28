using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using parla_metro_tickets_api.src.Interfaces;
using parla_metro_tickets_api.src.Models;
using parla_metro_tickets_api.src.DTOs;
using MongoDB.Driver;
using parla_metro_tickets_api.src.Data;
using parla_metro_tickets_api.src.Helper;

namespace parla_metro_tickets_api.src.Repositories
{
    // Repositorio encargado de gestionar la persistencia de Tickets en MongoDB
    public class TicketRepository : ITicketRepository
    {
        // Colección de tickets en MongoDB
        private readonly IMongoCollection<Tickets> _tickets;

        // Constructor que recibe el contexto de MongoDB
        public TicketRepository(MongoDbContext context)
        {
            _tickets = context.GetCollection<Tickets>("tickets");
        }

        // Crea un nuevo ticket
        public async Task<Tickets> CreateAsync(CreateTicketDto newTicket)
        {
            // Obtiene tickets existentes del mismo pasajero que no estén eliminados
            var tickets = await _tickets.Find(t => t.IdPassenger == newTicket.IdPassenger && !t.IsDeleted).ToListAsync();

            // Verifica si ya existe un ticket idéntico
            if (tickets != null && tickets.Count > 0)
            {
                for (int i = 0; i < tickets.Count; i++)
                {
                    if (tickets[i].Status.ToLower() == newTicket.Status.ToLower() && 
                        tickets[i].Type.ToLower() == newTicket.Type.ToLower() &&
                        tickets[i].Date.Date == newTicket.Date.Date && 
                        tickets[i].AmountPaid == newTicket.AmountPaid)
                    {
                        throw new Exception("Ya existe un ticket con los mismos datos.");
                    }
                }
            }

            // Crea el objeto ticket
            var ticket = new Tickets
            {
                IdPassenger = newTicket.IdPassenger,
                Date = newTicket.Date,
                Type = newTicket.Type,
                Status = newTicket.Status,
                AmountPaid = newTicket.AmountPaid,
                IsDeleted = false
            };

            // Inserta el ticket en MongoDB
            await _tickets.InsertOneAsync(ticket);
            return ticket;
        }
 
        // Obtiene un ticket por su ID
        public async Task<GetTicketByIdDto?> GetByIdAsync(Guid ticketId)
        {
            var ticket = await _tickets.Find(t => t.TicketID == ticketId && !t.IsDeleted).FirstOrDefaultAsync();
            if (ticket == null) return null;

            // Mapea a DTO
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

        // Obtiene todos los tickets con filtros y ordenamientos
        public async Task<IEnumerable<GetAllTicketsDto>> GetAllAsync(QueryObject query)
        {
            var tickets = _tickets.AsQueryable().Where(t => !t.IsDeleted);

            // Filtrado por texto
            if (!string.IsNullOrEmpty(query.textFilter))
            {
                tickets = tickets.Where(t => t.TicketID.ToString().Contains(query.textFilter) ||
                                             t.IdPassenger.ToString().Contains(query.textFilter) ||
                                             t.Date.ToString().Contains(query.textFilter) ||
                                             t.Type.ToLower().Contains(query.textFilter.ToLower()) ||
                                             t.Status.ToLower().Contains(query.textFilter.ToLower()) ||
                                             t.AmountPaid.ToString().Contains(query.textFilter));
            }

            // Filtrado por tipo
            if (!string.IsNullOrEmpty(query.type))
            {
                var validTypes = new[] { "Ida", "Vuelta" };
                if (!validTypes.Contains(query.type)) throw new Exception("Tipo de ticket incorrecto");
                tickets = tickets.Where(t => t.Type.ToLower() == query.type.ToLower());
            }

            // Filtrado por estado
            if (!string.IsNullOrEmpty(query.status))
            {
                var validStatuses = new[] { "Activo", "Usado", "Caducado" };
                if (!validStatuses.Contains(query.status)) throw new Exception("Estado incorrecto");
                tickets = tickets.Where(t => t.Status.ToLower() == query.status.ToLower());
            }

            // Ordenamiento por monto pagado
            if (!string.IsNullOrWhiteSpace(query.sortByAmountPaid) &&
                query.sortByAmountPaid.Equals("AmountPaid", StringComparison.OrdinalIgnoreCase))
            {
                tickets = query.isDescendingAmountPaid ? tickets.OrderByDescending(x => x.AmountPaid) : tickets.OrderBy(x => x.AmountPaid);
            }

            // Ordenamiento por fecha
            if (!string.IsNullOrWhiteSpace(query.sortByDate) &&
                query.sortByDate.Equals("Date", StringComparison.OrdinalIgnoreCase))
            {
                tickets = query.isDescendingDate ? tickets.OrderByDescending(x => x.Date) : tickets.OrderBy(x => x.Date);
            }

            if(!tickets.Any()) throw new Exception("No se encontraron tickets");

            // Mapea a DTOs
            var ticketsDto = tickets.Select(ticket => new GetAllTicketsDto
            {
                TicketID = ticket.TicketID,
                IdPassenger = ticket.IdPassenger,
                Date = ticket.Date,
                Type = ticket.Type,
                Status = ticket.Status,
                AmountPaid = ticket.AmountPaid
            });
            
            return await Task.FromResult(ticketsDto);
        }

        // Actualiza un ticket existente
        public async Task<Tickets?> UpdateAsync(Guid ticketId, UpdateTicketDto updatedTicket)
        {
            var existingTicket = await _tickets.Find(t => t.TicketID == ticketId && !t.IsDeleted).FirstOrDefaultAsync();
            if (existingTicket == null) return null;

            // Reglas de negocio: no reactivar tickets caducados o usados
            if (existingTicket.Status.ToLower() == "caducado" && updatedTicket.Status.ToLower() == "activo")
                throw new Exception("No se puede reactivar un ticket caducado.");
            if (existingTicket.Status.ToLower() == "caducado" && updatedTicket.Status.ToLower() == "usado")
                throw new Exception("No se permite cambiar el estado de un ticket caducado.");
            if (existingTicket.Status.ToLower() == "usado" && updatedTicket.Status.ToLower() == "activo")
                throw new Exception("No se puede reactivar un ticket usado.");

            // Verifica duplicados del mismo pasajero
            var tickets = await _tickets.Find(t => t.IdPassenger == existingTicket.IdPassenger && !t.IsDeleted).ToListAsync();
            if (tickets != null && tickets.Count > 0)
            {
                for (int i = 0; i < tickets.Count; i++)
                {
                    if (tickets[i].Status.ToLower() == updatedTicket.Status.ToLower() &&
                        tickets[i].Type.ToLower() == updatedTicket.Type.ToLower() &&
                        tickets[i].Date.Date == updatedTicket.Date.Date &&
                        tickets[i].AmountPaid == updatedTicket.AmountPaid)
                    {
                        throw new Exception("Ya existe un ticket con los mismos datos.");
                    }
                }
            }

            // Actualiza los campos
            existingTicket.Date = updatedTicket.Date;
            existingTicket.Type = updatedTicket.Type;
            existingTicket.Status = updatedTicket.Status;
            existingTicket.AmountPaid = updatedTicket.AmountPaid;

            // Reemplaza el documento en MongoDB
            await _tickets.ReplaceOneAsync(t => t.TicketID == ticketId, existingTicket);
            return existingTicket;
        }
      
        // Elimina un ticket (eliminación lógica)
        public async Task<Tickets?> DeleteAsync(Guid ticketId)
        {
            var ticket = await _tickets.Find(t => t.TicketID == ticketId && !t.IsDeleted).FirstOrDefaultAsync();
            if (ticket == null) return null;

            // Marca el ticket como eliminado
            ticket.IsDeleted = true;
            await _tickets.ReplaceOneAsync(t => t.TicketID == ticketId, ticket);
            return ticket;
        }
    }
}
