using Weelo.Domain.Models;

namespace Weelo.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
