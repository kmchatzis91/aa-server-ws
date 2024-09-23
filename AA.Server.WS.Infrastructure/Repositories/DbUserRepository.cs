using AA.Server.WS.Application.Contracts;
using AA.Server.WS.Domain.Entities;
using AA.Server.WS.Domain.Models.Response;
using AA.Server.WS.Infrastructure.Context;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.Server.WS.Infrastructure.Repositories
{
    public class DbUserRepository : IDbUserRepository
    {
        #region Fields & Properties
        private readonly IConfiguration _configuration;
        private readonly ILogger<DbUserRepository> _logger;
        private readonly DapperContext _dapperContext;
        #endregion

        #region Constructor
        public DbUserRepository(
            IConfiguration configuration,
            ILogger<DbUserRepository> logger,
            DapperContext dapperContext)
        {
            _configuration = configuration;
            _logger = logger;
            _dapperContext = dapperContext;
        }
        #endregion

        #region Methods
        public async Task<List<DbUser>> Get()
        {
            try
            {
                _logger.LogInformation($"{nameof(Get)}");

                var testUsersPath = "test-data/users.json";
                var testUsers = File.ReadAllText(testUsersPath);
                var users = JsonConvert.DeserializeObject<List<DbUser>>(testUsers);
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Get)}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return new List<DbUser>();
            }
        }

        public async Task<List<DbUserResponse>> GetUsersView()
        {
            try
            {
                _logger.LogInformation($"{nameof(GetUsersView)}");

                var testUsersPath = "test-data/users.json";
                var testUsers = File.ReadAllText(testUsersPath);
                var users = JsonConvert.DeserializeObject<List<DbUserResponse>>(testUsers);
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetUsersView)}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return new List<DbUserResponse>();
            }
        }

        public async Task<DbUserResponse> GetByUsername(string username)
        {
            try
            {
                _logger.LogInformation($"{nameof(GetByUsername)}, username: {username}");

                var testUsersPath = "test-data/users.json";
                var testUsers = File.ReadAllText(testUsersPath);
                var users = JsonConvert.DeserializeObject<List<DbUserResponse>>(testUsers);
                var user = users.FirstOrDefault(x => x.Username == username);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetByUsername)}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return new DbUserResponse();
            }
        }
        #endregion
    }
}
