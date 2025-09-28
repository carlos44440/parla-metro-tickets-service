using parla_metro_tickets_api.src.DTOs;
using parla_metro_tickets_api.src.Helper;
using parla_metro_tickets_api.src.Models;

namespace parla_metro_tickets_api.src.Interfaces
{
    public interface ITicketRepository
    {
        // Crear un ticket
        Task<Tickets> CreateAsync(CreateTicketDto newtTicket);

        // Obtener un ticket por su Id
        Task<GetTicketByIdDto?> GetByIdAsync(Guid ticketId);

        // Listar todos los tickets
        Task<IEnumerable<GetAllTicketsDto>> GetAllAsync(QueryObject query);

        // Actualizar un ticket existente
        Task<Tickets?> UpdateAsync(Guid ticketId, UpdateTicketDto updatedTicket);

        // Eliminar un ticket por Id
        Task<Tickets?> DeleteAsync(Guid ticketId);
    }
}