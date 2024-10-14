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
        #endregion

        #region Constructor
        public UnitOfWork(
            IDbUserRepository dbUserRepository,
            ICompanyRepository companyRepository,
            ICatFactRepository catFactRepository)
        {
            DbUserRepository = dbUserRepository;
            CompanyRepository = companyRepository;
            CatFactRepository = catFactRepository;
        }
        #endregion
    }
}
