﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.Server.WS.Application.Contracts
{
    public interface IUnitOfWork
    {
        #region Fields & Properties
        IDbUserRepository DbUserRepository { get; }
        ICompanyRepository CompanyRepository { get; }
        ICatFactRepository CatFactRepository { get; }
        IDogApiRepository DogApiRepository { get; }
        IZeldaFanApiRepository ZeldaFanApiRepository { get; }
        IJokeApiRepository JokeApiRepository { get; }
        ISpaceFlightNewsApiRepository SpaceFlightNewsApiRepository { get; }
        #endregion
    }
}
