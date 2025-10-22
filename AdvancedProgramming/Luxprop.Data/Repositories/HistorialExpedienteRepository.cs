using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Data.Repositories
{
    public interface IHistorialExpedienteRepository
    {
        Task<IEnumerable<HistorialExpediente>> GetByExpedienteIdAsync(int expedienteId);
        Task CrearHistorialAsync(int expedienteId, string estadoNuevo, string descripcion, int? usuarioId = null);
        Task<IEnumerable<HistorialExpediente>> GetAllAsync();
    }

    public class HistorialExpedienteRepository : IHistorialExpedienteRepository
    {
        private readonly LuxpropContext _context;
        private readonly DbSet<HistorialExpediente> _dbSet;

        public HistorialExpedienteRepository(LuxpropContext context)
        {
            _context = context;
            _dbSet = _context.Set<HistorialExpediente>();
        }

        public async Task<IEnumerable<HistorialExpediente>> GetByExpedienteIdAsync(int expedienteId)
        {
            return await _dbSet
                .Include(h => h.Usuario)
                .Where(h => h.ExpedienteId == expedienteId)
                .OrderByDescending(h => h.FechaModificacion)
                .ToListAsync();
        }
        public async Task CrearHistorialAsync(int expedienteId, string estadoNuevo, string descripcion, int? usuarioId = null)
        {
            var historial = new HistorialExpediente
            {
                ExpedienteId = expedienteId,
                UsuarioId = usuarioId,
                EstadoNuevo = estadoNuevo,
                Descripcion = descripcion,
                FechaModificacion = DateTime.Now
            };

            await _dbSet.AddAsync(historial);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<HistorialExpediente>> GetAllAsync()
        {
            return await _dbSet
                .Include(h => h.Expediente)
                .Include(h => h.Usuario)
                .ToListAsync();
        }
    }
}
