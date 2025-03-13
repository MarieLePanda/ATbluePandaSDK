using ATPandaSDK.Models.Feed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ATbluePandaSDK.Models
{
    public class User
    {
        [JsonPropertyName("did")]
        public string Did { get; set; }

        [JsonPropertyName("handle")]
        public string Handle { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        [JsonPropertyName("viewer")]
        public Viewer Viewer { get; set; }
    }
}
