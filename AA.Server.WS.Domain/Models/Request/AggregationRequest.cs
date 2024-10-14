using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.Server.WS.Domain.Models.Request
{
    public class AggregationRequest
    {
        public int DogFactLimit { get; set; }
        public int ZeldaGameLimit { get; set; }
        public string? JokeCategoryName { get; set; }
        public int SpaceNewLimit { get; set; }
    }
}
