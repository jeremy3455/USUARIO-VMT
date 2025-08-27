using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SesionRepository : ISesionRepository
    {
        private readonly ApplicationDbContext _context;

        public SesionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task StartSessionAsync(int usuarioId)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC Sesion_Insert @UsuarioId={0}, @FechaInicio={1}",
                usuarioId,
                DateTime.UtcNow
            );
        }

        public async Task EndSessionAsync(int usuarioId)
        {
          
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC Sesion_EndByUsuario @UsuarioId={0}, @FechaCierre={1}",
                usuarioId,
                DateTime.UtcNow
            );
        }

        public async Task<IEnumerable<Sesion>> GetUserSessionsAsync(int usuarioId)
        {
          
            var sesiones = await _context.Sesiones
                .FromSqlRaw("EXEC Sesion_GetByUsuario @UsuarioId={0}", usuarioId)
                .Include(s => s.Usuario)
                .ToListAsync();

            return sesiones;
        }

        public async Task EndAllSessionsAsync()
        {
     
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC Sesion_EndAll @FechaCierre={0}",
                DateTime.UtcNow
            );
        }

        public async Task<bool> HasActiveSessionAsync(int usuarioId)
        {
         
            var active = await _context.Sesiones
                .FromSqlRaw("EXEC Sesion_HasActive @UsuarioId={0}", usuarioId)
                .AnyAsync(s => s.UsuarioId == usuarioId && s.FechaCierre == null);

            return active;
        }
    }
}
