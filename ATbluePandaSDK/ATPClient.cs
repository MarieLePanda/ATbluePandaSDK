

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
namespace ATPandaSDK
{
    public class ATPClient
    {
        private readonly AuthService _authService;
        private readonly PostService _postService;
        private readonly FeedService _feedService;
        private readonly AccountService _accountService;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ATPClient"/> class with default service implementations.
        /// </summary>
        public ATPClient()
        {
            HttpClient httpClient = new HttpClient { BaseAddress = new Uri(Configuration.BaseUrl) };
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                //builder.AddConsole();  // Log dans la console
                //builder.SetMinimumLevel(LogLevel.Debug);
            });

            _logger = loggerFactory.CreateLogger<ATPClient>();
            _authService = new AuthService(httpClient, _logger);
            _postService = new PostService(httpClient, _logger);
            _feedService = new FeedService(httpClient, _logger);
            _accountService = new AccountService(httpClient, _logger);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ATPClient"/> class with default service implementations.
        /// </summary>
        /// <param name="httpClient">An instance of <see cref="HttpClient"/> to use.</param>
        public ATPClient(HttpClient httpClient)
        {
            HttpClient _httpClient = httpClient;
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();  // Log dans la console
                builder.SetMinimumLevel(LogLevel.Debug);
            });

            _logger = loggerFactory.CreateLogger<ATPClient>();
            _logger.LogInformation("Test");
            _authService = new AuthService(httpClient, _logger);
            _postService = new PostService(httpClient, _logger);
            _feedService = new FeedService(httpClient, _logger);
            _accountService = new AccountService(httpClient, _logger);
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
        /// <returns>A <see cref="ActionResponse"/> object containing the result of the post creation.</returns>
        public AuthUser Authenticate(AuthRequest authRequest)
        {
            if (authRequest == null)
            {
                return new AuthUser { ErrorMessage = ErrorMessage.AUTH_REQUEST_IS_NULL };
            }

            if (string.IsNullOrEmpty(authRequest.Username) || string.IsNullOrEmpty(authRequest.Password))
            {
                return new AuthUser { ErrorMessage = ErrorMessage.USER_NAME_OR_PASSWORD_IS_NULL };
            }
            try
            {
                return _authService.AuthenticateAsync(authRequest).Result;
            }
            catch (Exception e)
            {
                return new AuthUser { ErrorMessage = e.Message };
            }

        }

        /// <summary>
        /// Creates a post with the specified text.
        /// </summary>
        /// <param name="auth">The authentication response containing the user's tokens.</param>
        /// <param name="text">The text content of the post.</param>
        /// <returns>A <see cref="ActionResponse"/> object containing the result of the post creation.</returns>
        /// <remarks>Don't be mean with other when you post</remarks>
        public ActionResponse CreatePost(AuthUser auth, string text)
        {
            if (auth == null || !auth.IsAuthenticated())
            {
                return new ActionResponse { ErrorMessage = ErrorMessage.USER_NOT_AUTHENTICATED };
            }
            if (string.IsNullOrWhiteSpace(text))
            {
                return new ActionResponse { ErrorMessage = ErrorMessage.TEXT_IS_NULL };
            }
            try
            {
                return _postService.CreatePostAsync(auth.AccessJwt, auth.Did, text).Result;
            }
            catch (Exception e)
            {
                return new ActionResponse { ErrorMessage = e.Message };

            }
        }

        /// <summary>
        /// Retrieves the user's timeline asynchronously.
        /// </summary>
        /// <param name="auth">The authentication response containing the user's tokens.</param>
        /// <param name="limit">The maximum number of posts to retrieve. Default is 30.</param>
        /// <param name="cursor">The cursor for pagination. Default is null.</param>
        /// <returns>A <see cref="TimelineResponse"/> object containing the user's timeline.</returns
        public TimelineResponse GetTimeline(AuthUser auth, int limit = 30, string cursor = null)
        {
            if (auth == null || !auth.IsAuthenticated())
            {
                return new TimelineResponse { ErrorMessage = ErrorMessage.USER_NOT_AUTHENTICATED };
            }
            if (limit <= 0)
            {
                return new TimelineResponse { ErrorMessage = ErrorMessage.LIMIT_IS_NEGATIVE };
            }
            try
            {
                return _feedService.GetTimelineAsync(auth.AccessJwt, limit, cursor).Result;
            }
            catch(Exception e)
            {
                return new TimelineResponse { ErrorMessage = e.Message };
            }
        }

        /// <summary>
        /// Retrieves a custom timeline asynchronously.
        /// </summary>
        /// <param name="auth">The authentication response containing the user's tokens.</param>
        /// <param name="feed">The feed identifier.</param>
        /// <param name="limit">The maximum number of posts to retrieve. Default is 30.</param>
        /// <param name="cursor">The cursor for pagination. Default is null.</param>
        /// <returns>A <see cref="TimelineResponse"/> object containing the custom timeline.</returns>
        public TimelineResponse GetCustomTimeline(AuthUser auth, string feed, int limit = 30, string cursor = null)
        {
            if (auth == null || !auth.IsAuthenticated())
            {
                return new TimelineResponse { ErrorMessage = ErrorMessage.USER_NOT_AUTHENTICATED };
            }
            if(string.IsNullOrWhiteSpace(feed))
            {
                return new TimelineResponse { ErrorMessage = ErrorMessage.FEED_IS_NULL };
            }
            if(limit <= 0)
            {
                return new TimelineResponse { ErrorMessage = ErrorMessage.LIMIT_IS_NEGATIVE };
            }
            try
            {
                return _feedService.GetFeedGeneratorAsync(auth.AccessJwt, feed, limit, cursor).Result;
            }
            catch (Exception e)
            {
                return new TimelineResponse { ErrorMessage = e.Message };
            }
        }

        /// <summary>
        /// Retrieves the author's timeline asynchronously.
        /// </summary>
        /// <param name="auth">The authentication response containing the user's tokens.</param>
        /// <param name="limit">The maximum number of posts to retrieve. Default is 30.</param>
        /// <param name="cursor">The cursor for pagination. Default is null.</param>
        /// <returns>A <see cref="TimelineResponse"/> object containing the author's timeline.</returns>
        public TimelineResponse GetAuthorTimeline(AuthUser auth, int limit = 30, string cursor = null)
        {
            if (auth == null || !auth.IsAuthenticated())
            {
                return new TimelineResponse { ErrorMessage = ErrorMessage.USER_NOT_AUTHENTICATED };
            }
            if (limit <= 0)
            {
                return new TimelineResponse { ErrorMessage = ErrorMessage.LIMIT_IS_NEGATIVE };
            }

            try
            {
                return _feedService.GetAuthorFeedAsync(auth.AccessJwt, auth.Did, limit, cursor).Result;
            }
            catch(Exception e)
            {
                return new TimelineResponse { ErrorMessage = e.Message };
            }
        }

        /// <summary>
        /// Retrieves the thread of a post asynchronously.
        /// </summary>
        /// <param name="auth">The authentication response containing the user's tokens.</param>
        /// <param name="postUri">The URI of the post.</param>
        /// <param name="depth">The depth of the thread to retrieve. Default is 3.</param>
        /// <returns>A <see cref="ThreadResponse"/> object containing the post thread.</returns>
        public ThreadResponse GetPostThread(AuthUser auth, string postUri, int depth = 3)
        {
            if (auth == null || !auth.IsAuthenticated())
            {
                return new ThreadResponse { ErrorMessage = ErrorMessage.USER_NOT_AUTHENTICATED };
            }
            if(string.IsNullOrEmpty(postUri))
            {
                return new ThreadResponse { ErrorMessage = ErrorMessage.POST_URI_IS_NULL };
            }
            if (depth <= 0)
            {
                return new ThreadResponse { ErrorMessage = ErrorMessage.LIMIT_IS_NEGATIVE };
            }

            try
            {
                return _postService.GetPostThreadAsync(auth.AccessJwt, postUri, depth).Result;
            }
            catch (Exception ex)
            {
                return new ThreadResponse { ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// Unfollows a user asynchronously.
        /// </summary>
        /// <param name="authUser">The authenticated user attempting to unfollow.</param>
        /// <param name="followee">The user to be unfollowed.</param>
        /// <returns>An <see cref="ActionResponse"/> indicating success or failure.</returns>
        /// <remarks>I hope yo uare doing it for @pykpyky</remarks>
        public ActionResponse Follow(AuthUser authUser, User followee)
        {
            ActionResponse actionResponse = userActionVerification(authUser, followee);

            if(actionResponse == null)
            {
                if (followee.Viewer.Following != null)
                {
                    return new ActionResponse { ErrorMessage = ErrorMessage.USER_ALREADY_FOLLOWED };
                }
                try
                {
                    return _accountService.FollowAsync(authUser.AccessJwt, authUser.Did, followee.Did).Result;
                }
                catch (Exception ex)
                {
                    return new ActionResponse { ErrorMessage = ex.Message };
                }
            }

            return actionResponse;
        }

        /// <summary>
        /// Unfollows a user asynchronously.
        /// </summary>
        /// <param name="authUser">The authenticated user attempting to unfollow.</param>
        /// <param name="followee">The user to be unfollowed.</param>
        /// <returns>An <see cref="ActionResponse"/> indicating success or failure.</returns>
        /// <remarks>Could you not check who you followed first ?</remarks>
        public ActionResponse Unfollow(AuthUser authUser, User followee)
        {
            ActionResponse actionResponse = userActionVerification(authUser, followee);
            if(actionResponse == null)
            {
                if (string.IsNullOrEmpty(followee.Viewer.Following))
                {
                    return new ActionResponse { ErrorMessage = ErrorMessage.USER_NOT_FOLLOWED };
                }
                try
                {
                    string followId = followee.Viewer.Following.Split("/").Last();
                    return _accountService.UnfollowAsync(authUser.AccessJwt, authUser.Did, followId).Result;
                }
                catch (Exception ex)
                {
                    return new ActionResponse { ErrorMessage = ex.Message };
                }
            }

            return actionResponse;
        }

        /// <summary>
        /// Unlikes a post asynchronously.
        /// </summary>
        /// <param name="auth">The authenticated user attempting to unlike a post.</param>
        /// <param name="post">The post to be unliked.</param>
        /// <returns>An <see cref="ActionResponse"/> indicating success or failure.</returns>
        /// <remarks>It's a nice action</remarks>
        public ActionResponse LikePost(AuthUser auth, Post post)
        {
            ActionResponse actionResponse = likeOrUnlikeVerification(auth, post);
            if (actionResponse == null)
            {
                if (post.Viewer.Like != null)
                {
                    return new ActionResponse { ErrorMessage = ErrorMessage.POST_ALREADY_LIKED };
                }
                try
                {
                    return _postService.LikePostAsync(auth.AccessJwt, auth.Did, post.Uri, post.Cid).Result;
                }
                catch (Exception ex)
                {
                    return new ActionResponse { ErrorMessage = ex.Message };
                }
            }

            return actionResponse
;
        }

        /// <summary>
        /// Unlikes a post asynchronously.
        /// </summary>
        /// <param name="auth">The authenticated user attempting to unlike a post.</param>
        /// <param name="post">The post to be unliked.</param>
        /// <returns>An <see cref="ActionResponse"/> indicating success or failure.</returns>
        /// <remarks>Why did you like first?</remarks>
        public ActionResponse UnlikePost(AuthUser auth, Post post)
        {
            ActionResponse actionResponse = likeOrUnlikeVerification(auth, post);
            if (actionResponse == null)
            {
                if (post.Viewer.Like == null)
                {
                    return new ActionResponse { ErrorMessage = ErrorMessage.POST_NOT_LIKED };

                }
                try
                {
                    string likeId = post.Viewer.Like.Split("/").Last();
                    return _postService.UnlikePostAsync(auth.AccessJwt, auth.Did, likeId).Result;
                }
                catch (Exception ex)
                {
                    return new ActionResponse { ErrorMessage = ex.Message };
                }
            }
            return actionResponse;
        }

        public ActionResponse MuteUser(AuthUser authUser, User user)
        {
            ActionResponse actionResponse = userActionVerification(authUser, user);

            if (actionResponse == null)
            {
                if (user.Viewer.Muted == true)
                {
                    return new ActionResponse { ErrorMessage = ErrorMessage.USER_ALREADY_MUTED };
                }
                try
                {
                    return _accountService.MuteUser(authUser.AccessJwt, authUser.Did, user.Did).Result;
                }
                catch (Exception ex)
                {
                    return new ActionResponse { ErrorMessage = ex.Message };
                }
            }

            return actionResponse;
        }

        public ActionResponse UnMuteUser(AuthUser authUser, User user)
        {
            ActionResponse actionResponse = userActionVerification(authUser, user);

            if (actionResponse == null)
            {
                if (user.Viewer.Muted == false)
                {
                    return new ActionResponse { ErrorMessage = ErrorMessage.USER_NOT_MUTED };
                }
                try
                {
                    return _accountService.UnMuteUser(authUser.AccessJwt, authUser.Did, user.Did).Result;
                }
                catch (Exception ex)
                {
                    return new ActionResponse { ErrorMessage = ex.Message };
                }
            }

            return actionResponse;
        }
        /// <summary>
        /// Verifies if the user can like or unlike a post.
        /// </summary>
        /// <param name="auth">The authenticated user.</param>
        /// <param name="post">The post to be verified.</param>
        /// <returns>An <see cref="ActionResponse"/> indicating verification status or null if valid.</returns>
        private ActionResponse? likeOrUnlikeVerification(AuthUser auth, Post post)
        {
            if (auth == null || !auth.IsAuthenticated())
            {
                return new ActionResponse { ErrorMessage = ErrorMessage.USER_NOT_AUTHENTICATED };
            }
            if (post == null)
            {
                return new ActionResponse { ErrorMessage = ErrorMessage.POST_IS_NULL };
            }
            if (post.Viewer == null)
            {
                return new ActionResponse { ErrorMessage = ErrorMessage.VIEWER_IS_NULL };
            }
            if (string.IsNullOrEmpty(post.Uri) || string.IsNullOrEmpty(post.Cid))
            {
                return new ActionResponse { ErrorMessage =  ErrorMessage.POST_URI_OR_CID_IS_NULL};
            }

            return null;
        }

        /// <summary>
        /// Verifies if the user can interact with another user.
        /// </summary>
        /// <param name="authUser">The authenticated user.</param>
        /// <param name="user">The user whith you want interact.</param>
        /// <returns>An <see cref="ActionResponse"/> indicating verification status or null if valid.</returns>
        private ActionResponse? userActionVerification(AuthUser authUser, User user)
        {
            if (authUser == null || !authUser.IsAuthenticated())
            {
                return new ActionResponse { ErrorMessage = ErrorMessage.USER_NOT_AUTHENTICATED };
            }
            if (user == null)
            {
                return new ActionResponse { ErrorMessage = ErrorMessage.USER_IS_NULL };
            }
            if (user.Viewer == null)
            {
                return new ActionResponse { ErrorMessage = ErrorMessage.VIEWER_IS_NULL };
            }
            if (string.IsNullOrEmpty(user.Did))
            {
                return new ActionResponse { ErrorMessage = ErrorMessage.USER_DID_IS_NULL };
            }
            if (authUser.Did.Equals(user.Did))
            {
                return new ActionResponse { ErrorMessage = ErrorMessage.SAME_DID_USER };
            }

            return null;
        }


    }
}
