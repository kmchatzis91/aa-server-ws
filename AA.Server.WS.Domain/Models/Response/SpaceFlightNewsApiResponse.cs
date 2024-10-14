using AA.Server.WS.Domain.Entities;
using AA.Server.WS.Domain.Models.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AA.Server.WS.Domain.Models.Response
{
    public class SpaceFlightNewsApiResponse
    {
        public SpaceFlightNewsApi? Values { get; set; }
        public List<Error>? Errors { get; set; }
    }
}
