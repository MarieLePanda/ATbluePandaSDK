using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATbluePandaSDK.Models.Account
{
    public class UserProfileResponse : Response
    {
        public UserProfileResponse() { }

        public UserProfileResponse(Response response)
        {
            StatusCode = response.StatusCode;
            Result = response.Result;
            ErrorMessage = response.ErrorMessage;
        }

        public User BskyUser { get; set; }
    }
}
