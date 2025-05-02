

using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using ATPandaSDK.Services;
using ATPandaSDK.Models.Auth;
using ATPandaSDK.Models.Feed;
using System.Collections.Generic;
using ATbluePandaSDK.Models;
using ATbluePandaSDK.Services;

using Microsoft.Extensions.Logging;
using ATbluePandaSDK;
using ATbluePandaSDK.Models.Account;
namespace ATPandaSDK
{
    public class ATPClient
    {
        private readonly AuthService _authService;
        private readonly PostService _postService;
        private readonly FeedService _feedService;
        private readonly AccountService _accountService;
        public BskyAuthUser AuthUser { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ATPClient"/> class with default service implementations.
        /// </summary>
        public ATPClient()
        {
            HttpClient httpClient = new HttpClient { BaseAddress = new Uri(Configuration.BaseUrl) };
            _authService = new AuthService(httpClient);
            _postService = new PostService(httpClient);
            _feedService = new FeedService(httpClient);
            _accountService = new AccountService(httpClient);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ATPClient"/> class with default service implementations.
        /// </summary>
        /// <param name="httpClient">An instance of <see cref="HttpClient"/> to use.</param>
        public ATPClient(BskyAuthUser? auth = null, HttpClient? httpClient = null)
        {
            if(httpClient == null)
            {
                httpClient = new HttpClient { BaseAddress = new Uri(Configuration.BaseUrl) };
            }

            if(auth != null)
            {
                AuthUser = auth;
            }
            HttpClient _httpClient = httpClient;
            _authService = new AuthService(httpClient);
            _postService = new PostService(httpClient);
            _feedService = new FeedService(httpClient);
            _accountService = new AccountService(httpClient);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ATPClient"/> class with the specified service implementations.
        /// </summary>
        /// <param name="authService">The authentication service implementation.</param>
        /// <param name="postService">The post service implementation.</param>
        /// <param name="feedService">The feed service implementation.</param>
        /// <param name="accountService">The account service implementation.</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the service implementations are null.</exception>
        public ATPClient(AuthService authService, PostService postService, FeedService feedService, AccountService accountService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _postService = postService ?? throw new ArgumentNullException(nameof(_postService));
            _feedService = feedService ?? throw new ArgumentNullException(nameof(_feedService));
            _accountService = accountService ?? throw new ArgumentNullException(nameof(_accountService));
        }


        /// <summary>
        /// Creates a post asynchronously.
        /// </summary>
        /// <param name="auth">The authentication response containing the user's tokens.</param>
        /// <param name="text">The text content of the post.</param>
        /// <returns>A <see cref="BskyActionResponse"/> object containing the result of the post creation.</returns>
        /// <remarks>The authenticate user will be also saved in the BskyAuthUser property of this class and use in future
        /// client usage if the BskyAuthUser parameter is not set</remarks>
        /// <exception cref="ArgumentNullException">Thrown when the authentification request is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the authentification request have properies not set correctly.</exception>
        /// <exception cref="BskyException">Thrown when an error occur during the interaction with BlueSky service.</exception>
        public BskyAuthUser Authenticate(AuthRequest authRequest) 
        {
            
            if (authRequest == null)
            {
                throw new ArgumentNullException(nameof(authRequest), ErrorMessage.ARG_IS_NULL);

            }else if(string.IsNullOrEmpty(authRequest.Username) || string.IsNullOrEmpty(authRequest.Password))
            {
                throw new ArgumentException(nameof(authRequest), ErrorMessage.ARG_IS_INVALID);
            }
            else
            {
                try
                {
                    AuthUser = _authService.AuthenticateAsync(authRequest).Result;

                }
                catch (Exception e)
                {
                    throw new BskyException(e.Message);
                }

            }

            return AuthUser;

        }

        /// <summary>
        /// Creates a post with the specified text.
        /// </summary>
        /// <param name="text">The text content of the post.</param>
        /// <param name="auth">The authentication response containing the user's tokens. Default is null, in this case the 
        /// BskyAuthUser property of the client will be used</param>
        /// <returns>A <see cref="BskyActionResponse"/> object containing the result of the post creation.</returns>
        /// <remarks>Don't be mean with other when you post</remarks>
        /// <exception cref="ArgumentNullException">Thrown when the text is null.</exception>
        /// <exception cref="BskyException">Thrown when an error occur during the interaction with BlueSky service or the user in not authenticated.</exception>
        public BskyActionResponse CreatePost(string text, BskyAuthUser? auth = null)
        {
            if (auth == null)
                auth = AuthUser;

            if (auth == null || !auth.IsAuthenticated())
            {
                throw new BskyException(ErrorMessage.USER_NOT_AUTHENTICATED);
            }
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException(nameof(text), ErrorMessage.ARG_IS_NULL);
            }
            try
            {
                return _postService.CreatePostAsync(auth.AccessJwt, auth.Did, text).Result;
            }
            catch (Exception e)
            {
                throw new BskyException(e.Message);
            }
        }

        /// <summary>
        /// Retrieves the user's timeline asynchronously.
        /// </summary>
        /// <param name="limit">The maximum number of posts to retrieve. Default is 30.</param>
        /// <param name="cursor">The cursor for pagination. Default is null.</param>
        /// <param name="auth">The authentication response containing the user's tokens. Default is null, in this case the 
        /// BskyAuthUser property of the client will be used</param>
        /// <returns>A <see cref="BskyTimeline"/> object containing the user's timeline.</returns
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the limit is negative.</exception>
        /// <exception cref="BskyException">Thrown when an error occur during the interaction with BlueSky service or the user in not authenticated.</exception>
        public BskyTimeline GetTimeline(int limit = 30, string? cursor = null, BskyAuthUser? auth = null)
        {
            if (auth == null)
                auth = AuthUser;

            if (auth == null || !auth.IsAuthenticated())
            {
                throw new BskyException(ErrorMessage.USER_NOT_AUTHENTICATED);
            }
            if (limit <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(limit), ErrorMessage.ARG_IS_NEGATIVE);
            }
            try
            {
                return _feedService.GetTimelineAsync(auth.AccessJwt, limit, cursor).Result;
            }
            catch(Exception e)
            {
                throw new BskyException(e.Message);
            }
        }

        /// <summary>
        /// Retrieves a custom timeline asynchronously.
        /// </summary>
        /// <param name="feed">The feed identifier.</param>
        /// <param name="limit">The maximum number of posts to retrieve. Default is 30.</param>
        /// <param name="cursor">The cursor for pagination. Default is null.</param>
        /// <param name="auth">The authentication response containing the user's tokens. Default is null, in this case the 
        /// BskyAuthUser property of the client will be used</param>
        /// <returns>A <see cref="BskyTimeline"/> object containing the custom timeline.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the limit is negative.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the custom feed is null.</exception>
        /// <exception cref="BskyException">Thrown when an error occur during the interaction with BlueSky service or the user in not authenticated.</exception>
        public BskyTimeline GetCustomTimeline(string feed, int limit = 30, string? cursor = null, BskyAuthUser? auth = null)
        {
            if (auth == null)
                auth = AuthUser;

            if (auth == null || !auth.IsAuthenticated())
            {
                throw new BskyException(ErrorMessage.USER_NOT_AUTHENTICATED);
            }
            if (string.IsNullOrWhiteSpace(feed))
            {
                throw new ArgumentNullException(nameof(feed), ErrorMessage.ARG_IS_NULL );
            }
            if(limit <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(limit), ErrorMessage.ARG_IS_NEGATIVE);
            }
            try
            {
                return _feedService.GetFeedGeneratorAsync(auth.AccessJwt, feed, limit, cursor).Result;
            }
            catch (Exception e)
            {
                throw new BskyException(e.Message);
            }
        }

        /// <summary>
        /// Retrieves the author's timeline asynchronously.
        /// </summary>
        /// <param name="limit">The maximum number of posts to retrieve. Default is 30.</param>
        /// <param name="cursor">The cursor for pagination. Default is null.</param>
        /// <param name="auth">The authentication response containing the user's tokens. Default is null, in this case the 
        /// BskyAuthUser property of the client will be used</param>
        /// <returns>A <see cref="BskyTimeline"/> object containing the author's timeline.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the limit is negative.</exception>
        /// <exception cref="BskyException">Thrown when an error occur during the interaction with BlueSky service or the user in not authenticated.</exception>
        public BskyTimeline GetAuthorTimeline(int limit = 30, string cursor = null, BskyAuthUser auth = null)
        {
            if (auth == null)
                auth = AuthUser;

            if (auth == null || !auth.IsAuthenticated())
            {
                throw new BskyException(ErrorMessage.USER_NOT_AUTHENTICATED);
            }
            if (limit <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(limit), ErrorMessage.ARG_IS_NEGATIVE);
            }

            try
            {
                return _feedService.GetAuthorFeedAsync(auth.AccessJwt, auth.Did, limit, cursor).Result;
            }
            catch(Exception e)
            {
                throw new BskyException(e.Message);
            }
        }

        /// <summary>
        /// Retrieves the thread of a post asynchronously.
        /// </summary>
        /// <param name="postUri">The URI of the post.</param>
        /// <param name="depth">The depth of the thread to retrieve. Default is 3.</param>
        /// <param name="auth">The authentication response containing the user's tokens. Default is null, in this case the 
        /// BskyAuthUser property of the client will be used</param>
        /// <returns>A <see cref="BskyThread"/> object containing the post thread.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the limit is negative.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the post uri is null.</exception>
        /// <exception cref="BskyException">Thrown when an error occur during the interaction with BlueSky service or the user in not authenticated.</exception>
        public BskyThread GetPostThread(string postUri, int depth = 3, BskyAuthUser auth = null)
        {
            if (auth == null)
                auth = AuthUser;

            if (auth == null || !auth.IsAuthenticated())
            {
                throw new BskyException(ErrorMessage.USER_NOT_AUTHENTICATED);
            }
            if (string.IsNullOrEmpty(postUri))
            {
                throw new ArgumentNullException(nameof(postUri), ErrorMessage.ARG_IS_NULL);
            }
            if (depth <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(depth), ErrorMessage.ARG_IS_NEGATIVE);
            }

            try
            {
                return _postService.GetPostThreadAsync(auth.AccessJwt, postUri, depth).Result;
            }
            catch (Exception e)
            {
                throw new BskyException(e.Message);
            }
        }

        /// <summary>
        /// Unfollows a user asynchronously.
        /// </summary>
        /// <param name="followee">The user to be unfollowed.</param>
        /// <param name="auth">The authentication response containing the user's tokens. Default is null, in this case the 
        /// BskyAuthUser property of the client will be used</param>
        /// <returns>An <see cref="BskyActionResponse"/> indicating success or failure.</returns>
        /// <remarks>I hope you are doing it for @pykpyky</remarks>
        /// <exception cref="ArgumentNullException">Thrown when the user where you want perform the action is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the user where you want perform have mandatories fields nul as Viewer or Did.</exception>
        /// <exception cref="BskyException">Thrown when an error occur during the interaction with BlueSky service or the user in not authenticated.</exception>

        public BskyActionResponse Follow(User followee, BskyAuthUser auth = null)
        {
            if (auth == null)
                auth = AuthUser;

            BskyActionResponse actionResponse = userActionVerification(auth, followee);

            if(actionResponse == null)
            {
                if (followee.Viewer.Following != null)
                {
                    return new BskyActionResponse { ErrorMessage = ErrorMessage.USER_ALREADY_FOLLOWED };
                }
                try
                {
                    return _accountService.FollowAsync(auth.AccessJwt, auth.Did, followee.Did).Result;
                }
                catch (Exception ex)
                {
                    throw new BskyException(ex.Message);
                }
            }

            return actionResponse;
        }

        /// <summary>
        /// Unfollows a user asynchronously.
        /// </summary>
        /// <param name="followee">The user to be unfollowed.</param>
        /// <param name="auth">The authentication response containing the user's tokens. Default is null, in this case the 
        /// BskyAuthUser property of the client will be used</param>
        /// <returns>An <see cref="BskyActionResponse"/> indicating success or failure.</returns>
        /// <remarks>Could you not check who you followed first ?</remarks>
        /// <exception cref="ArgumentNullException">Thrown when the user where you want perform the action is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the user where you want perform have mandatories fields nul as Viewer or Did.</exception>
        /// <exception cref="BskyException">Thrown when an error occur during the interaction with BlueSky service or the user in not authenticated.</exception>
        public BskyActionResponse Unfollow(User followee, BskyAuthUser auth = null)
        {
            if (auth == null)
                auth = AuthUser;

            BskyActionResponse actionResponse = userActionVerification(auth, followee);
            if(actionResponse == null)
            {
                if (string.IsNullOrEmpty(followee.Viewer.Following))
                {
                    return new BskyActionResponse { ErrorMessage = ErrorMessage.USER_NOT_FOLLOWED };
                }
                try
                {
                    string followId = followee.Viewer.Following.Split("/").Last();
                    return _accountService.UnfollowAsync(auth.AccessJwt, auth.Did, followId).Result;
                }
                catch (Exception ex)
                {
                    throw new BskyException(ex.Message);
                }
            }

            return actionResponse;
        }

        /// <summary>
        /// Unlikes a post asynchronously.
        /// </summary>
        /// <param name="post">The post to be unliked.</param>
        /// <param name="auth">The authentication response containing the user's tokens. Default is null, in this case the 
        /// BskyAuthUser property of the client will be used</param>
        /// <returns>An <see cref="BskyActionResponse"/> indicating success or failure.</returns>
        /// <remarks>It's a nice action</remarks>
        /// <exception cref="ArgumentNullException">Thrown when the post where you want perform the action is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the post where you want perform have mandatories fields nul as Viewer or Cid.</exception>
        /// <exception cref="BskyException">Thrown when an error occur during the interaction with BlueSky service or the user in not authenticated.</exception>
        public BskyActionResponse LikePost(Post post, BskyAuthUser auth = null)
        {
            if (auth == null)
                auth = AuthUser;

            BskyActionResponse actionResponse = likeOrUnlikeVerification(auth, post);
            if (actionResponse == null)
            {
                if (post.Viewer.Like != null)
                {
                    return new BskyActionResponse { ErrorMessage = ErrorMessage.POST_ALREADY_LIKED };
                }
                try
                {
                    return _postService.LikePostAsync(auth.AccessJwt, auth.Did, post.Uri, post.Cid).Result;
                }
                catch (Exception ex)
                {
                    throw new BskyException(ex.Message);
                }
            }

            return actionResponse
;
        }

        /// <summary>
        /// Unlikes a post.
        /// </summary>
        /// <param name="post">The post to be unliked.</param>
        /// <param name="auth">The authentication response containing the user's tokens. Default is null, in this case the 
        /// BskyAuthUser property of the client will be used</param>
        /// <returns>An <see cref="BskyActionResponse"/> indicating success or failure.</returns>
        /// <remarks>Why did you like first?</remarks>
        /// <exception cref="ArgumentNullException">Thrown when the post where you want perform the action is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the post where you want perform have mandatories fields nul as Viewer or Cid.</exception>
        /// <exception cref="BskyException">Thrown when an error occur during the interaction with BlueSky service or the user in not authenticated.</exception>
        public BskyActionResponse UnlikePost(Post post, BskyAuthUser auth = null)
        {
            if (auth == null)
                auth = AuthUser;

            BskyActionResponse actionResponse = likeOrUnlikeVerification(auth, post);
            if (actionResponse == null)
            {
                if (post.Viewer.Like == null)
                {
                    return new BskyActionResponse { ErrorMessage = ErrorMessage.POST_NOT_LIKED };

                }
                try
                {
                    string likeId = post.Viewer.Like.Split("/").Last();
                    return _postService.UnlikePostAsync(auth.AccessJwt, auth.Did, likeId).Result;
                }
                catch (Exception ex)
                {
                    throw new BskyException(ex.Message);
                }
            }
            return actionResponse;
        }

        /// <summary>
        /// Mute a user.
        /// </summary>
        /// <param name="user">The user to be muted.</param>
        /// <param name="auth">The authentication response containing the user's tokens. Default is null, in this case the 
        /// BskyAuthUser property of the client will be used</param>
        /// <returns>An <see cref="BskyActionResponse"/> indicating success or failure.</returns>
        /// <remarks>You can still block the user later if mute is not enough</remarks>
        /// <exception cref="ArgumentNullException">Thrown when the user where you want perform the action is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the user where you want perform have mandatories fields nul as Viewer or Did.</exception>
        /// <exception cref="BskyException">Thrown when an error occur during the interaction with BlueSky service or the user in not authenticated.</exception>
        public BskyActionResponse MuteUser(User user, BskyAuthUser? auth = null)
        {
            if (auth == null)
                auth = AuthUser;

            BskyActionResponse actionResponse = userActionVerification(auth, user);

            if (actionResponse == null)
            {
                if (user.Viewer.Muted == true)
                {
                    return new BskyActionResponse { ErrorMessage = ErrorMessage.USER_ALREADY_MUTED };
                }
                try
                {
                    return _accountService.MuteUserAsync(auth.AccessJwt, auth.Did, user.Did).Result;
                }
                catch (Exception ex)
                {
                    throw new BskyException(ex.Message);
                }
            }

            return actionResponse;
        }

        /// <summary>
        /// Unmute a user.
        /// </summary>
        /// <param name="user">The user to be unmuted.</param>
        /// <param name="auth">The authentication response containing the user's tokens. Default is null, in this case the 
        /// BskyAuthUser property of the client will be used</param>
        /// <returns>An <see cref="BskyActionResponse"/> indicating success or failure.</returns>
        /// <remarks>We will not juge you</remarks>
        /// <exception cref="ArgumentNullException">Thrown when the user where you want perform the action is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the user where you want perform have mandatories fields nul as Viewer or Did.</exception>
        /// <exception cref="BskyException">Thrown when an error occur during the interaction with BlueSky service or the user in not authenticated.</exception>
        public BskyActionResponse UnMuteUser(User user, BskyAuthUser? auth = null)
        {
            if (auth == null)
                auth = AuthUser;

            BskyActionResponse actionResponse = userActionVerification(auth, user);

            if (actionResponse == null)
            {
                if (user.Viewer.Muted == false)
                {
                    return new BskyActionResponse { ErrorMessage = ErrorMessage.USER_NOT_MUTED };
                }
                try
                {
                    return _accountService.UnMuteUserAsync(auth.AccessJwt, auth.Did, user.Did).Result;
                }
                catch (Exception ex)
                {
                    throw new BskyException(ex.Message);
                }
            }

            return actionResponse;
        }

        /// <summary>
        /// Unmute a user.
        /// </summary>
        /// <param name="user">The user to get.</param>
        /// <param name="auth">The authentication response containing the user's tokens. Default is null, in this case the 
        /// BskyAuthUser property of the client will be used</param>
        /// <returns>An <see cref="UserProfileResponse"/> The user profile requested.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the user where you want perform the action is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the user where you want perform have mandatories fields nul as Viewer or Did.</exception>
        /// <exception cref="BskyException">Thrown when an error occur during the interaction with BlueSky service or the user in not authenticated.</exception>
        public User GetUserProfil(string userDid, BskyAuthUser? auth=null)
        { 
            if (auth == null)
                auth = AuthUser;

            if (AuthUser == null || !AuthUser.IsAuthenticated())
            {
                throw new BskyException(ErrorMessage.USER_NOT_AUTHENTICATED);
            }
            try
            {
                UserProfileResponse userProfileResponse = _accountService.GetUserProfileAysnc(AuthUser.AccessJwt, userDid).Result;
                if(userProfileResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return userProfileResponse.BskyUser;
                }
                else
                {
                    throw new BskyException(userProfileResponse.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new BskyException(ex.Message);
            }
        }

        /// <summary>
        /// Verifies if the user can like or unlike a post.
        /// </summary>
        /// <param name="auth">The authenticated user.</param>
        /// <param name="post">The post to be verified.</param>
        /// <returns>An <see cref="BskyActionResponse"/> indicating verification status or null if valid.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the post where you want perform the action is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the post where you want perform have mandatories fields nul as Viewer or Cid.</exception>
        /// <exception cref="BskyException">Thrown when the user in not authenticated.</exception>

        private BskyActionResponse? likeOrUnlikeVerification(BskyAuthUser auth, Post post)
        {
            if (auth == null || !auth.IsAuthenticated())
            {
                throw new BskyException(ErrorMessage.USER_NOT_AUTHENTICATED);
            }
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post), ErrorMessage.ARG_IS_NULL);
            }
            if (post.Viewer == null)
            {
                throw new ArgumentException(nameof(post), ErrorMessage.ARG_IS_NULL);
            }
            if (string.IsNullOrEmpty(post.Uri) || string.IsNullOrEmpty(post.Cid))
            {
                throw new ArgumentException(nameof(post), ErrorMessage.ARG_IS_NULL);
            }

            return null;
        }

        /// <summary>
        /// Verifies if the user can interact with another user.
        /// </summary>
        /// <param name="authUser">The authenticated user.</param>
        /// <param name="user">The user whith you want interact.</param>
        /// <returns>An <see cref="BskyActionResponse"/> indicating verification status or null if valid.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the user where you want perform the action is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the user where you want perform have mandatories fields nul as Viewer or Did.</exception>
        /// <exception cref="BskyException">Thrown when the user in not authenticated.</exception>
        private BskyActionResponse? userActionVerification(BskyAuthUser authUser, User user)
        {
            if (authUser == null || !authUser.IsAuthenticated())
            {
                throw new BskyException(ErrorMessage.USER_NOT_AUTHENTICATED);
            }
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), ErrorMessage.ARG_IS_NULL);
            }
            if (user.Viewer == null)
            {
                throw new ArgumentException(nameof(user), ErrorMessage.ARG_IS_NULL);
            }
            if (string.IsNullOrEmpty(user.Did))
            {
                throw new ArgumentException(nameof(user), ErrorMessage.ARG_IS_NULL);
            }
            if (authUser.Did.Equals(user.Did))
            {
                return new BskyActionResponse { ErrorMessage = ErrorMessage.SAME_DID_USER };
            }

            return null;
        }


    }
}
