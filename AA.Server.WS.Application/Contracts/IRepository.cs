using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.Server.WS.Application.Contracts
{
    public interface IRepository <T> where T : class
    {
        #region Methods
        Task<List<T>> Get();
        #endregion
    }
}
