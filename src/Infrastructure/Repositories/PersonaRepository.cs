using Domain.Entities;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PersonaRepository : IPersonaRepository
    {
        private readonly ApplicationDbContext _context;

        public PersonaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Persona> GetByIdAsync(int id)
        {
            var personas = await _context.Personas
                .FromSqlRaw("EXEC Persona_GetById @IdPersona={0}", id)
                .ToListAsync();

            return personas.FirstOrDefault();
        }

        public async Task<IEnumerable<Persona>> GetAllAsync()
        {
            return await _context.Personas
                .FromSqlRaw("EXEC Persona_GetAll")
                .ToListAsync();
        }

        public async Task<Persona> AddAsync(Persona persona)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC Persona_Add @Nombres={0}, @Apellidos={1}, @Identificacion={2}, @FechaNacimiento={3}",
                persona.Nombres, persona.Apellidos, persona.Identificacion, persona.FechaNacimiento
            );
            
          
            return persona;
        }

        public async Task UpdateAsync(Persona persona)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC Persona_Update @IdPersona={0}, @Nombres={1}, @Apellidos={2}, @Identificacion={3}, @FechaNacimiento={4}",
                persona.IdPersona, persona.Nombres, persona.Apellidos, persona.Identificacion, persona.FechaNacimiento
            );
        }

        public async Task DeleteAsync(int id)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC Persona_Delete @IdPersona={0}", id
            );
        }

        public async Task<Persona> GetByIdentificacionAsync(string identificacion)
        {
            var personas = await _context.Personas
                .FromSqlRaw("EXEC Persona_GetByIdentificacion @Identificacion={0}", identificacion)
                .ToListAsync();

            return personas.FirstOrDefault();
        }
    }
}
