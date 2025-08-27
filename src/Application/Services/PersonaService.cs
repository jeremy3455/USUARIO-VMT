using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PersonaService
    {
        private readonly IPersonaRepository _personaRepository;

        public PersonaService(IPersonaRepository personaRepository)
        {
            _personaRepository = personaRepository;
        }

        public async Task<IEnumerable<Persona>> GetAllAsync()
        {
            return await _personaRepository.GetAllAsync();
        }

        public async Task<Persona> GetByIdAsync(int id)
        {
            var persona = await _personaRepository.GetByIdAsync(id);
            if (persona == null)
                throw new Exception("Persona no encontrada");
            return persona;
        }

        public async Task<Persona> CreateAsync(PersonaCreateRequest request)
        {
            var existente = await _personaRepository.GetByIdentificacionAsync(request.Identificacion);
            if (existente != null)
                throw new Exception("Ya existe una persona con esta identificación");

            var persona = new Persona
            {
                Nombres = request.Nombres,
                Apellidos = request.Apellidos,
                Identificacion = request.Identificacion,
                FechaNacimiento = request.FechaNacimiento
            };

            return await _personaRepository.AddAsync(persona);
        }

        public async Task<Persona> UpdateAsync(int id, PersonaUpdateRequest request)
        {
            var persona = await _personaRepository.GetByIdAsync(id);
            if (persona == null)
                throw new Exception("Persona no encontrada");

            persona.Nombres = request.Nombres;
            persona.Apellidos = request.Apellidos;
            persona.Identificacion = request.Identificacion;
            persona.FechaNacimiento = request.FechaNacimiento;

            await _personaRepository.UpdateAsync(persona);
            return persona;
        }

        public async Task DeleteAsync(int id)
        {
            await _personaRepository.DeleteAsync(id);
        }
    }
}
