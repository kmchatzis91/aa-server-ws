using AA.Server.WS.Application.Contracts;
using AA.Server.WS.Domain.Entities;
using AA.Server.WS.Domain.Models.Request;
using AA.Server.WS.Domain.Models.Response;
using AA.Server.WS.Domain.Models.Server;
using AA.Server.WS.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
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
        private readonly RequestAnalyticsService _requestAnalyticsService;
        private List<RequestAnalytics> RequestData = new();
        #endregion

        #region Constructor
        public AggregationController(
            IConfiguration configuration,
            ILogger<AggregationController> logger,
            IUnitOfWork unitOfWork,
            RequestAnalyticsService requestAnalyticsService)
        {
            _configuration = configuration;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _requestAnalyticsService = requestAnalyticsService;
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
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var requestAnalytics = new RequestAnalytics();
            requestAnalytics.Id = 1;
            requestAnalytics.TimeStamp = DateTime.UtcNow;

            _logger.LogInformation($"{nameof(GetAggregationData)}");

            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            if (!roles.Contains(Role.Admin) && !roles.Contains(Role.User))
            {
                stopwatch.Stop();
                requestAnalytics.User = "Anonymous";
                requestAnalytics.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
                RequestData.Add(requestAnalytics);
                _requestAnalyticsService.AddRequestAnalytics(requestAnalytics);

                return Unauthorized();
            }

            var dogApiResponse = _unitOfWork.DogApiRepository.GetDogFact();
            var zeldaFanApi = _unitOfWork.ZeldaFanApiRepository.GetZeldaGameInfo();
            var jokeApiResponse = _unitOfWork.JokeApiRepository.GetJoke();
            var spaceFlightNewsApiResponse = _unitOfWork.SpaceFlightNewsApiRepository.GetSpaceNew();

            await Task.WhenAll(dogApiResponse, zeldaFanApi, jokeApiResponse, spaceFlightNewsApiResponse);

            var result = new AggregationResponse()
            {
                DogApi = await dogApiResponse,
                ZeldaFanApi = await zeldaFanApi,
                JokeApi = await jokeApiResponse,
                SpaceFlightNewsApi = await spaceFlightNewsApiResponse
            };

            stopwatch.Stop();
            requestAnalytics.User = User.Claims.FirstOrDefault(c => c.Type == "username")?.Value;
            requestAnalytics.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            RequestData.Add(requestAnalytics);
            _requestAnalyticsService.AddRequestAnalytics(requestAnalytics);

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
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var requestAnalytics = new RequestAnalytics();
            requestAnalytics.Id = 2;
            requestAnalytics.TimeStamp = DateTime.UtcNow;

            _logger.LogInformation($"{nameof(GetAggregationFilteredData)}");

            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            if (!roles.Contains(Role.Admin) && !roles.Contains(Role.User))
            {
                stopwatch.Stop();
                requestAnalytics.User = "Anonymous";
                requestAnalytics.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
                RequestData.Add(requestAnalytics);
                _requestAnalyticsService.AddFilteredRequestAnalytics(requestAnalytics);

                return Unauthorized();
            }

            var dogApiResponse = _unitOfWork.DogApiRepository.GetManyDogFacts(request.DogFactLimit);
            var zeldaFanApi = _unitOfWork.ZeldaFanApiRepository.GetManyZeldaGameInfo(request.ZeldaGameLimit);
            var jokeApiResponse = _unitOfWork.JokeApiRepository.GetJokeByCategoryName(request.JokeCategoryName);
            var spaceFlightNewsApiResponse = _unitOfWork.SpaceFlightNewsApiRepository.GetManySpaceNews(request.SpaceNewLimit);

            await Task.WhenAll(dogApiResponse, zeldaFanApi, jokeApiResponse, spaceFlightNewsApiResponse);

            var result = new AggregationResponse()
            {
                DogApi = await dogApiResponse,
                ZeldaFanApi = await zeldaFanApi,
                JokeApi = await jokeApiResponse,
                SpaceFlightNewsApi = await spaceFlightNewsApiResponse
            };

            stopwatch.Stop();
            requestAnalytics.User = User.Claims.FirstOrDefault(c => c.Type == "username")?.Value;
            requestAnalytics.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
            RequestData.Add(requestAnalytics);
            _requestAnalyticsService.AddFilteredRequestAnalytics(requestAnalytics);

            return Ok(result);
        }

        [Authorize(Policy = Policy.Admin)]
        [HttpGet]
        [Route("analytics/data")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RequestAnalyticsResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAggregationDataAnalytics()
        {
            _logger.LogInformation($"{nameof(GetAggregationDataAnalytics)}");

            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            if (!roles.Contains(Role.Admin))
            {
                return Unauthorized();
            }

            var requestData = _requestAnalyticsService.RequestData.Where(x => x.Id == 1).ToList();
            var totalRequests = _requestAnalyticsService.TotalRequests;

            var result = new RequestAnalyticsResponse()
            {
                Analytics = requestData,
                TotalRequests = totalRequests
            };

            return Ok(result);
        }

        [Authorize(Policy = Policy.Admin)]
        [HttpGet]
        [Route("analytics/filtered/data")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RequestAnalyticsResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAggregationFilteredDataAnalytics()
        {
            _logger.LogInformation($"{nameof(GetAggregationFilteredDataAnalytics)}");

            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            if (!roles.Contains(Role.Admin))
            {
                return Unauthorized();
            }

            var requestData = _requestAnalyticsService.RequestData.Where(x => x.Id == 2).ToList();
            var totalRequests = _requestAnalyticsService.TotalFilteredRequests;

            var result = new RequestAnalyticsResponse()
            {
                Analytics = requestData,
                TotalRequests = totalRequests
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
