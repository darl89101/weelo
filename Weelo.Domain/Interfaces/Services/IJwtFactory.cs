using System.Security.Claims;

namespace Weelo.Domain.Interfaces.Services
{
    public interface IJwtFactory
    {
        DateTime GetExpireDateFromToken(string token);
        (string token, List<Claim> claims) CreateJwtToken(
            string userId,
            string userName,
            string[] roles,
            string email,
            string tokenKey,
            string issuer,
            string audience,
            int expirationMinutes
        );
    }
}
