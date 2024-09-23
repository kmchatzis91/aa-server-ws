using AA.Server.WS.Domain.Entities;
using AA.Server.WS.Domain.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.Server.WS.Application.Contracts
{
    public interface IDbUserRepository : IRepository<DbUser>
    {
        #region Methods
        Task<List<DbUserResponse>> GetUsersView();
        Task<DbUserResponse> GetByUsername(string username);
        #endregion
    }
}
