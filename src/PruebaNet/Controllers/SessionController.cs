using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace PruebaNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SesionesController : ControllerBase
    {
        private readonly ISesionRepository _sesionRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public SesionesController(ISesionRepository sesionRepository, IUsuarioRepository usuarioRepository)
        {
            _sesionRepository = sesionRepository;
            _usuarioRepository = usuarioRepository;
        }

        [HttpPost("iniciar/{usuarioId}")]
        public async Task<IActionResult> IniciarSesion(int usuarioId)
        {
            try
            {
                var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
                if (usuario == null)
                {
                    return NotFound(new { message = "Usuario no encontrado" });
                }

                await _sesionRepository.StartSessionAsync(usuarioId);
                
                return Ok(new { message = "Sesión iniciada correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al iniciar sesión", error = ex.Message });
            }
        }

        [HttpPost("cerrar/{usuarioId}")]
        public async Task<IActionResult> CerrarSesion(int usuarioId)
        {
            try
            {
                await _sesionRepository.EndSessionAsync(usuarioId);
                return Ok(new { message = "Sesión cerrada correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al cerrar sesión", error = ex.Message });
            }
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> ObtenerSesionesUsuario(int usuarioId)
        {
            try
            {
                var sesiones = await _sesionRepository.GetUserSessionsAsync(usuarioId);
                return Ok(sesiones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener sesiones", error = ex.Message });
            }
        }

        [HttpPost("cerrar-todas")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CerrarTodasLasSesiones()
        {
            try
            {
                await _sesionRepository.EndAllSessionsAsync();
                return Ok(new { message = "Todas las sesiones han sido cerradas" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al cerrar todas las sesiones", error = ex.Message });
            }
        }

        [HttpGet("activas/{usuarioId}")]
        public async Task<IActionResult> TieneSesionActiva(int usuarioId)
        {
            try
            {
                var sesiones = await _sesionRepository.GetUserSessionsAsync(usuarioId);
                var sesionActiva = sesiones.FirstOrDefault(s => s.FechaCierre == null);
                
                return Ok(new { tieneSesionActiva = sesionActiva != null });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al verificar sesión activa", error = ex.Message });
            }
        }
    }
}