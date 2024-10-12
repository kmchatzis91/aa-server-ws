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
    public class AggregationController : ControllerBase
    {
        #region Fields & Properties
        private readonly IConfiguration _configuration;
        private readonly ILogger<AggregationController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Constructor
        public AggregationController(
            IConfiguration configuration,
            ILogger<AggregationController> logger,
            IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Methods
        //[Authorize(Policy = Policy.AdminOrUser)]
        [HttpGet]
        [Route("test/{category}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Test([FromRoute] string category)
        {
            _logger.LogInformation($"{nameof(GetAllAggregationExample)}");

            //var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            //if (!roles.Contains(Role.Admin) && !roles.Contains(Role.User))
            //{
            //    return Unauthorized();
            //}

            //var testResponse = await _unitOfWork.DogApiRepository.GetDogFact(); // => OK!
            //var testResponse = await _unitOfWork.DogApiRepository.GetManyDogFacts(5); // => OK!

            //var testResponse = await _unitOfWork.ZeldaFanApiRepository.GetZeldaGameInfo(); // => OK!
            // var testResponse = await _unitOfWork.ZeldaFanApiRepository.GetManyZeldaGameInfo(5); // => OK!

            //var testResponse = await _unitOfWork.JokeApiRepository.GetJoke(); // => OK!
            var testResponse = await _unitOfWork.JokeApiRepository.GetJokeByCategoryName(category);



            return Ok(testResponse);
        }

        [Authorize(Policy = Policy.AdminOrUser)]
        [HttpGet]
        [Route("company-cat-fact")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CompanyCatFact))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAggregationExample()
        {
            _logger.LogInformation($"{nameof(GetAllAggregationExample)}");

            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            if (!roles.Contains(Role.Admin) && !roles.Contains(Role.User))
            {
                return Unauthorized();
            }

            var companies = await _unitOfWork.CompanyRepository.Get();
            var catFact = await _unitOfWork.CatFactRepository.GetCatFact();

            if (companies == null || catFact == null)
            {
                return NotFound();
            }

            var companyCatFact = new CompanyCatFact()
            {
                Companies = companies,
                CatFact = catFact
            };

            return Ok(companyCatFact);
        }
        #endregion
    }
}
