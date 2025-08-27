using Domain.Entities;
using Application.Interfaces;
using Application.DTOs;
using Application.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPersonaRepository _personaRepository;
        private readonly ISesionRepository _sesionRepository;

        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            IPersonaRepository personaRepository,
            ISesionRepository sesionRepository)
        {
            _usuarioRepository = usuarioRepository;
            _personaRepository = personaRepository;
            _sesionRepository = sesionRepository;
        }

        public async Task<Usuario> CrearUsuarioAsync(UsuarioCreateRequest request)
        {
            var persona = await _personaRepository.GetByIdAsync(request.PersonaId)
                          ?? throw new Exception("La persona especificada no existe");

            if (await _usuarioRepository.CheckIfPersonaHasUsuarioAsync(request.PersonaId))
                throw new Exception("Esta persona ya tiene una cuenta de usuario");

            var valUser = Validador.ValidarUsername(request.UserName);
            if (!valUser.esValido) throw new Exception(valUser.mensaje);

            var valPass = Validador.ValidarPassword(request.Password);
            if (!valPass.esValida) throw new Exception(valPass.mensaje);

            var usuariosExistentes = await _usuarioRepository.GetAllAsync();
            if (usuariosExistentes.Any(u => u.UserName == request.UserName))
                throw new Exception("El nombre de usuario ya está en uso");

            var correosExistentes = usuariosExistentes.Select(u => u.Mail).ToList();
            var correoGenerado = EmailGenerator.GenerateEmail(persona.Nombres, persona.Apellidos, correosExistentes);

            var usuario = new Usuario
            {
                UserName = request.UserName,
                Password = PasswordHasher.HashPassword(request.Password),
                Mail = correoGenerado,
                PersonaId = request.PersonaId,
                Status = "ACTIVE",
                IntentosLogin = 0,
                SessionActive = "N",
                FechaBloqueo = null
            };

            await _usuarioRepository.AddAsync(usuario);
            return usuario;
        }

        public async Task<Usuario> UpdateUsuarioAsync(int id, UsuarioUpdateRequest request)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id)
                          ?? throw new Exception("Usuario no encontrado");

            if (usuario.UserName != request.UserName)
            {
                var valUser = Validador.ValidarUsername(request.UserName);
                if (!valUser.esValido) throw new Exception(valUser.mensaje);

                var existUser = await _usuarioRepository.GetByUsernameAsync(request.UserName);
                if (existUser != null && existUser.IdUsuario != id)
                    throw new Exception("El nombre de usuario ya está en uso");

                usuario.UserName = request.UserName;
            }

            if (!string.IsNullOrEmpty(request.Password))
            {
                var valPass = Validador.ValidarPassword(request.Password);
                if (!valPass.esValida) throw new Exception(valPass.mensaje);

                usuario.Password = PasswordHasher.HashPassword(request.Password);
            }

            usuario.Status = request.Status;
            await _usuarioRepository.UpdateAsync(usuario);
            return usuario;
        }

        public async Task<Usuario> LoginAsync(LoginRequest request)
        {
            var usuario = await _usuarioRepository.GetByUsernameOrEmailAsync(request.UsernameOrEmail)
                          ?? throw new Exception("Credenciales inválidas");

            if (usuario.Status == "BLOQUEADO")
                throw new Exception("Usuario bloqueado");

            if (!PasswordHasher.VerifyPassword(request.Password, usuario.Password))
            {
                usuario.IntentosLogin++;
                if (usuario.IntentosLogin >= 3)
                {
                    usuario.Status = "BLOQUEADO";
                    usuario.FechaBloqueo = DateTime.UtcNow;
                    await _usuarioRepository.UpdateAsync(usuario);
                    throw new Exception("Usuario bloqueado por demasiados intentos fallidos");
                }
                await _usuarioRepository.UpdateAsync(usuario);
                throw new Exception($"Contraseña incorrecta. Intentos restantes: {3 - usuario.IntentosLogin}");
            }

            if (usuario.SessionActive == "S")
                throw new Exception("Ya existe una sesión activa para este usuario");

            usuario.SessionActive = "S";
            usuario.IntentosLogin = 0;
            await _usuarioRepository.UpdateAsync(usuario);
            await _sesionRepository.StartSessionAsync(usuario.IdUsuario);

            return usuario;
        }

        public async Task LogoutAsync(int usuarioId)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario != null)
            {
                usuario.SessionActive = "N";
                await _usuarioRepository.UpdateAsync(usuario);
            }
            await _sesionRepository.EndSessionAsync(usuarioId);
        }

        public async Task<Usuario?> GetUsuarioByIdAsync(int id)
            => await _usuarioRepository.GetByIdAsync(id);

        public async Task<IEnumerable<Usuario>> GetAllUsuariosAsync()
            => await _usuarioRepository.GetAllAsync();

        public async Task EliminarUsuarioAsync(int id)
            => await _usuarioRepository.DeleteAsync(id);
    }
}
