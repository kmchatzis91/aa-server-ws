using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.Server.WS.Domain.Models.Server
{
    public class Error
    {
        public ErrorCode Code { get; set; }
        public string? Message { get; set; }
    }
}
