using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AA.Server.WS.Domain.Entities
{
    public class Launch
    {
        [JsonPropertyName("launch_id")]
        public string? LaunchId { get; set; }

        [JsonPropertyName("provider")]
        public string? Provider { get; set; }
    }
}
