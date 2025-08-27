using Microsoft.AspNetCore.Mvc;
using Application.Services;

namespace PruebaNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _loginService;
        
        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _loginService.LoginAsync(request.UsernameOrEmail, request.Password);
            
            if (!result.Success)
                return BadRequest(new { message = result.Message });
            
            return Ok(new { 
                message = result.Message, 
                usuario = new {
                    result.Usuario.IdUsuario,
                    result.Usuario.UserName,
                    result.Usuario.Mail,
                    result.Usuario.Persona.Nombres,
                    result.Usuario.Persona.Apellidos
                }
            });
        }
        
        [HttpPost("logout/{usuarioId}")]
        public async Task<IActionResult> Logout(int usuarioId)
        {
            var result = await _loginService.LogoutAsync(usuarioId);
            
            if (!result.Success)
                return BadRequest(new { message = result.Message });
            
            return Ok(new { message = result.Message });
        }
    }
    
    public class LoginRequest
    {
        public string UsernameOrEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}