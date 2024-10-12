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
    public class JokeApiRepository : IJokeApiRepository
    {
        #region Fields & Properties
        private readonly IConfiguration _configuration;
        private readonly ILogger<JokeApiRepository> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        #endregion

        #region Constructor
        public JokeApiRepository(
            IConfiguration configuration,
            ILogger<JokeApiRepository> logger,
            IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }
        #endregion

        #region Methods
        public async Task<JokeApiResponse> GetJoke()
        {
            try
            {
                _logger.LogInformation($"{nameof(GetJoke)}");

                var endpoint = "any?type=twopart";
                var httpClient = _httpClientFactory.CreateClient(HttpClientName.JokeApi.ToString());
                var result = new JokeApiResponse();

                var response = await httpClient.GetAsync(endpoint);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    result.Values = null;
                    result.Errors = new List<Error>();
                    result.Errors.Add(new Error() { Code = ErrorCode.UnavailableEndpoint, Message = ErrorMessage.UnavailableEndpoint });
                    return result;
                }

                var content = await response.Content.ReadAsStringAsync();
                var parsedContent = JsonConvert.DeserializeObject<JokeApi>(content);

                result.Values = parsedContent;
                result.Errors = null;

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetJoke)}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");

                var result = new JokeApiResponse();
                result.Values = null;
                result.Errors = new List<Error>();
                result.Errors.Add(new Error() { Code = ErrorCode.Unexpected, Message = ErrorMessage.Unexpected });
                return result;
            }
        }

        public async Task<JokeApiResponse> GetJokeByCategoryName(string category)
        {
            try
            {
                _logger.LogInformation($"{nameof(GetJokeByCategoryName)}, category: {category}");

                var endpoint = $"{category}?type=twopart";
                var result = new JokeApiResponse();

                if (string.IsNullOrWhiteSpace(category) || !JokeCategories.Values.Contains(category.ToLower()))
                {
                    result.Values = null;
                    result.Errors = new List<Error>();
                    result.Errors.Add(new Error() { Code = ErrorCode.WrongInput, Message = ErrorMessage.WrongInput });
                    return result;
                }

                var httpClient = _httpClientFactory.CreateClient(HttpClientName.JokeApi.ToString());
                var response = await httpClient.GetAsync(endpoint);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    result.Values = null;
                    result.Errors = new List<Error>();
                    result.Errors.Add(new Error() { Code = ErrorCode.UnavailableEndpoint, Message = ErrorMessage.UnavailableEndpoint });
                    return result;
                }

                var content = await response.Content.ReadAsStringAsync();
                var parsedContent = JsonConvert.DeserializeObject<JokeApi>(content);

                result.Values = parsedContent;
                result.Errors = null;

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetJokeByCategoryName)}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");

                var result = new JokeApiResponse();
                result.Values = null;
                result.Errors = new List<Error>();
                result.Errors.Add(new Error() { Code = ErrorCode.Unexpected, Message = ErrorMessage.Unexpected });
                return result;
            }
        }
        #endregion
    }
}
