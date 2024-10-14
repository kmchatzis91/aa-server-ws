using AA.Server.WS.Domain.Entities;
using AA.Server.WS.Domain.Models.Response;
using AA.Server.WS.Domain.Models.Server;
using AA.Server.WS.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AA.Server.WS.Infrastructure.Tests.Repositories
{
    public class DogApiRepositoryTests
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<ILogger<DogApiRepository>> _loggerMock;
        private readonly IConfiguration _configurationMock;
        private readonly DogApiRepository _dogApiRepository;

        public DogApiRepositoryTests()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _loggerMock = new Mock<ILogger<DogApiRepository>>();
            _configurationMock = new Mock<IConfiguration>().Object;
            _dogApiRepository = new DogApiRepository(_configurationMock, _loggerMock.Object, _httpClientFactoryMock.Object);
        }

        [Fact]
        public async Task GetDogFact_ReturnsDogFact_WhenApiCallIsSuccessful()
        {
            // Arrange
            var expectedDogApiResponse = new DogApi
            {
                Data = new List<DogFact>
                {
                    new DogFact
                    {
                        Id = "1",
                        Type = "fact",
                        Attributes = new DogFactAttributes { Body = "Dogs are loyal!" }
                    }
                }
            };

            var jsonResponse = JsonConvert.SerializeObject(expectedDogApiResponse);
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse)
            };

            var httpClientMock = new Mock<HttpMessageHandler>();
            httpClientMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    It.IsAny<HttpRequestMessage>(),
                    It.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            var httpClient = new HttpClient(httpClientMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _dogApiRepository.GetDogFact();

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Values);
            Assert.Single(result.Values.Data);
            Assert.Equal("Dogs are loyal!", result.Values.Data[0].Attributes.Body);
            Assert.Null(result.Errors);
        }

        [Fact]
        public async Task GetDogFact_ReturnsError_WhenApiCallFails()
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
            var httpClientMock = new Mock<HttpMessageHandler>();
            httpClientMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    It.IsAny<HttpRequestMessage>(),
                    It.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            var httpClient = new HttpClient(httpClientMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _dogApiRepository.GetDogFact();

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Values);
            Assert.NotNull(result.Errors);
            Assert.Single(result.Errors);
            Assert.Equal(ErrorCode.UnavailableEndpoint, result.Errors[0].Code);
        }

        [Fact]
        public async Task GetDogFact_ReturnsError_WhenExceptionOccurs()
        {
            // Arrange
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Throws(new Exception("Test exception"));

            // Act
            var result = await _dogApiRepository.GetDogFact();

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Values);
            Assert.NotNull(result.Errors);
            Assert.Single(result.Errors);
            Assert.Equal(ErrorCode.Unexpected, result.Errors[0].Code);
        }

        [Fact]
        public async Task GetManyDogFacts_ReturnsDogFacts_WhenApiCallIsSuccessful()
        {
            // Arrange
            int limit = 2;
            var expectedDogApiResponse = new DogApi
            {
                Data = new List<DogFact>
                {
                    new DogFact
                    {
                        Id = "1",
                        Type = "fact",
                        Attributes = new DogFactAttributes { Body = "Dogs love to play!" }
                    },
                    new DogFact
                    {
                        Id = "2",
                        Type = "fact",
                        Attributes = new DogFactAttributes { Body = "Dogs need regular exercise!" }
                    }
                }
            };

            var jsonResponse = JsonConvert.SerializeObject(expectedDogApiResponse);
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse)
            };

            var httpClientMock = new Mock<HttpMessageHandler>();
            httpClientMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    It.IsAny<HttpRequestMessage>(),
                    It.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            var httpClient = new HttpClient(httpClientMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _dogApiRepository.GetManyDogFacts(limit);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Values);
            Assert.Equal(2, result.Values.Data.Count);
            Assert.Equal("Dogs love to play!", result.Values.Data[0].Attributes.Body);
            Assert.Null(result.Errors);
        }

        [Fact]
        public async Task GetManyDogFacts_ReturnsError_WhenLimitIsZeroOrNegative()
        {
            // Arrange
            int limit = 0; // Zero limit for test
            // Act
            var result = await _dogApiRepository.GetManyDogFacts(limit);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Values);
            Assert.NotNull(result.Errors);
            Assert.Single(result.Errors);
            Assert.Equal(ErrorCode.WrongInput, result.Errors[0].Code);
        }

        [Fact]
        public async Task GetManyDogFacts_ReturnsError_WhenApiCallFails()
        {
            // Arrange
            int limit = 2; // Valid limit
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
            var httpClientMock = new Mock<HttpMessageHandler>();
            httpClientMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    It.IsAny<HttpRequestMessage>(),
                    It.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            var httpClient = new HttpClient(httpClientMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _dogApiRepository.GetManyDogFacts(limit);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Values);
            Assert.NotNull(result.Errors);
            Assert.Single(result.Errors);
            Assert.Equal(ErrorCode.UnavailableEndpoint, result.Errors[0].Code);
        }

        [Fact]
        public async Task GetManyDogFacts_ReturnsError_WhenExceptionOccurs()
        {
            // Arrange
            int limit = 2; // Valid limit
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Throws(new Exception("Test exception"));

            // Act
            var result = await _dogApiRepository.GetManyDogFacts(limit);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Values);
            Assert.NotNull(result.Errors);
            Assert.Single(result.Errors);
            Assert.Equal(ErrorCode.Unexpected, result.Errors[0].Code);
        }
    }
}