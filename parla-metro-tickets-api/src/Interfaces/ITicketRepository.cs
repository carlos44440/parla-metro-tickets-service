using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using parla_metro_tickets_api.src.DTOs;
using parla_metro_tickets_api.src.Models;

namespace parla_metro_tickets_api.src.Interfaces
{
    public interface ITicketRepository
    {
        // Crear un ticket
        Task<Tickets> CreateAsync(CreateTicketDto newtTicket);

        // Obtener un ticket por su Id
        Task<GetTicketByIdDto?> GetByIdAsync(string id);

        // Listar todos los tickets
        Task<IEnumerable<GetAllTicketsDto>> GetAllAsync();

        // Actualizar un ticket existente
        Task<Tickets?> UpdateAsync(string id, UpdateTicketDto updatedTicket);

        // Eliminar un ticket por Id
        Task<Tickets?> DeleteAsync(string id);
    }
}