using Luxprop.Data.Models;
using Luxprop.Data.Repositories;

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

        public PropiedadService(IPropiedadRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Propiedad>> GetAllAsync()
        {
            var list = await _repository.ReadAsync();
            return list.ToList();
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
