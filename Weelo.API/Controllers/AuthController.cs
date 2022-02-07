using Microsoft.AspNetCore.Mvc;
using Weelo.Domain.Attributes;
using Weelo.Domain.Interfaces.Services;
using Weelo.Domain.Models;

namespace Weelo.API.Controllers
{
    /// <summary>
    /// Auth controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// Auth service
        /// </summary>
        private readonly IAuthService _authService;

        /// <summary>
        /// Controller
        /// </summary>
        /// <param name="authService"></param>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Generate token
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /login
        ///     {
        ///        "Email": "darl.8910@gmail.com",
        ///        "Password": "qwerty"
        ///     }
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>A newly created TodoItem</returns>
        [HttpPost("[action]"), RequestValidation]
        public async Task<ActionResult<LoginResponse>> LoginAsync([FromBody] LoginRequest model)
            => Ok(await _authService.LoginAsync(model));
    }
}
