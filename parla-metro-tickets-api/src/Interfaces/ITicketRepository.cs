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
        Task<Ticket> CreateAsync(CreateTicketDto newtTicket);

        // Obtener un ticket por su Id
        Task<Ticket?> GetByIdAsync(int id);

        // Listar todos los tickets
        Task<IEnumerable<Ticket>> GetAllAsync();

        // Actualizar un ticket existente
        Task<Ticket?> UpdateAsync(int id, UpdateTicketDto updatedTicket);

        // Eliminar un ticket por Id
        Task<Ticket?> DeleteAsync(int id);
    }
}