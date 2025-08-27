using Application.Interfaces;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RolService
    {
        private readonly IRolRepository _rolRepository;

        public RolService(IRolRepository rolRepository)
        {
            _rolRepository = rolRepository;
            
        }

        public async Task<IEnumerable<RolOpciones>> ObtenerOpcionesPorRolAsync(int rolId)
        {
            return await _rolRepository.GetOpcionesByRolAsync(rolId);
        }

        public async Task<IEnumerable<Rol>> ObtenerRolesPorUsuarioAsync(int usuarioId)
        {
            return await _rolRepository.GetRolesByUsuarioAsync(usuarioId);
        }

        public async Task<bool> UsuarioTieneRolAsync(int usuarioId, string rolName)
        {
            var roles = await _context.RolUsuarios
                .Include(ru => ru.rol)
                .Where(ru => ru.usuario.IdUsuario == usuarioId)
                .ToListAsync();

            return roles.Any(r => r.rol.RolName == rolName);
        }

    }
}
