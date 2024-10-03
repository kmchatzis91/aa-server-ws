using AA.Server.WS.Application.Contracts;
using AA.Server.WS.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using AA.Server.WS.Domain.Models.Response;

namespace AA.Server.WS.Infrastructure.Repositories
{
    public class CatFactRepository : ICatFactRepository
    {
        #region Fields & Properties
        private readonly IConfiguration _configuration;
        private readonly ILogger<CatFactRepository> _logger;
        private readonly HttpClient _httpClient;
        #endregion

        #region Constructor
        public CatFactRepository(
            IConfiguration configuration,
            ILogger<CatFactRepository> logger,
            HttpClient httpClient)
        {
            _configuration = configuration;
            _logger = logger;
            _httpClient = httpClient;
        }
        #endregion

        #region Methods
        public async Task<List<CatFact>> Get()
        {
            _logger.LogInformation($"{nameof(Get)}");
            await Task.Delay(500);
            return new List<CatFact>();
        }

        public async Task<CatFact> GetCatFact()
        {
            try
            {
                _logger.LogInformation($"{nameof(GetCatFact)}");

                var endpoint = "https://catfact.ninja/fact";
                var response = await _httpClient.GetAsync(endpoint);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var parsedContent = JsonConvert.DeserializeObject<CatFactResponse>(content);

                var catFact = new CatFact()
                {
                    Fact = parsedContent.Fact,
                    Length = parsedContent.Length,
                };

                return catFact;

            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetCatFact)}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return null;
            }
        }
        #endregion

    }
}
