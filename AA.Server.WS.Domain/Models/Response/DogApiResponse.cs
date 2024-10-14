using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AA.Server.WS.Domain.Entities;
using AA.Server.WS.Domain.Models.Server;

namespace AA.Server.WS.Domain.Models.Response
{
    public class DogApiResponse
    {
        public DogApi? Values { get; set; }
        public List<Error>? Errors { get; set; }
    }
}
