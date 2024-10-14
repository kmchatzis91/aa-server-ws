using AA.Server.WS.Domain.Models.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.Server.WS.Domain.Models.Response
{
    public class RequestAnalyticsResponse
    {
        public List<RequestAnalytics>? Analytics { get; set; }
        public int TotalRequests { get; set; }
    }
}
