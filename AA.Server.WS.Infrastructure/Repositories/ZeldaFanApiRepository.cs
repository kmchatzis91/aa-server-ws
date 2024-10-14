using AA.Server.WS.Application.Contracts;
using AA.Server.WS.Domain.Entities;
using AA.Server.WS.Domain.Models.Response;
using AA.Server.WS.Domain.Models.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AA.Server.WS.Infrastructure.Repositories
{
    public class ZeldaFanApiRepository : IZeldaFanApiRepository
    {
        #region Fields & Properties
        private readonly IConfiguration _configuration;
        private readonly ILogger<ZeldaFanApiRepository> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        #endregion

        #region Constructor
        public ZeldaFanApiRepository(
            IConfiguration configuration,
            ILogger<ZeldaFanApiRepository> logger,
            IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }
        #endregion

        #region Methods
        public async Task<ZeldaFanApiResponse> GetZeldaGameInfo()
        {
            try
            {
                _logger.LogInformation($"{nameof(GetZeldaGameInfo)}");

                var endpoint = "games?limit=1";
                var httpClient = _httpClientFactory.CreateClient(HttpClientName.ZeldaFanApi.ToString());
                var result = new ZeldaFanApiResponse();

                var response = await httpClient.GetAsync(endpoint);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    result.Values = null;
                    result.Errors = new List<Error>();
                    result.Errors.Add(new Error() { Code = ErrorCode.UnavailableEndpoint, Message = ErrorMessage.UnavailableEndpoint });
                    return result;
                }

                var content = await response.Content.ReadAsStringAsync();
                var parsedContent = JsonConvert.DeserializeObject<ZeldaFanApi>(content);

                result.Values = parsedContent;
                result.Errors = null;

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetZeldaGameInfo)}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");

                var result = new ZeldaFanApiResponse();
                result.Values = null;
                result.Errors = new List<Error>();
                result.Errors.Add(new Error() { Code = ErrorCode.Unexpected, Message = ErrorMessage.Unexpected });
                return result;
            }
        }

        public async Task<ZeldaFanApiResponse> GetManyZeldaGameInfo(int limit)
        {
            try
            {
                _logger.LogInformation($"{nameof(GetManyZeldaGameInfo)}, limit: {limit}");

                var endpoint = $"games?limit={limit}";
                var result = new ZeldaFanApiResponse();

                if (limit <= 0)
                {
                    result.Values = null;
                    result.Errors = new List<Error>();
                    result.Errors.Add(new Error() { Code = ErrorCode.WrongInput, Message = ErrorMessage.WrongInput });
                    return result;
                }

                var httpClient = _httpClientFactory.CreateClient(HttpClientName.ZeldaFanApi.ToString());
                var response = await httpClient.GetAsync(endpoint);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    result.Values = null;
                    result.Errors = new List<Error>();
                    result.Errors.Add(new Error() { Code = ErrorCode.UnavailableEndpoint, Message = ErrorMessage.UnavailableEndpoint });
                    return result;
                }

                var content = await response.Content.ReadAsStringAsync();
                var parsedContent = JsonConvert.DeserializeObject<ZeldaFanApi>(content);

                result.Values = parsedContent;
                result.Errors = null;

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetManyZeldaGameInfo)}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");

                var result = new ZeldaFanApiResponse();
                result.Values = null;
                result.Errors = new List<Error>();
                result.Errors.Add(new Error() { Code = ErrorCode.Unexpected, Message = ErrorMessage.Unexpected });
                return result;
            }
        }
        #endregion
    }
}
