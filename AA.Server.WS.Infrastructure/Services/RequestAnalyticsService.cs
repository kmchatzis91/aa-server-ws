using AA.Server.WS.Domain.Models.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.Server.WS.Infrastructure.Services
{
    public class RequestAnalyticsService
    {
        #region Fields & Properties
        public List<RequestAnalytics> RequestData { get; private set; } = new();
        public int TotalRequests { get; private set; } = 0;
        public int TotalFilteredRequests { get; private set; } = 0;
        #endregion

        #region Methods
        public void AddRequestAnalytics(RequestAnalytics analytics)
        {
            RequestData.Add(analytics);
            TotalRequests++;
        }

        public void AddFilteredRequestAnalytics(RequestAnalytics analytics)
        {
            RequestData.Add(analytics);
            TotalFilteredRequests++;
        }
        #endregion
    }
}
