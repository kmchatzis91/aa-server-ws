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

                var response = await httpClient.GetAsync(endpoint);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var parsedContent = JsonConvert.DeserializeObject<ZeldaFanApiResponse>(content);

                return parsedContent;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetZeldaGameInfo)}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task<ZeldaFanApiResponse> GetManyZeldaGameInfo(int limit)
        {
            try
            {
                _logger.LogInformation($"{nameof(GetManyZeldaGameInfo)}");

                var endpoint = $"games?limit={limit}";
                var httpClient = _httpClientFactory.CreateClient(HttpClientName.ZeldaFanApi.ToString());

                var response = await httpClient.GetAsync(endpoint);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var parsedContent = JsonConvert.DeserializeObject<ZeldaFanApiResponse>(content);

                return parsedContent;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetManyZeldaGameInfo)}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return null;
            }
        }
        #endregion
    }
}
