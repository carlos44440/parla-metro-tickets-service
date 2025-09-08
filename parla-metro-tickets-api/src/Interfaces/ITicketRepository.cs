using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace parla_metro_tickets_api.src.Interfaces
{
    public interface ITicketRepository
    {
        // Crear un ticket
        Task<Ticket> CreateAsync(Ticket ticket);

        // Obtener un ticket por su Id
        Task<Ticket?> GetByIdAsync(int id);

        // Listar todos los tickets
        Task<IEnumerable<Ticket>> GetAllAsync();

        // Actualizar un ticket existente
        Task<Ticket?> UpdateAsync(int id, Ticket ticket);

        // Eliminar un ticket por Id
        Task<Ticket?> DeleteAsync(int id);
    }
}