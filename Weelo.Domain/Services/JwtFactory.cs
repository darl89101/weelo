using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Weelo.Domain.Commons;
using Weelo.Domain.Interfaces.Entities;
using Weelo.Domain.Interfaces.Services;

namespace Weelo.Domain.Services
{
    public class JwtFactory : IJwtFactory, Inject
    {
        private readonly RandomNumberGenerator _rand = RandomNumberGenerator.Create();

        /// <summary>
        /// Generate random guid
        /// </summary>
        /// <returns></returns>
        public Guid CreateCryptographicallySecureGuid()
        {
            var bytes = new byte[16];
            _rand.GetBytes(bytes);
            return new Guid(bytes);
        }

        /// <summary>
        /// Create jwt token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="roles"></param>
        /// <param name="email"></param>
        /// <param name="tokenKey"></param>
        /// <param name="issuer"></param>
        /// <param name="audience"></param>
        /// <param name="expirationMinutes"></param>
        /// <returns></returns>
        public (string token, List<Claim> claims) CreateJwtToken(
            string userId,
            string userName,
            string[] roles,
            string email,

            string tokenKey,
            string issuer,
            string audience,
            int expirationMinutes
        )
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, CreateCryptographicallySecureGuid().ToString(), ClaimValueTypes.String, issuer),
                new Claim(JwtRegisteredClaimNames.Iss, issuer, ClaimValueTypes.String,issuer),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, issuer),
                new Claim(ClaimTypes.NameIdentifier, userId, ClaimValueTypes.String, issuer),
                new Claim(ClaimTypes.Name, userName, ClaimValueTypes.String, issuer),
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(ClaimTypes.UserData, userId, ClaimValueTypes.String, issuer)
            };

            // add roles
            if (roles != null)
            {
                claims.AddRange(roles.Select(m => new Claim(ClaimTypes.Role, m, ClaimValueTypes.String, issuer)));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var now = DateTime.UtcNow;

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(expirationMinutes),
                signingCredentials: credentials
            );

            return (token: new JwtSecurityTokenHandler().WriteToken(token), claims: claims);
        }

        /// <summary>
        /// Get expiration Date from token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public DateTime GetExpireDateFromToken(string token)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            if (!jwtSecurityTokenHandler.CanReadToken(token))
            {
                return DateUtils.GetCurrentDate();
            }

            var jwtSecurityToken = jwtSecurityTokenHandler.ReadToken(token);
            return jwtSecurityToken.ValidTo;
        }
    }
}
