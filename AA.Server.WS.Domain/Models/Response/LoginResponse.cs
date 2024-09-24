using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.Server.WS.Domain.Models.Response
{
    public class LoginResponse
    {
        public string? AccessToken { get; set; }
        public int Expiration { get; set; }
    }
}
