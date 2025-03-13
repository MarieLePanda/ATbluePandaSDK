using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATbluePandaSDK
{
    public class ErrorMessage
    {
        public static string AUTH_REQUEST_IS_NULL = "Authentication request is null.";
        public static string USER_NAME_OR_PASSWORD_IS_NULL = "Username or password cannot be null or empty.";
        public static string USER_NOT_AUTHENTICATED = "User is not authenticated.";
        public static string TEXT_IS_NULL = "Text cannot be null or empty.";
        public static string LIMIT_IS_NEGATIVE = "Limit must be greater than zero";
        public static string FEED_IS_NULL = "Feed cannot be null or empty.";
        public static string POST_URI_IS_NULL = "Post uri cannot be null or empty.";
        public static string USER_ALREADY_FOLLOWED = "User is already followed.";
        public static string USER_NOT_FOLLOWED = "User is not followed.";
        public static string POST_ALREADY_LIKED = "Post already liked";
        public static string POST_NOT_LIKED = "Post not liked";
        public static string POST_IS_NULL = "Post cannot be null or empty.";
        public static string VIEWER_IS_NULL = "Viewer cannot be null.";
        public static string POST_URI_OR_CID_IS_NULL = "Post uri or cid cannot be null or empty.";
        public static string FOLLOWEE_IS_NULL = "Followee cannot be null.";
        public static string FOLLOWEE_DID_IS_NULL = "Followee DID cannot be null or empty.";
        public static string SAME_DID_USER_FOLLOWEE = "Authenticate user and followee user cannot have the same DID";
    }
}
