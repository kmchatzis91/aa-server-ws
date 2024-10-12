using AA.Server.WS.Application.Contracts;
using AA.Server.WS.Domain.Entities;
using AA.Server.WS.Domain.Models.Request;
using AA.Server.WS.Domain.Models.Response;
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
        [Authorize(Policy = Policy.AdminOrUser)]
        [HttpGet]
        [Route("data")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AggregationResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAggregationData()
        {
            _logger.LogInformation($"{nameof(GetAggregationData)}");

            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            if (!roles.Contains(Role.Admin) && !roles.Contains(Role.User))
            {
                return Unauthorized();
            }

            var dogApiResponse = await _unitOfWork.DogApiRepository.GetDogFact();
            var zeldaFanApi = await _unitOfWork.ZeldaFanApiRepository.GetZeldaGameInfo();
            var jokeApiResponse = await _unitOfWork.JokeApiRepository.GetJoke();
            var spaceFlightNewsApiResponse = await _unitOfWork.SpaceFlightNewsApiRepository.GetSpaceNew();

            var result = new AggregationResponse() 
            {
                DogApi = dogApiResponse,
                ZeldaFanApi = zeldaFanApi,
                JokeApi = jokeApiResponse,
                SpaceFlightNewsApi = spaceFlightNewsApiResponse
            };

            return Ok(result);
        }

        [Authorize(Policy = Policy.AdminOrUser)]
        [HttpPost]
        [Route("filtered/data")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AggregationResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAggregationFilteredData([FromBody] AggregationRequest request)
        {
            _logger.LogInformation($"{nameof(GetAggregationFilteredData)}");

            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            if (!roles.Contains(Role.Admin) && !roles.Contains(Role.User))
            {
                return Unauthorized();
            }

            var dogApiResponse = await _unitOfWork.DogApiRepository.GetManyDogFacts(request.DogFactLimit);
            var zeldaFanApi = await _unitOfWork.ZeldaFanApiRepository.GetManyZeldaGameInfo(request.ZeldaGameLimit);
            var jokeApiResponse = await _unitOfWork.JokeApiRepository.GetJokeByCategoryName(request.JokeCategoryName);
            var spaceFlightNewsApiResponse = await _unitOfWork.SpaceFlightNewsApiRepository.GetManySpaceNews(request.SpaceNewLimit);

            var result = new AggregationResponse() 
            {
                DogApi = dogApiResponse,
                ZeldaFanApi = zeldaFanApi,
                JokeApi = jokeApiResponse,
                SpaceFlightNewsApi = spaceFlightNewsApiResponse
            };

            return Ok(result);
        }

        [Authorize(Policy = Policy.AdminOrUser)]
        [HttpGet]
        [Route("playground")]
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
