using ATPandaSDK.Models.Auth;
using ATPandaSDK;
using Moq.Protected;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using ATbluePandaSDK.Models;
using System.Runtime.Intrinsics.Arm;
using ATbluePandaSDK;
using ATPandaSDK.Models.Feed;

namespace TestATbluePandaSDK
{
    public class PostTest
    {
        [Fact]
        public void CreatePostSuccess()
        {
            // Arrange
            BskyAuthUser authUser = Utils.GetAuthUser();
            BskyActionResponse fakeAction = Utils.GetActionResponse();

            var fakeActionJSON = JsonSerializer.Serialize(fakeAction);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Post, fakeActionJSON);
            var client = new ATPClient(authUser, httpClient);

            //Act
            BskyActionResponse actionResponse = client.CreatePost("Create post test");


            //Verify
            Assert.NotNull(actionResponse);
            Assert.NotEmpty(actionResponse.Cid);
            Assert.NotEmpty(actionResponse.Uri);
            Assert.NotEmpty(actionResponse.ValidationStatus);
            
            Assert.NotNull(actionResponse.Commit);
            Assert.NotEmpty(actionResponse.Commit.cid);
            Assert.NotEmpty(actionResponse.Commit.rev);
            Assert.Null(actionResponse.ErrorMessage);
        }

        [Fact]
        public void CreatePostFail()
        {
            // Arrange
            BskyAuthUser authUser = Utils.GetAuthUser();

            BskyActionResponse fakeAction = Utils.GetActionResponseError();

            var fakeActionJSON = JsonSerializer.Serialize(fakeAction);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.BadRequest, HttpMethod.Post, fakeActionJSON);
            var client = new ATPClient(authUser, httpClient);

            //Act
            BskyActionResponse actionResponse = client.CreatePost("Create post test");


            //Verify
            Assert.NotNull(actionResponse);
            Assert.Null(actionResponse.Cid);
            Assert.Null(actionResponse.Uri);
            Assert.Null(actionResponse.ValidationStatus);

            Assert.Null(actionResponse.Commit);
            Assert.NotEmpty(actionResponse.ErrorMessage);
        }

        [Fact]
        public void CreatePostWrongArgumentFail()
        {
            // Arrange
            BskyAuthUser authUser = Utils.GetAuthUser();

            //Act
            var client = new ATPClient();

            BskyException actionResponseAuthNull = Assert.Throws<BskyException>(() => client.CreatePost("hello", null));
            ArgumentNullException actionResponseTextEmpty = Assert.Throws<ArgumentNullException>(() => client.CreatePost("", authUser));
            ArgumentNullException actionResponseTextNull = Assert.Throws<ArgumentNullException>(() => client.CreatePost(null, authUser));

            //Verify
            Assert.NotNull(actionResponseAuthNull);
            Assert.NotEmpty(actionResponseAuthNull.Message);
            Assert.Contains(ErrorMessage.USER_NOT_AUTHENTICATED, actionResponseAuthNull.Message);

            Assert.NotNull(actionResponseTextEmpty);
            Assert.NotEmpty(actionResponseTextEmpty.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, actionResponseTextEmpty.Message);

            Assert.NotNull(actionResponseTextNull);
            Assert.NotEmpty(actionResponseTextNull.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, actionResponseTextEmpty.Message);

        }

        [Fact]
        public void LikePostSuccess()
        {
            // Arrange
            BskyAuthUser authUser = Utils.GetAuthUser();
            BskyActionResponse fakeAction = Utils.GetActionResponse();
            Post post = Utils.GetPost();

            var fakeAcionJSON = JsonSerializer.Serialize(fakeAction);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Post, fakeAcionJSON);
            var client = new ATPClient(authUser, httpClient);

            //Act
            BskyActionResponse actionResponse = client.LikePost(post, authUser);


            //Verify
            Assert.NotNull(actionResponse);
            Assert.NotEmpty(actionResponse.Cid);
            Assert.NotEmpty(actionResponse.Uri);
            Assert.NotEmpty(actionResponse.ValidationStatus);

            Assert.NotNull(actionResponse.Commit);
            Assert.NotEmpty(actionResponse.Commit.cid);
            Assert.NotEmpty(actionResponse.Commit.rev);
            Assert.Null(actionResponse.ErrorMessage);
        }

        [Fact]
        public void LikePostFail()
        {
            // Arrange
            BskyAuthUser authUser = Utils.GetAuthUser();
            Post post = Utils.GetPost();

            BskyActionResponse fakeAction = Utils.GetActionResponseError();
            var fakeAcionJSON = JsonSerializer.Serialize(fakeAction);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.BadRequest, HttpMethod.Post, fakeAcionJSON);
            var client = new ATPClient(authUser, httpClient);

            //Act
            BskyActionResponse actionResponse = client.LikePost(post);


            //Verify
            Assert.NotNull(actionResponse);
            Assert.Null(actionResponse.Cid);
            Assert.Null(actionResponse.Uri);
            Assert.Null(actionResponse.ValidationStatus);
            Assert.Null(actionResponse.Commit);
            Assert.NotEmpty(actionResponse.ErrorMessage);

        }

        [Fact]
        public void LikePostWrongArgumentFail()
        {
            // Arrange
            BskyAuthUser authUser = Utils.GetAuthUser();

            Viewer viewer = new Viewer
            {
                Like = "ded//dedwdw/dewdwd/dweretertg"
            };

            Post postLiked = Utils.GetPost();
            postLiked.Viewer = viewer;

            Post postStringNull = new Post
            {
                Viewer = viewer
            };

            Post postEmpty = new Post();

            //Act
            var client = new ATPClient();

            BskyException ActionResponseAuthNull = Assert.Throws<BskyException>(() => client.LikePost(postLiked, null));
            BskyActionResponse ActionResponsePostLiked = client.LikePost(postLiked, authUser);
            ArgumentNullException ActionResponsePostNull = Assert.Throws<ArgumentNullException>(() => client.LikePost(null, authUser));
            ArgumentException ActionResponsePostStringNull = Assert.Throws<ArgumentException>(() => client.LikePost(postStringNull, authUser));
            ArgumentException ActionResponsePostEmpty = Assert.Throws<ArgumentException>(() => client.LikePost(postEmpty, authUser));




            //Verify
            Assert.NotNull(ActionResponseAuthNull);
            Assert.NotEmpty(ActionResponseAuthNull.Message);
            Assert.Contains(ErrorMessage.USER_NOT_AUTHENTICATED, ActionResponseAuthNull.Message);

            Assert.NotNull(ActionResponsePostLiked);
            Assert.Null(ActionResponsePostLiked.Cid);
            Assert.Null(ActionResponsePostLiked.Uri);
            Assert.Null(ActionResponsePostLiked.ValidationStatus);
            Assert.Null(ActionResponsePostLiked.Commit);
            Assert.NotEmpty(ActionResponsePostLiked.ErrorMessage);
            Assert.Equal(ErrorMessage.POST_ALREADY_LIKED, ActionResponsePostLiked.ErrorMessage);

            Assert.NotNull(ActionResponsePostNull);
            Assert.NotEmpty(ActionResponsePostNull.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, ActionResponsePostNull.Message);

            Assert.NotNull(ActionResponsePostStringNull);
            Assert.NotEmpty(ActionResponsePostStringNull.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, ActionResponsePostStringNull.Message);

            Assert.NotNull(ActionResponsePostEmpty);
            Assert.NotEmpty(ActionResponsePostEmpty.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, ActionResponsePostEmpty.Message);

        }

        [Fact]
        public void UnlikePostSuccess()
        {
            // Arrange
            BskyAuthUser authUser = Utils.GetAuthUser();

            BskyActionResponse fakeAction = Utils.GetActionResponse();

            Viewer viewer = new Viewer
            {
                Like = "ded//dedwdw/dewdwd/dweretertg"
            };

            Post post = Utils.GetPost();
            post.Viewer = viewer;

            var fakeAcionJSON = JsonSerializer.Serialize(fakeAction);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Post, fakeAcionJSON);
            var client = new ATPClient(authUser, httpClient);

            //Act
            BskyActionResponse actionResponse = client.UnlikePost(post);


            //Verify
            Assert.NotNull(actionResponse);
            Assert.NotEmpty(actionResponse.Cid);
            Assert.NotEmpty(actionResponse.Uri);
            Assert.NotEmpty(actionResponse.ValidationStatus);

            Assert.NotNull(actionResponse.Commit);
            Assert.NotEmpty(actionResponse.Commit.cid);
            Assert.NotEmpty(actionResponse.Commit.rev);
            Assert.Null(actionResponse.ErrorMessage);
        }

        [Fact]
        public void UnlikePostFail()
        {
            // Arrange
            BskyAuthUser authUser = Utils.GetAuthUser();

            Viewer viewer = new Viewer
            {
                Like = "ded//dedwdw/dewdwd/dweretertg"
            };

            Post post = Utils.GetPost();
            post.Viewer = viewer;

            BskyActionResponse fakeAction = Utils.GetActionResponseError();

            var fakeAcionJSON = JsonSerializer.Serialize(fakeAction);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.BadRequest, HttpMethod.Post, fakeAcionJSON);
            var client = new ATPClient(authUser, httpClient);

            //Act
            BskyActionResponse actionResponse = client.LikePost(post);


            //Verify
            Assert.NotNull(actionResponse);
            Assert.Null(actionResponse.Cid);
            Assert.Null(actionResponse.Uri);
            Assert.Null(actionResponse.ValidationStatus);
            Assert.Null(actionResponse.Commit);
            Assert.NotEmpty(actionResponse.ErrorMessage);

        }

        [Fact]
        public void UnlikePostWrongArgumentFail()
        {
            // Arrange
            BskyAuthUser authUser = Utils.GetAuthUser();
            Post postNotLiked = Utils.GetPost();
            Post postStringNull = new Post
            {
                Viewer = new Viewer()
            };
            Post postEmpty = new Post();

            //Act
            var client = new ATPClient();

            BskyException ActionResponseAuthNull = Assert.Throws<BskyException>(() => client.UnlikePost(postNotLiked, null));
            BskyActionResponse ActionResponsePostLiked = client.UnlikePost(postNotLiked, authUser);
            ArgumentNullException ActionResponsePostNull = Assert.Throws<ArgumentNullException>(() => client.UnlikePost(null, authUser));
            ArgumentException ActionResponsePostStringNull = Assert.Throws<ArgumentException>(() => client.UnlikePost(postStringNull, authUser));
            ArgumentException ActionResponsePostEmpty = Assert.Throws<ArgumentException>(() => client.UnlikePost(postEmpty, authUser));

            //Verify
            Assert.NotNull(ActionResponseAuthNull);
            Assert.NotEmpty(ActionResponseAuthNull.Message);
            Assert.Contains(ErrorMessage.USER_NOT_AUTHENTICATED, ActionResponseAuthNull.Message);

            Assert.NotNull(ActionResponsePostLiked);
            Assert.Null(ActionResponsePostLiked.Cid);
            Assert.Null(ActionResponsePostLiked.Uri);
            Assert.Null(ActionResponsePostLiked.ValidationStatus);
            Assert.Null(ActionResponsePostLiked.Commit);
            Assert.NotEmpty(ActionResponsePostLiked.ErrorMessage);
            Assert.Equal(ErrorMessage.POST_NOT_LIKED, ActionResponsePostLiked.ErrorMessage);

            Assert.NotNull(ActionResponsePostNull);
            Assert.NotEmpty(ActionResponsePostNull.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, ActionResponsePostNull.Message);

            Assert.NotNull(ActionResponsePostStringNull);
            Assert.NotEmpty(ActionResponsePostStringNull.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, ActionResponsePostStringNull.Message);

            Assert.NotNull(ActionResponsePostEmpty);
            Assert.NotEmpty(ActionResponsePostEmpty.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, ActionResponsePostEmpty.Message);

        }

        [Fact]
        public void ThreadSuccess()
        {
            BskyAuthUser authUser = Utils.GetAuthUser();


            Author author = Utils.GetAuthor();
            Post post = Utils.GetPost();
            Reply reply = new Reply
            {
                post = Utils.GetPost(),
            };

            List<Reply> replies = new List<Reply>();
            replies.Add(reply);


            ThreadPost thread = new ThreadPost
            {
                post = post,
                replies = replies,
                threadContext = new ThreadContext(),
                type = "app.bsky.feed.defs#threadViewPost"
            };

            BskyThread threadResponse = new BskyThread
            {
                thread = thread
            };

            var fakeAcionJSON = JsonSerializer.Serialize(threadResponse);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Get, fakeAcionJSON);
            var client = new ATPClient(authUser, httpClient);

            //Act
            BskyThread PostResult = client.GetPostThread("uri//dijdsiduoifjufhjeujfef");


            //Verify
            Assert.NotNull(PostResult);
            Assert.NotNull(PostResult.thread);
            Assert.NotNull(PostResult.thread.post);
            Assert.NotEmpty(PostResult.thread.type);
            Assert.NotNull(PostResult.thread.replies);
            Assert.Null(PostResult.ErrorMessage);

        }

        [Fact]
        public void ThreadFaill()
        {
            BskyAuthUser authUser = Utils.GetAuthUser();

            string errorMessage = "RANDOM ERROR";
            
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.BadRequest, HttpMethod.Get, errorMessage);
            var client = new ATPClient(authUser, httpClient);

            //Act
            BskyThread PostResult = client.GetPostThread("uri//dijdsiduoifjufhjeujfef");

            //Verify
            Assert.NotNull(PostResult);
            Assert.Null(PostResult.thread);
            Assert.NotEmpty(PostResult.ErrorMessage);

        }

        [Fact]
        public void ThreadArgumentFail()
        {
            BskyAuthUser authUser = Utils.GetAuthUser();
            var client = new ATPClient();

            //Act
            BskyException PostResultNotAuth = Assert.Throws<BskyException>(() => client.GetPostThread("uri//dijdsiduoifjufhjeujfef", auth:null));
            ArgumentNullException PostResultUriEmpy = Assert.Throws<ArgumentNullException>(() => client.GetPostThread("", auth:authUser));
            ArgumentOutOfRangeException PostResultWrongLimit = Assert.Throws<ArgumentOutOfRangeException>(() => client.GetPostThread("uri//dijdsiduoifjufhjeujfef", -3, auth:authUser));


            //Verify
            Assert.NotNull(PostResultNotAuth);
            Assert.NotEmpty(PostResultNotAuth.Message);
            Assert.Contains(ErrorMessage.USER_NOT_AUTHENTICATED, PostResultNotAuth.Message);

            Assert.NotNull(PostResultUriEmpy);
            Assert.NotEmpty(PostResultUriEmpy.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, PostResultUriEmpy.Message);

            Assert.NotNull(PostResultWrongLimit);
            Assert.NotEmpty(PostResultWrongLimit.Message);
            Assert.Contains(ErrorMessage.LIMIT_NOT_SUPPORTED, PostResultWrongLimit.Message);

        }

    }

}
