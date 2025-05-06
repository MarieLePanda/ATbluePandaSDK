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

        [JsonPropertyName("associated")]
        public Associated Associated { get; set; }

        [JsonPropertyName("label")]
        public List<object> Labels { get; set; }
        
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        [JsonPropertyName("indexedAt")]
        public DateTime IndexedAt { get; set; }

        [JsonPropertyName("banner")]
        public string Banner { get; set; }

        [JsonPropertyName("followersCount")]
        public int FollowersCount { get; set; }

        [JsonPropertyName("followsCount")]
        public int FollowsCount { get; set; }

        [JsonPropertyName("postsCount")]
        public int PostsCount { get; set; }
        

    }

    public class KnownFollowers
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("followers")]
        public List<User> Followers { get; set; }
    }

}
