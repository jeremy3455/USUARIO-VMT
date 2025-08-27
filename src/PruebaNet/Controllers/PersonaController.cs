using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Application.DTOs;
using Domain.Entities;
using System.Threading.Tasks;

namespace PruebaNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonasController : ControllerBase
    {
        private readonly PersonaService _personaService;

        public PersonasController(PersonaService personaService)
        {
            _personaService = personaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var personas = await _personaService.GetAllAsync();
            return Ok(personas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var persona = await _personaService.GetByIdAsync(id);
            return Ok(persona);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PersonaCreateRequest request)
        {
            var persona = await _personaService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = persona.IdPersona }, persona);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PersonaUpdateRequest request)
        {
            var persona = await _personaService.UpdateAsync(id, request);
            return Ok(persona);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _personaService.DeleteAsync(id);
            return NoContent();
        }
    }
}
