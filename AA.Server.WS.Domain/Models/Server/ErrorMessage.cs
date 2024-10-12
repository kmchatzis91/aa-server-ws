using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.Server.WS.Domain.Models.Server
{
    public class ErrorMessage
    {
        #region Fields & Properties
        public const string Unexpected = "An unexpected error occurred!";
        public const string UnavailableEndpoint = "Endpoint unavailable!";
        public const string WrongInput = "Wrong input!";
        #endregion
    }
}
