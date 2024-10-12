using AA.Server.WS.Domain.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.Server.WS.Application.Contracts
{
    public interface IZeldaFanApiRepository
    {
        #region Methods
        Task<ZeldaFanApiResponse> GetZeldaGameInfo();
        Task<ZeldaFanApiResponse> GetManyZeldaGameInfo(int limit);
        #endregion
    }
}
