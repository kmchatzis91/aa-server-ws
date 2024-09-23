using AA.Server.WS.Application.Contracts;
using AA.Server.WS.Domain.Entities;
using AA.Server.WS.Domain.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AA.Server.WS.Presentation.Controllers
{
    [Authorize]
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
        [HttpGet]
        [Route("users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllUsers()
        {
            _logger.LogInformation($"{nameof(GetAllUsers)}");
            var users = await _unitOfWork.DbUserRepository.GetUsersView();
            return Ok(users);
        }

        [HttpGet]
        [Route("users/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserByUsername([FromRoute] string username)
        {
            _logger.LogInformation($"{nameof(GetUserByUsername)} username: {username}");
            var user = await _unitOfWork.DbUserRepository.GetByUsername(username);
            return Ok(user);
        }
        #endregion
    }
}
