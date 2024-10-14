using AA.Server.WS.Domain.Entities;
using AA.Server.WS.Domain.Models.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.Server.WS.Domain.Models.Response
{
    public class AggregationResponse
    {
        public DogApiResponse? DogApi { get; set; }
        public ZeldaFanApiResponse? ZeldaFanApi { get; set; }
        public JokeApiResponse? JokeApi { get; set; }
        public SpaceFlightNewsApiResponse? SpaceFlightNewsApi { get; set; }
    }
}
