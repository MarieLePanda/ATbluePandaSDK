using ATPandaSDK.Models.Feed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ATbluePandaSDK.Models.Account
{
    public class ListRecordResponse : Response
    {
        [JsonPropertyName("cursor")]
        public string Cursor { get; set; }
        
        [JsonPropertyName("records")]
        public List<Records> Records { get; set; }
    }

    public class Records
    {
        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        [JsonPropertyName("cid")]
        public string Cid { get; set; }

        [JsonPropertyName("value")]
        public Value Value { get; set; }
    }
}
