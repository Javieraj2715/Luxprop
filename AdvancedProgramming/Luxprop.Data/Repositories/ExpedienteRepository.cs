using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Data.Repositories
{
    public interface IExpedienteRepository : IRepositoryBase<Expediente>
    {
        Task<IEnumerable<Expediente>> GetByPropiedadIdAsync(int propiedadId);
        Task<IEnumerable<Expediente>> GetByClienteIdAsync(int clienteId);
        Task<IEnumerable<Expediente>> GetByEstadoAsync(string estado);
        Task<IEnumerable<Expediente>> GetByTipoOcupacionAsync(string tipoOcupacion);
        Task<IEnumerable<Expediente>> GetOpenExpedientesAsync();
        Task<IEnumerable<Expediente>> GetExpedientesWithDocumentsAsync();
    }
    
    public class ExpedienteRepository : RepositoryBase<Expediente>, IExpedienteRepository
    {
        public ExpedienteRepository()
        {
            DbSet = DbContext.Set<Expediente>();
        }

        public async Task<IEnumerable<Expediente>> GetByPropiedadIdAsync(int propiedadId)
        {
            return await DbSet
                .Include(e => e.Propiedad)
                .Include(e => e.Cliente)
                .Include(e => e.Documentos)
                .Include(e => e.Cita)
                .Include(e => e.TareaTramites)
                .Where(e => e.PropiedadId == propiedadId)
                .OrderBy(e => e.FechaApertura)
                .ToListAsync();
        }

        public async Task<IEnumerable<Expediente>> GetByClienteIdAsync(int clienteId)
        {
            return await DbSet
                .Include(e => e.Propiedad)
                .Include(e => e.Cliente)
                .Include(e => e.Documentos)
                .Include(e => e.Cita)
                .Include(e => e.TareaTramites)
                .Where(e => e.ClienteId == clienteId)
                .OrderBy(e => e.FechaApertura)
                .ToListAsync();
        }

        public async Task<IEnumerable<Expediente>> GetByEstadoAsync(string estado)
        {
            return await DbSet
                .Include(e => e.Propiedad)
                .Include(e => e.Cliente)
                .Include(e => e.Documentos)
                .Include(e => e.Cita)
                .Include(e => e.TareaTramites)
                .Where(e => e.Estado == estado)
                .OrderBy(e => e.FechaApertura)
                .ToListAsync();
        }

        public async Task<IEnumerable<Expediente>> GetByTipoOcupacionAsync(string tipoOcupacion)
        {
            return await DbSet
                .Include(e => e.Propiedad)
                .Include(e => e.Cliente)
                .Include(e => e.Documentos)
                .Include(e => e.Cita)
                .Include(e => e.TareaTramites)
                .Where(e => e.TipoOcupacion == tipoOcupacion)
                .OrderBy(e => e.FechaApertura)
                .ToListAsync();
        }

        public async Task<IEnumerable<Expediente>> GetOpenExpedientesAsync()
        {
            return await DbSet
                .Include(e => e.Propiedad)
                .Include(e => e.Cliente)
                .Include(e => e.Documentos)
                .Include(e => e.Cita)
                .Include(e => e.TareaTramites)
                .Where(e => e.Estado != "Closed" && e.FechaCierre == null)
                .OrderBy(e => e.FechaApertura)
                .ToListAsync();
        }

        public async Task<IEnumerable<Expediente>> GetExpedientesWithDocumentsAsync()
        {
            return await DbSet
                .Include(e => e.Propiedad)
                .Include(e => e.Cliente)
                .Include(e => e.Documentos)
                .Include(e => e.Cita)
                .Include(e => e.TareaTramites)
                .Where(e => e.Documentos.Any())
                .OrderBy(e => e.FechaApertura)
                .ToListAsync();
        }
    }
}
