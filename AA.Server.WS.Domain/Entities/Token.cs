using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.Server.WS.Domain.Entities
{
    public class Token
    {
        public string? Value { get; set; }
        public int ExpirationSeconds { get; set; }
    }
}
