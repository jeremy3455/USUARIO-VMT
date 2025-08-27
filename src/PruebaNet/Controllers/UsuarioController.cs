using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Services;
using Application.DTOs;

using Domain.Entities;

namespace PruebaNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuariosController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

   
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetAllUsuarios()
        {
            try
            {
                var usuarios = await _usuarioService.GetAllUsuariosAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener usuarios", error = ex.Message });
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            try
            {
                var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
                if (usuario == null)
                    return NotFound(new { message = "Usuario no encontrado" });

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener el usuario", error = ex.Message });
            }
        }

  
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUsuario([FromBody] UsuarioCreateRequest request)
        {
            try
            {
                var usuario = await _usuarioService.CrearUsuarioAsync(request);
                return CreatedAtAction(nameof(GetUsuario), new { id = usuario.IdUsuario }, usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

  
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] UsuarioUpdateRequest request)
        {
            try
            {
                var usuario = await _usuarioService.UpdateUsuarioAsync(id, request);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            try
            {
                await _usuarioService.EliminarUsuarioAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/usuarios/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var dto = new Application.DTOs.LoginRequest
                {
                    UsernameOrEmail = request.UsernameOrEmail,
                    Password = request.Password
                };

                var usuario = await _usuarioService.LoginAsync(dto);

                return Ok(new
                {
                    message = "Login exitoso",
                    usuarioId = usuario.IdUsuario,
                    userName = usuario.UserName,
                    mail = usuario.Mail
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        // POST: api/usuarios/logout/5
        [HttpPost("logout/{usuarioId}")]
        public async Task<IActionResult> Logout(int usuarioId)
        {
            try
            {
                await _usuarioService.LogoutAsync(usuarioId);
                return Ok(new { message = "Sesión cerrada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
