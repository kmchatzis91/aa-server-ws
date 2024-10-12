using AA.Server.WS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AA.Server.WS.Domain.Models.Response
{
    public class JokeApiResponse
    {
        [JsonPropertyName("error")]
        public bool Error { get; set; }

        [JsonPropertyName("category")]
        public string? Category { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("setup")]
        public string? Setup { get; set; }

        [JsonPropertyName("delivery")]
        public string? Delivery { get; set; }

        [JsonPropertyName("flags")]
        public Flag? Flags { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("safe")]
        public bool Safe { get; set; }

        [JsonPropertyName("lang")]
        public string? Lang { get; set; }
    }
}
