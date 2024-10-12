using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AA.Server.WS.Domain.Entities;

namespace AA.Server.WS.Domain.Models.Response
{
    public class DogApiResponse
    {
        [JsonPropertyName("data")]
        public List<DogFact>? Data { get; set; }
    }
}
