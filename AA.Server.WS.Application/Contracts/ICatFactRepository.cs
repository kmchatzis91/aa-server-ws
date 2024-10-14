using AA.Server.WS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.Server.WS.Application.Contracts
{
    public interface ICatFactRepository : IRepository<CatFact>
    {
        #region Methods
        Task<CatFact> GetCatFact();
        #endregion
    }
}
