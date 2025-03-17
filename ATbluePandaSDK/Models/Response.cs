using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ATbluePandaSDK.Models
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }

        public bool isSuccess()
        {
            return string.IsNullOrWhiteSpace(ErrorMessage);
        }
    }
}
