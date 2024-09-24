using AA.Server.WS.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AA.Server.WS.Infrastructure.Services
{
    public class PasswordService
    {
        #region Fields & Properties
        private readonly IConfiguration _configuration;
        private readonly ILogger<PasswordService> _logger;
        #endregion

        #region Constructor
        public PasswordService(
            IConfiguration configuration,
            ILogger<PasswordService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        #endregion

        #region Methods
        public string GetPasswordHash(string password)
        {
            using (var sha = SHA256.Create())
            {
                _logger.LogInformation($"{nameof(GetPasswordHash)}");

                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha.ComputeHash(passwordBytes);
                string hashedPassword = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return hashedPassword;
            }
        }
        #endregion
    }
}
