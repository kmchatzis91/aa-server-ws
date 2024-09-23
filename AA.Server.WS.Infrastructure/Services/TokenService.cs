using AA.Server.WS.Domain.Entities;
using AA.Server.WS.Domain.Models.Request;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AA.Server.WS.Infrastructure.Services
{
    public class TokenService
    {
        #region Fields & Properties
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenService> _logger;
        #endregion

        #region Constructor
        public TokenService(
            IConfiguration configuration,
            ILogger<TokenService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        #endregion

        #region Methods
        public Token GenerateToken(DbUser dbUser)
        {
            try
            {
                _logger.LogInformation($"{nameof(GenerateToken)}, dbUser: {JsonConvert.SerializeObject(dbUser)}");

                if (dbUser is null)
                {
                    _logger.LogError($"{nameof(GenerateToken)}, Message: dbUser can not be null!");
                    return null;
                }

                var claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("userId", dbUser.Id.ToString()),
                    new Claim("firstName", dbUser.FirstName),
                    new Claim("lastName", dbUser.LastName),
                    new Claim("role", dbUser.Role),
                    new Claim(JwtRegisteredClaimNames.Email, dbUser.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, dbUser.Email),
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var jwtToken = new JwtSecurityToken
                (
                    issuer: _configuration["JwtSettings:Issuer"],
                    audience: _configuration["JwtSettings:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddSeconds(Convert.ToInt32(_configuration["JwtSettings:ExpirationSeconds"])),
                    signingCredentials: credentials
                );

                var token = new Token()
                {
                    Value = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    ExpirationSeconds = Convert.ToInt32(_configuration["JwtSettings:ExpirationSeconds"])
                };

                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GenerateToken)}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return null;
            }
        }
        #endregion
    }
}
