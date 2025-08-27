using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MenuController : ControllerBase
    {
        private readonly IRolRepository _rolRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public MenuController(IRolRepository rolRepository, IUsuarioRepository usuarioRepository)
        {
            _rolRepository = rolRepository;
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet("{usuarioId}")]
        public async Task<IActionResult> GetMenuByUsuario(int usuarioId)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null)
                return NotFound(new { message = "Usuario no encontrado" });

            var roles = await _rolRepository.GetRolesByUsuarioAsync(usuarioId);
            var opciones = roles
                .SelectMany(r => r.RolOpciones.Select(ro => ro.RolOpc))
                .GroupBy(o => o.IdOpcion)
                .Select(g => g.First())
                .OrderBy(o => o.NombreOpcion)
                .ToList();

            var menu = opciones.Select(o => new
            {
                id = o.IdOpcion,
                nombre = o.NombreOpcion,
                url = $"/{o.NombreOpcion.ToLower()}" 
            });

            return Ok(menu);
        }
    }
}
