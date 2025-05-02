using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATbluePandaSDK.Models
{
    public class BskyException : Exception
    {
        public BskyException(string message) : base(message)
        {
        }
    }
}
