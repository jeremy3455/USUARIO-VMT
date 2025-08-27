using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRolRepository
    {
        Task<Rol> GetByIdAsync(int id);
        Task<Rol> GetByNameAsync(string rolName);
        Task<IEnumerable<Rol>> GetAllAsync();
        Task AddAsync(Rol rol);
        Task UpdateAsync(Rol rol);
        Task DeleteAsync(int id);
        Task<IEnumerable<RolOpciones>> GetOpcionesByRolAsync(int rolId);
        Task<IEnumerable<Rol>> GetRolesByUsuarioAsync(int usuarioId);
    }
}
