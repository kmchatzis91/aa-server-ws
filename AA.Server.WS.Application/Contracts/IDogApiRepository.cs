using AA.Server.WS.Domain.Entities;
using AA.Server.WS.Domain.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.Server.WS.Application.Contracts
{
    public interface IDogApiRepository
    {
        #region Methods
        Task<DogApiResponse> GetDogFact();
        Task<DogApiResponse> GetManyDogFacts(int limit);
        #endregion
    }
}
