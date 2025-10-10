using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Data.Repositories
{
    public interface IAlertaVencimientoRepository : IRepositoryBase<AlertaVencimiento>
    {
        Task<IEnumerable<AlertaVencimiento>> GetByDocumentoIdAsync(int documentoId);
        Task<IEnumerable<AlertaVencimiento>> GetByTipoAsync(string tipo);
        Task<IEnumerable<AlertaVencimiento>> GetByEstadoAsync(string estado);
        Task<IEnumerable<AlertaVencimiento>> GetUpcomingAlertsAsync(int daysAhead = 30);
        Task<IEnumerable<AlertaVencimiento>> GetOverdueAlertsAsync();
    }
    
    public class AlertaVencimientoRepository : RepositoryBase<AlertaVencimiento>, IAlertaVencimientoRepository
    {
        public AlertaVencimientoRepository()
        {
            DbSet = DbContext.Set<AlertaVencimiento>();
        }

        public async Task<IEnumerable<AlertaVencimiento>> GetByDocumentoIdAsync(int documentoId)
        {
            return await DbSet
                .Include(a => a.Documento)
                .Where(a => a.DocumentoId == documentoId)
                .OrderBy(a => a.FechaProgramada)
                .ToListAsync();
        }

        public async Task<IEnumerable<AlertaVencimiento>> GetByTipoAsync(string tipo)
        {
            return await DbSet
                .Include(a => a.Documento)
                .Where(a => a.Tipo == tipo)
                .OrderBy(a => a.FechaProgramada)
                .ToListAsync();
        }

        public async Task<IEnumerable<AlertaVencimiento>> GetByEstadoAsync(string estado)
        {
            return await DbSet
                .Include(a => a.Documento)
                .Where(a => a.Estado == estado)
                .OrderBy(a => a.FechaProgramada)
                .ToListAsync();
        }

        public async Task<IEnumerable<AlertaVencimiento>> GetUpcomingAlertsAsync(int daysAhead = 30)
        {
            var futureDate = DateOnly.FromDateTime(DateTime.Now.AddDays(daysAhead));
            return await DbSet
                .Include(a => a.Documento)
                .Where(a => a.FechaProgramada <= futureDate && a.Estado != "Completed")
                .OrderBy(a => a.FechaProgramada)
                .ToListAsync();
        }

        public async Task<IEnumerable<AlertaVencimiento>> GetOverdueAlertsAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            return await DbSet
                .Include(a => a.Documento)
                .Where(a => a.FechaProgramada < today && a.Estado != "Completed")
                .OrderBy(a => a.FechaProgramada)
                .ToListAsync();
        }
    }
}
