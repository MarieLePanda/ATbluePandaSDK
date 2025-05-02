using ATbluePandaSDK.Models;
using System.Text.Json.Serialization;

namespace ATPandaSDK.Models.Auth
{
    public class BskyAuthUser : Response
    {
        [JsonPropertyName("did")]
        public string Did { get; set; }

        [JsonPropertyName("handle")]
        public string Handle { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("emailConfirmed")]
        public bool EmailConfirmed { get; set; }

        [JsonPropertyName("accessJwt")]
        public string AccessJwt { get; set; }

        [JsonPropertyName("refreshJwt")]
        public string RefreshJwt { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }


        public bool IsAuthenticated()
        {
            return !string.IsNullOrEmpty(AccessJwt);
        }

    }

}
