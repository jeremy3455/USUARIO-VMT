using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> AddAsync(Usuario usuario)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC Usuario_Insert @UserName={0}, @Password={1}, @Mail={2}, @PersonaId={3}, @Status={4}, @IntentosLogin={5}, @SessionActive={6}, @FechaBloqueo={7}",
                usuario.UserName,
                usuario.Password,
                usuario.Mail,
                usuario.PersonaId,
                usuario.Status,
                usuario.IntentosLogin,
                usuario.SessionActive,
                usuario.FechaBloqueo
            );
            return usuario;
        }


        public async Task DeleteAsync(int id)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC Usuario_Delete @Id={0}", id);
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _context.Usuarios
                .FromSqlRaw("EXEC Usuario_GetAll")
                .Include(u => u.Persona)
                .ToListAsync();
        }

        public async Task<Usuario> GetByIdAsync(int id)
        {
            return await _context.Usuarios
                .FromSqlRaw("EXEC Usuario_GetById @Id={0}", id)
                .Include(u => u.Persona)
                .FirstOrDefaultAsync();
        }

        public async Task<Usuario> GetByUsernameAsync(string username)
        {
            return await _context.Usuarios
                .FromSqlRaw("EXEC Usuario_GetByUsername @UserName={0}", username)
                .Include(u => u.Persona)
                .FirstOrDefaultAsync();
        }

        public async Task<Usuario> GetByUsernameOrEmailAsync(string usernameOrEmail)
        {
            return await _context.Usuarios
                .FromSqlRaw("EXEC Usuario_GetByUsernameOrEmail @UsernameOrEmail={0}", usernameOrEmail)
                .Include(u => u.Persona)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CheckIfPersonaHasUsuarioAsync(int personaId)
        {
            var result = await _context.Usuarios
                .FromSqlRaw("EXEC Usuario_CheckPersona @PersonaId={0}", personaId)
                .ToListAsync();
            return result.Count > 0;
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC Usuario_Update @Id={0}, @UserName={1}, @Password={2}, @Mail={3}, @Status={4}, @IntentosLogin={5}, @SessionActive={6}, @FechaBloqueo={7}",
                usuario.IdUsuario,
                usuario.UserName,
                usuario.Password,
                usuario.Mail,
                usuario.Status,
                usuario.IntentosLogin,
                usuario.SessionActive,
                usuario.FechaBloqueo
            );
        }
    }
}
