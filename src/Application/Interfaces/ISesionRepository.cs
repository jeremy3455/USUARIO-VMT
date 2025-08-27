using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISesionRepository
    {
        Task StartSessionAsync(int usuarioId);
        Task EndSessionAsync(int usuarioId);
        Task<IEnumerable<Sesion>> GetUserSessionsAsync(int usuarioId);
        Task EndAllSessionsAsync();
        Task<bool> HasActiveSessionAsync(int usuarioId);
    }
}
