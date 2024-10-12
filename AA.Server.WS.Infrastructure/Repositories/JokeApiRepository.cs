using AA.Server.WS.Application.Contracts;
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

                var response = await httpClient.GetAsync(endpoint);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var parsedContent = JsonConvert.DeserializeObject<JokeApiResponse>(content);

                return parsedContent;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetJoke)}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task<JokeApiResponse> GetJokeByCategoryName(string category)
        {
            try
            {
                _logger.LogInformation($"{nameof(GetJokeByCategoryName)}");

                var endpoint = $"{category}?type=twopart";

                if (string.IsNullOrWhiteSpace(category))
                {
                    return null;
                }

                if (!JokeCategories.Values.Contains(category.ToLower()))
                {
                    return null;
                }

                var httpClient = _httpClientFactory.CreateClient(HttpClientName.JokeApi.ToString());

                var response = await httpClient.GetAsync(endpoint);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var parsedContent = JsonConvert.DeserializeObject<JokeApiResponse>(content);

                return parsedContent;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetJokeByCategoryName)}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return null;
            }
        }
        #endregion
    }
}
