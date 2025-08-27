using Domain.Entities;
namespace Application.Interfaces
{
    public interface IPersonaRepository
    {
        Task<Persona> GetByIdAsync(int id);
        Task<IEnumerable<Persona>> GetAllAsync();
        Task<Persona> AddAsync(Persona persona);
        Task UpdateAsync(Persona persona);
        Task DeleteAsync(int id);
        Task<Persona> GetByIdentificacionAsync(string identificacion);
    }
}
