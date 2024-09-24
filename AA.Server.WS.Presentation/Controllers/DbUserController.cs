using AA.Server.WS.Application.Contracts;
using AA.Server.WS.Domain.Entities;
using AA.Server.WS.Domain.Models.Response;
using AA.Server.WS.Domain.Models.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AA.Server.WS.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DbUserController : ControllerBase
    {
        #region Fields & Properties
        private readonly IConfiguration _configuration;
        private readonly ILogger<DbUserController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Constructor
        public DbUserController(
            IConfiguration configuration,
            ILogger<DbUserController> logger,
            IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Methods
        [Authorize(Policy = Policy.Admin)]
        [HttpGet]
        [Route("users/admin-view")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllUsers()
        {
            _logger.LogInformation($"{nameof(GetAllUsers)}");

            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            if (!roles.Contains(Role.Admin))
            {
                return Unauthorized();
            }

            var users = await _unitOfWork.DbUserRepository.Get();

            return Ok(users);
        }

        [Authorize(Policy = Policy.User)]
        [HttpGet]
        [Route("users/user-view")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllUsersView()
        {
            _logger.LogInformation($"{nameof(GetAllUsersView)}");

            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            if (!roles.Contains(Role.User))
            {
                return Unauthorized();
            }

            var users = await _unitOfWork.DbUserRepository.GetUsersView();

            return Ok(users);
        }

        [Authorize(Policy = Policy.AdminOrUser)]
        [HttpGet]
        [Route("users/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserByUsername([FromRoute] string username)
        {
            _logger.LogInformation($"{nameof(GetUserByUsername)} username: {username}");

            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            if (!roles.Contains(Role.Admin) && !roles.Contains(Role.User))
            {
                return Unauthorized();
            }

            var user = await _unitOfWork.DbUserRepository.GetByUsername(username);

            return Ok(user);
        }
        #endregion
    }
}
