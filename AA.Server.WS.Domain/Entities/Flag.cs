using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AA.Server.WS.Domain.Entities
{
    public class Flag
    {
        [JsonPropertyName("nsfw")]
        public bool Nsfw { get; set; }

        [JsonPropertyName("religious")]
        public bool Religious { get; set; }

        [JsonPropertyName("political")]
        public bool Political { get; set; }

        [JsonPropertyName("racist")]
        public bool Racist { get; set; }

        [JsonPropertyName("sexist")]
        public bool Sexist { get; set; }

        [JsonPropertyName("explicit")]
        public bool Explicit { get; set; }
    }
}
