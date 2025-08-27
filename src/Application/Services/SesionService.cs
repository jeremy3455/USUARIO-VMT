using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SesionService
    {
        private readonly ISesionRepository _sesionRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public SesionService(ISesionRepository sesionRepository, IUsuarioRepository usuarioRepository)
        {
            _sesionRepository = sesionRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task IniciarSesionAsync(int usuarioId)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            if (await _sesionRepository.HasActiveSessionAsync(usuarioId))
                throw new Exception("El usuario ya tiene una sesión activa");

            await _sesionRepository.StartSessionAsync(usuarioId);

            usuario.SessionActive = "S";
            await _usuarioRepository.UpdateAsync(usuario);
        }

        public async Task CerrarSesionAsync(int usuarioId)
        {
            await _sesionRepository.EndSessionAsync(usuarioId);

            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario != null)
            {
                usuario.SessionActive = "N";
                await _usuarioRepository.UpdateAsync(usuario);
            }
        }

        public async Task<IEnumerable<Sesion>> ObtenerSesionesUsuarioAsync(int usuarioId)
        {
            return await _sesionRepository.GetUserSessionsAsync(usuarioId);
        }

        public async Task CerrarTodasLasSesionesAsync()
        {
            await _sesionRepository.EndAllSessionsAsync();

            var usuarios = await _usuarioRepository.GetAllAsync();
            foreach (var usuario in usuarios)
            {
                usuario.SessionActive = "N";
                await _usuarioRepository.UpdateAsync(usuario);
            }
        }

        public async Task<bool> TieneSesionActivaAsync(int usuarioId)
        {
            return await _sesionRepository.HasActiveSessionAsync(usuarioId);
        }
    }
}
