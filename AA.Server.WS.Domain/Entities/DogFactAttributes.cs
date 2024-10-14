using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AA.Server.WS.Domain.Entities
{
    public class DogFactAttributes
    {
        [JsonPropertyName("body")]
        public string? Body { get; set; }
    }
}
