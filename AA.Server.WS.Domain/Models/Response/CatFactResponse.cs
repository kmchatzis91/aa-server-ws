using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AA.Server.WS.Domain.Models.Response
{
    public class CatFactResponse
    {
        [JsonPropertyName("fact")]
        public string? Fact { get; set; }

        [JsonPropertyName("length")]
        public int Length { get; set; }
    }
}
