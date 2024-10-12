using AA.Server.WS.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.Server.WS.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields & Properties
        public IDbUserRepository DbUserRepository { get; set; }
        public ICompanyRepository CompanyRepository { get; set; }
        public ICatFactRepository CatFactRepository { get; set; }
        public IDogApiRepository DogApiRepository { get; set; }
        public IZeldaFanApiRepository ZeldaFanApiRepository { get; set; }
        public IJokeApiRepository JokeApiRepository { get; set; }
        #endregion

        #region Constructor
        public UnitOfWork(
            IDbUserRepository dbUserRepository,
            ICompanyRepository companyRepository,
            ICatFactRepository catFactRepository,
            IDogApiRepository dogApiRepository,
            IZeldaFanApiRepository zeldaFanApiRepository,
            IJokeApiRepository jokeApiRepository)
        {
            DbUserRepository = dbUserRepository;
            CompanyRepository = companyRepository;
            CatFactRepository = catFactRepository;
            DogApiRepository = dogApiRepository;
            ZeldaFanApiRepository = zeldaFanApiRepository;
            JokeApiRepository = jokeApiRepository;
        }
        #endregion
    }
}
