using Luxprop.Data.Models;
using Luxprop.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Business.Services
{
    public interface IPropiedadService
    {
        Task<List<Propiedad>> GetAllAsync();
        Task<bool> DeleteAsync(int id);
    }

    public class PropiedadService : IPropiedadService
    {
        private readonly IPropiedadRepository _repository;
        private readonly LuxpropContext _db;

        public PropiedadService(IPropiedadRepository repository, LuxpropContext db)
        {
            _repository = repository;
            _db = db;
        }

        // ✅ CARGA PROPIEDADES + SUS EXPEDIENTES (para mostrar el botón correctamente)
        public async Task<List<Propiedad>> GetAllAsync()
        {
            return await _db.Propiedads
                .Include(p => p.Expedientes)
                .Include(p => p.Agente)
                .ToListAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var propiedad = await _repository.FindAsync(id);
            if (propiedad == null)
                return false;

            return await _repository.DeleteAsync(propiedad);
        }
    }
}
