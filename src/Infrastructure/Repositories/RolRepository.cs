using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class RolRepository : IRolRepository
    {
        private readonly ApplicationDbContext _context;

        public RolRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Rol> GetByIdAsync(int id)
        {
            var roles = await _context.Roles
                .FromSqlRaw("EXEC Rol_GetById @IdRol={0}", id)
                .Include(r => r.RolOpciones)
                    .ThenInclude(ro => ro.RolOpc)
                .ToListAsync();

            return roles.FirstOrDefault();
        }

        public async Task<Rol> GetByNameAsync(string rolName)
        {
            var roles = await _context.Roles
                .FromSqlRaw("EXEC Rol_GetByName @RolName={0}", rolName)
                .Include(r => r.RolOpciones)
                    .ThenInclude(ro => ro.RolOpc)
                .ToListAsync();

            return roles.FirstOrDefault();
        }

        public async Task<IEnumerable<Rol>> GetAllAsync()
        {
            var roles = await _context.Roles
                .FromSqlRaw("EXEC Rol_GetAll")
                .Include(r => r.RolOpciones)
                    .ThenInclude(ro => ro.RolOpc)
                .ToListAsync();

            return roles;
        }

        public async Task AddAsync(Rol rol)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC Rol_Add @RolName={0}", rol.RolName
            );
        }

        public async Task UpdateAsync(Rol rol)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC Rol_Update @IdRol={0}, @RolName={1}", rol.IdRol, rol.RolName
            );
        }

        public async Task DeleteAsync(int id)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC Rol_Delete @IdRol={0}", id
            );
        }

        public async Task<IEnumerable<RelOpciones>> GetOpcionesByRolAsync(int rolId)
        {
            var opciones = await _context.RolOpciones
                .FromSqlRaw("EXEC RolOpciones_GetByRol @IdRol={0}", rolId)
                .Include(ro => ro.RelOpc)
                .Select(ro => ro.RelOpc)
                .ToListAsync();

            return opciones;
        }

        public async Task<IEnumerable<Rol>> GetRolesByUsuarioAsync(int usuarioId)
        {
            var roles = await _context.RolUsuarios
                .FromSqlRaw("EXEC RolUsuarios_GetByUsuario @UsuarioId={0}", usuarioId)
                .Include(ru => ru.Rol)
                    .ThenInclude(r => r.RolOpciones)
                        .ThenInclude(ro => ro.RolOpc)
                .Select(ru => ru.Rol)
                .ToListAsync();

            return roles;
        }
    }
}
