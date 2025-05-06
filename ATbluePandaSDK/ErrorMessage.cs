using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATbluePandaSDK
{
    public class ErrorMessage
    {
        public static string ARG_IS_NULL = "Argument is null or empty.";
        public static string ARG_IS_INVALID = "Argument is not valid.";
        public static string USER_NOT_AUTHENTICATED = "BskyUser is not authenticated.";
        public static string LIMIT_NOT_SUPPORTED = "Value not supported";
        public static string USER_ALREADY_FOLLOWED = "BskyUser is already followed.";
        public static string USER_NOT_FOLLOWED = "BskyUser is not followed.";
        public static string POST_ALREADY_LIKED = "Post already liked";
        public static string POST_NOT_LIKED = "Post not liked";
        public static string SAME_DID_USER = "Users cannot have the same DID";
        public static string USER_ALREADY_MUTED = "BskyUser is already muted.";
        public static string USER_NOT_MUTED = "BskyUser is not muted.";
        public static string USER_ALREADY_BLOCKED = "BskyUser is already blocked.";
        public static string USER_NOT_BLOCKED = "BskyUser is not blocked.";
    }
}
