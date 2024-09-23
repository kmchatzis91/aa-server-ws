using AA.Server.WS.Application.Contracts;
using AA.Server.WS.Domain.Entities;
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
    public class CompanyRepository : ICompanyRepository
    {
        #region Fields & Properties
        private readonly IConfiguration _configuration;
        private readonly ILogger<CompanyRepository> _logger;
        private readonly DapperContext _dapperContext;
        #endregion

        #region Constructor
        public CompanyRepository(
            IConfiguration configuration,
            ILogger<CompanyRepository> logger,
            DapperContext dapperContext)
        {
            _configuration = configuration;
            _logger = logger;
            _dapperContext = dapperContext;
        }
        #endregion

        #region Methods
        public async Task<List<Company>> Get()
        {
            try
            {
                _logger.LogInformation($"{nameof(Get)}");

                // If using db, in our case we have a json file
                //using (var connection = _dapperContext.CreateConnection())
                //{
                //    var query = "select * from company";
                //    var companies = await connection.QueryAsync<Company>(query);

                //    if (companies.Count() <= 0 || companies == null)
                //    {
                //        return new List<Company>();
                //    }

                //    return companies.ToList();
                //}

                var testCompaniesPath = "test-data/companies.json";
                var testCompanies = File.ReadAllText(testCompaniesPath);
                var companies = JsonConvert.DeserializeObject<List<Company>>(testCompanies);
                return companies;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Get)}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return new List<Company>();
            }
        }

        public async Task<Company> GetById(Guid id)
        {
            try
            {
                _logger.LogInformation($"{nameof(GetById)}");

                // If using db, in our case we have a json file
                //using (var connection = _dapperContext.CreateConnection())
                //{
                //    var query = $" select * from company where company_id = '{id}'";
                //    var company = await connection.QueryAsync<Company>(query);

                //    if (company == null)
                //    {
                //        return new Company();
                //    }

                //    return company.FirstOrDefault();
                //}

                var testCompaniesPath = "test-data/companies.json";
                var testCompanies = File.ReadAllText(testCompaniesPath);
                var companies = JsonConvert.DeserializeObject<List<Company>>(testCompanies);
                var company = companies.FirstOrDefault(c => c.Id == id);
                return company;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetById)}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return new Company();
            }
        }
        #endregion
    }
}

