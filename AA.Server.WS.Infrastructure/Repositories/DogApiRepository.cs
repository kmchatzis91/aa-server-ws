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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AA.Server.WS.Infrastructure.Repositories
{
    public class DogApiRepository : IDogApiRepository
    {
        #region Fields & Properties
        private readonly IConfiguration _configuration;
        private readonly ILogger<DogApiRepository> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        #endregion

        #region Constructor
        public DogApiRepository(
            IConfiguration configuration,
            ILogger<DogApiRepository> logger,
            IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }
        #endregion

        #region Methods
        public async Task<DogApiResponse> GetDogFact()
        {
            try
            {
                _logger.LogInformation($"{nameof(GetDogFact)}");

                var endpoint = "facts?limit=1";
                var httpClient = _httpClientFactory.CreateClient(HttpClientName.DogApi.ToString());
                var result = new DogApiResponse();

                var response = await httpClient.GetAsync(endpoint);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    result.Values = null;
                    result.Errors = new List<Error>();
                    result.Errors.Add(new Error() { Code = ErrorCode.UnavailableEndpoint, Message = ErrorMessage.UnavailableEndpoint });
                    return result;
                }

                var content = await response.Content.ReadAsStringAsync();
                var parsedContent = JsonConvert.DeserializeObject<DogApi>(content);

                result.Values = parsedContent;
                result.Errors = null;

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetDogFact)}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");

                var result = new DogApiResponse();
                result.Values = null;
                result.Errors = new List<Error>();
                result.Errors.Add(new Error() { Code = ErrorCode.Unexpected, Message = ErrorMessage.Unexpected });
                return result;
            }
        }

        public async Task<DogApiResponse> GetManyDogFacts(int limit)
        {
            try
            {
                _logger.LogInformation($"{nameof(GetManyDogFacts)}, limit: {limit}");

                var endpoint = $"facts?limit={limit}";
                var result = new DogApiResponse();

                if (limit <= 0) 
                {
                    result.Values = null;
                    result.Errors = new List<Error>();
                    result.Errors.Add(new Error() { Code = ErrorCode.WrongInput, Message = ErrorMessage.WrongInput });
                    return result;
                }

                var httpClient = _httpClientFactory.CreateClient(HttpClientName.DogApi.ToString());
                var response = await httpClient.GetAsync(endpoint);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    result.Values = null;
                    result.Errors = new List<Error>();
                    result.Errors.Add(new Error() { Code = ErrorCode.UnavailableEndpoint, Message = ErrorMessage.UnavailableEndpoint });
                    return result;
                }

                var content = await response.Content.ReadAsStringAsync();
                var parsedContent = JsonConvert.DeserializeObject<DogApi>(content);

                result.Values = parsedContent;
                result.Errors = null;

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetManyDogFacts)}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");

                var result = new DogApiResponse();
                result.Values = null;
                result.Errors = new List<Error>();
                result.Errors.Add(new Error() { Code = ErrorCode.Unexpected, Message = ErrorMessage.Unexpected });
                return result;
            }
        }
        #endregion
    }
}
