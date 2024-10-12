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
    public class DogFactRepository : IDogApiRepository
    {
        #region Fields & Properties
        private readonly IConfiguration _configuration;
        private readonly ILogger<DogFactRepository> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        #endregion

        #region Constructor
        public DogFactRepository(
            IConfiguration configuration,
            ILogger<DogFactRepository> logger,
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

                var response = await httpClient.GetAsync(endpoint);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var parsedContent = JsonConvert.DeserializeObject<DogApiResponse>(content);

                return parsedContent;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetDogFact)}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task<DogApiResponse> GetManyDogFacts(int limit)
        {
            try
            {
                _logger.LogInformation($"{nameof(GetManyDogFacts)}");

                var endpoint = $"facts?limit={limit}";
                var httpClient = _httpClientFactory.CreateClient(HttpClientName.DogApi.ToString());

                var response = await httpClient.GetAsync(endpoint);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var parsedContent = JsonConvert.DeserializeObject<DogApiResponse>(content);

                return parsedContent;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetManyDogFacts)}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task<CatFact> GetCatFact()
        {
            try
            {
                //_logger.LogInformation($"{nameof(GetCatFact)}");

                //var endpoint = "https://catfact.ninja/fact";
                //var response = await _httpClient.GetAsync(endpoint);

                //if (response.StatusCode != HttpStatusCode.OK)
                //{
                //    return null;
                //}

                //var content = await response.Content.ReadAsStringAsync();
                //var parsedContent = JsonConvert.DeserializeObject<CatFactResponse>(content);

                //var catFact = new CatFact()
                //{
                //    Fact = parsedContent.Fact,
                //    Length = parsedContent.Length,
                //};

                //return catFact;

                return null;

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
