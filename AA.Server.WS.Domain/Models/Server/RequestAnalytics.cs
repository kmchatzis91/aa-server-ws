using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.Server.WS.Domain.Models.Server
{
    public class RequestAnalytics
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string? User { get; set; }
        public long ResponseTimeMs { get; set; }
        public string? Performance => ResponseTimeMs switch
        {
            < 100 => "fast",
            >= 100 and <= 200 => "average",
            > 200 => "slow"
        };
    }
}
