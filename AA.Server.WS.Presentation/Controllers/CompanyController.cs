using AA.Server.WS.Application.Contracts;
using AA.Server.WS.Domain.Entities;
using AA.Server.WS.Domain.Models.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AA.Server.WS.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        #region Fields & Properties
        private readonly IConfiguration _configuration;
        private readonly ILogger<CompanyController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Constructor
        public CompanyController(
            IConfiguration configuration,
            ILogger<CompanyController> logger,
            IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Methods
        [Authorize(Policy = Policy.AdminOrUser)]
        [HttpGet]
        [Route("companies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllCompanies()
        {
            _logger.LogInformation($"{nameof(GetAllCompanies)}");

            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            if (!roles.Contains(Role.Admin) && !roles.Contains(Role.User))
            {
                return Unauthorized();
            }

            var companies = await _unitOfWork.CompanyRepository.Get();

            return Ok(companies);
        }

        [Authorize(Policy = Policy.AdminOrUser)]
        [HttpGet]
        [Route("companies/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCompanyById([FromRoute] string id)
        {
            _logger.LogInformation($"{nameof(GetCompanyById)} id: {id}");

            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            if (!roles.Contains(Role.Admin) && !roles.Contains(Role.User))
            {
                return Unauthorized();
            }

            var company = await _unitOfWork.CompanyRepository.GetById(new Guid(id));

            return Ok(company);
        }
        #endregion
    }
}
