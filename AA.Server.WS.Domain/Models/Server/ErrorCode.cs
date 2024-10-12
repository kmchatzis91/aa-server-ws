using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.Server.WS.Domain.Models.Server
{
    public enum ErrorCode
    {
        #region Fields & Properties
        Unexpected = 1,
        UnavailableEndpoint,
        WrongInput,
        #endregion
    }
}
