using AA.Server.WS.Application.Contracts;
using AA.Server.WS.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet]
        [Route("companies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<List<Company>> GetAllCompanies()
        {
            _logger.LogInformation($"{nameof(GetAllCompanies)}");
            var companies = await _unitOfWork.CompanyRepository.Get();
            return companies;
        }

        [HttpGet]
        [Route("companies/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Company> GetCompanyById([FromRoute] string id)
        {
            _logger.LogInformation($"{nameof(GetCompanyById)} id: {id}");
            var company = await _unitOfWork.CompanyRepository.GetById(new Guid(id));
            return company;
        }
        #endregion
    }
}
