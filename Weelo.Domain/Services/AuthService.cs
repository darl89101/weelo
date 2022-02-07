using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Weelo.Domain.Exceptions;
using Weelo.Domain.Interfaces.Entities;
using Weelo.Domain.Interfaces.Services;
using Weelo.Domain.Models;

namespace Weelo.Domain.Services
{
    /// <summary>
    /// Auth service
    /// </summary>
    public class AuthService : IAuthService, Inject
    {
        /// <summary>
        /// Jwt Keys
        /// </summary>
        private readonly JwtOptions _jwtOptions;
        /// <summary>
        /// Jwt factory
        /// </summary>
        private readonly IJwtFactory _jwtFactory;
        /// <summary>
        /// user manager
        /// </summary>
        private readonly UserManager<IdentityUser> _userManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="jwtOptions"></param>
        /// <param name="userManager"></param>
        /// <param name="jwtFactory"></param>
        public AuthService(
            IOptions<JwtOptions> jwtOptions,
            UserManager<IdentityUser> userManager,
            IJwtFactory jwtFactory
        )
        {
            _jwtFactory = jwtFactory;
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
        }

        /// <summary>
        /// Login method
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="BadRequestException"></exception>
        public async Task<LoginResponse> LoginAsync(LoginRequest login)
        {
            var appUser = await _userManager.FindByEmailAsync(login.Email);

            if (appUser == null) throw new NotFoundException($"User with email ({login.Email}) does'nt exists");

            var checkPassword = await _userManager.CheckPasswordAsync(appUser, login.Password);

            if (!checkPassword) throw new BadRequestException("Password incorrect.");

            var roles = await _userManager.GetRolesAsync(appUser);
            var tokenData = _jwtFactory.CreateJwtToken(
                appUser.Id.ToString(),
                appUser.UserName,
                roles.ToArray(),
                appUser.Email,
                _jwtOptions.Key, _jwtOptions.Issuer, _jwtOptions.Audience, _jwtOptions.AccessTokenExpirationMinutes
            );

            return new LoginResponse
            {
                AccessToken = tokenData.token,
                ExpiresIn = _jwtFactory.GetExpireDateFromToken(tokenData.token)
            };
        }
    }
}
