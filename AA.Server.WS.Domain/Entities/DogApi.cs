using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AA.Server.WS.Domain.Entities
{
    public class DogApi
    {
        [JsonPropertyName("data")]
        public List<DogFact>? Data { get; set; }
    }
}
