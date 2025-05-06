using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ATbluePandaSDK.Models.Account
{
    public class BskyBlockByResponse : Response
    {
        [JsonPropertyName("blocks")]
        public List<Block> Blocks { get; set; }
    }
}
