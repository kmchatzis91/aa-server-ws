using AA.Server.WS.Application.Contracts;
using AA.Server.WS.Domain.Entities;
using AA.Server.WS.Domain.Models.Request;
using AA.Server.WS.Domain.Models.Response;
using AA.Server.WS.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AA.Server.WS.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Fields & Properties
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TokenService _tokenService;
        #endregion

        #region Constructor
        public AuthController(
            IConfiguration configuration,
            ILogger<AuthController> logger,
            IUnitOfWork unitOfWork,
            TokenService tokenService)
        {
            _configuration = configuration;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }
        #endregion

        #region Methods
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation($"{nameof(Login)}");

            var users = await _unitOfWork.DbUserRepository.Get();
            var userAttemptingLogin = users.FirstOrDefault(u => u.Username == request.Username && u.Password == request.Password);

            if (userAttemptingLogin is null)
            {
                return NotFound();
            }

            var dbUser = new DbUser()
            {
                Id = userAttemptingLogin.Id,
                FirstName = userAttemptingLogin.FirstName,
                LastName = userAttemptingLogin.LastName,
                Email = userAttemptingLogin.Email,
                Username = userAttemptingLogin.Username,
                Role = userAttemptingLogin.Role,
                IsActive = userAttemptingLogin.IsActive
            };

            var token = _tokenService.GenerateToken(dbUser);

            var response = new LoginResponse()
            {
                AccessToken = token.Value,
                Expiration = token.ExpirationSeconds
            };

            return Ok(response);
        }
        #endregion
    }
}
