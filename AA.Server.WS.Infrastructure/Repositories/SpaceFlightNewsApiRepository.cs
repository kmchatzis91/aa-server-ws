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
        public async Task<SpaceFlightNewsResponse> GetNew()
        {
            try
            {
                _logger.LogInformation($"{nameof(GetNew)}");

                var endpoint = "articles/?limit=1&format=json";
                var httpClient = _httpClientFactory.CreateClient(HttpClientName.SpaceFlightNewsApi.ToString());

                var response = await httpClient.GetAsync(endpoint);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var parsedContent = JsonConvert.DeserializeObject<SpaceFlightNewsResponse>(content);

                return parsedContent;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetNew)}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task<SpaceFlightNewsResponse> GetManyNews(int limit)
        {
            try
            {
                _logger.LogInformation($"{nameof(GetManyNews)}, limit: {limit}");

                var endpoint = $"articles/?limit={limit}&format=json";

                if (limit == 0)
                {
                    return null;
                }

                var httpClient = _httpClientFactory.CreateClient(HttpClientName.SpaceFlightNewsApi.ToString());

                var response = await httpClient.GetAsync(endpoint);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var parsedContent = JsonConvert.DeserializeObject<SpaceFlightNewsResponse>(content);

                return parsedContent;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetManyNews)}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return null;
            }
        }
        #endregion
    }
}
