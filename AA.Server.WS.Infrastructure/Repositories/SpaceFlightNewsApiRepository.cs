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
    public class SpaceFlightNewsApiRepository : ISpaceFlightNewsApiRepository
    {
        #region Fields & Properties
        private readonly IConfiguration _configuration;
        private readonly ILogger<SpaceFlightNewsApiRepository> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        #endregion

        #region Constructor
        public SpaceFlightNewsApiRepository(
            IConfiguration configuration,
            ILogger<SpaceFlightNewsApiRepository> logger,
            IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }
        #endregion

        #region Methods
        public async Task<SpaceFlightNewsApiResponse> GetSpaceNew()
        {
            try
            {
                _logger.LogInformation($"{nameof(GetSpaceNew)}");

                var endpoint = "articles/?limit=1&format=json";
                var httpClient = _httpClientFactory.CreateClient(HttpClientName.SpaceFlightNewsApi.ToString());
                var result = new SpaceFlightNewsApiResponse();

                var response = await httpClient.GetAsync(endpoint);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    result.Values = null;
                    result.Errors = new List<Error>();
                    result.Errors.Add(new Error() { Code = ErrorCode.UnavailableEndpoint, Message = ErrorMessage.UnavailableEndpoint });
                    return result;
                }

                var content = await response.Content.ReadAsStringAsync();
                var parsedContent = JsonConvert.DeserializeObject<SpaceFlightNewsApi>(content);

                result.Values = parsedContent;
                result.Errors = null;

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetSpaceNew)}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");

                var result = new SpaceFlightNewsApiResponse();
                result.Values = null;
                result.Errors = new List<Error>();
                result.Errors.Add(new Error() { Code = ErrorCode.Unexpected, Message = ErrorMessage.Unexpected });
                return result;
            }
        }

        public async Task<SpaceFlightNewsApiResponse> GetManySpaceNews(int limit)
        {
            try
            {
                _logger.LogInformation($"{nameof(GetManySpaceNews)}, limit: {limit}");

                var endpoint = $"articles/?limit={limit}&format=json";
                var result = new SpaceFlightNewsApiResponse();

                if (limit <= 0)
                {
                    result.Values = null;
                    result.Errors = new List<Error>();
                    result.Errors.Add(new Error() { Code = ErrorCode.WrongInput, Message = ErrorMessage.WrongInput });
                    return result;
                }

                var httpClient = _httpClientFactory.CreateClient(HttpClientName.SpaceFlightNewsApi.ToString());
                var response = await httpClient.GetAsync(endpoint);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    result.Values = null;
                    result.Errors = new List<Error>();
                    result.Errors.Add(new Error() { Code = ErrorCode.UnavailableEndpoint, Message = ErrorMessage.UnavailableEndpoint });
                    return result;
                }

                var content = await response.Content.ReadAsStringAsync();
                var parsedContent = JsonConvert.DeserializeObject<SpaceFlightNewsApi>(content);

                result.Values = parsedContent;
                result.Errors = null;

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetManySpaceNews)}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");

                var result = new SpaceFlightNewsApiResponse();
                result.Values = null;
                result.Errors = new List<Error>();
                result.Errors.Add(new Error() { Code = ErrorCode.Unexpected, Message = ErrorMessage.Unexpected });
                return result;
            }
        }
        #endregion
    }
}
