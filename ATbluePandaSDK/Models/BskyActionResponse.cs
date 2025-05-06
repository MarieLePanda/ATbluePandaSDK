using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ATbluePandaSDK.Models
{
    public class BskyActionResponse : Response
    {
        public BskyActionResponse() { }

        public BskyActionResponse(Response response)
        {
            StatusCode = response.StatusCode;
            Result = response.Result;
            ErrorMessage = response.ErrorMessage;
        }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }
        [JsonPropertyName("cid")]
        public string Cid { get; set; }
        [JsonPropertyName("commit")]
        public CommitInfo Commit { get; set; }
        [JsonPropertyName("validationStatus")]
        public string ValidationStatus { get; set; }
    }
}
