using ATPandaSDK.Models.Feed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATbluePandaSDK.Models.Account
{
    public class Block
    {
        public string did { get; set; }
        public string handle { get; set; }
        public string displayName { get; set; }
        public string avatar { get; set; }
        public Associated associated { get; set; }
        public Viewer viewer { get; set; }
        public List<Label> labels { get; set; }
        public DateTime createdAt { get; set; }
        public string description { get; set; }
        public DateTime indexedAt { get; set; }
    }
}
