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
            AuthUser authUser = Utils.GetAuthUser();
            ActionResponse fakeAction = Utils.GetActionResponse();

            var fakeActionJSON = JsonSerializer.Serialize(fakeAction);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Post, fakeActionJSON);
            var client = new ATPClient(httpClient);

            //Act
            ActionResponse actionResponse = client.CreatePost(authUser, "Create post test");


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
            AuthUser authUser = Utils.GetAuthUser();

            ActionResponse fakeAction = Utils.GetActionResponseError();

            var fakeActionJSON = JsonSerializer.Serialize(fakeAction);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.BadRequest, HttpMethod.Post, fakeActionJSON);
            var client = new ATPClient(httpClient);

            //Act
            ActionResponse actionResponse = client.CreatePost(authUser, "Create post test");


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
            AuthUser authUser = Utils.GetAuthUser();

            //Act
            var client = new ATPClient();

            ActionResponse ActionResponseAuthNull = client.CreatePost(null, "hello");
            ActionResponse ActionResponseTextEmpty = client.CreatePost(authUser, "");
            ActionResponse ActionResponseTextNull = client.CreatePost(authUser, null);



            //Verify
            Assert.NotNull(ActionResponseAuthNull);
            Assert.Null(ActionResponseAuthNull.Cid);
            Assert.Null(ActionResponseAuthNull.Uri);
            Assert.Null(ActionResponseAuthNull.ValidationStatus);
            Assert.Null(ActionResponseAuthNull.Commit);
            Assert.NotEmpty(ActionResponseAuthNull.ErrorMessage);
            Assert.Equal(ErrorMessage.USER_NOT_AUTHENTICATED, ActionResponseAuthNull.ErrorMessage);

            Assert.NotNull(ActionResponseTextEmpty);
            Assert.Null(ActionResponseTextEmpty.Cid);
            Assert.Null(ActionResponseTextEmpty.Uri);
            Assert.Null(ActionResponseTextEmpty.ValidationStatus);
            Assert.Null(ActionResponseTextEmpty.Commit);
            Assert.NotEmpty(ActionResponseTextEmpty.ErrorMessage);
            Assert.Equal(ErrorMessage.TEXT_IS_NULL, ActionResponseTextEmpty.ErrorMessage);

            Assert.NotNull(ActionResponseTextNull);
            Assert.Null(ActionResponseTextNull.Cid);
            Assert.Null(ActionResponseTextNull.Uri);
            Assert.Null(ActionResponseTextNull.ValidationStatus);
            Assert.Null(ActionResponseTextNull.Commit);
            Assert.NotEmpty(ActionResponseTextNull.ErrorMessage);
            Assert.Equal(ErrorMessage.TEXT_IS_NULL, ActionResponseTextNull.ErrorMessage);

        }

        [Fact]
        public void LikePostSuccess()
        {
            // Arrange
            AuthUser authUser = Utils.GetAuthUser();
            ActionResponse fakeAction = Utils.GetActionResponse();
            Post post = Utils.GetPost();

            var fakeAcionJSON = JsonSerializer.Serialize(fakeAction);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Post, fakeAcionJSON);
            var client = new ATPClient(httpClient);

            //Act
            ActionResponse actionResponse = client.LikePost(authUser, post);


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
            AuthUser authUser = Utils.GetAuthUser();
            Post post = Utils.GetPost();

            ActionResponse fakeAction = Utils.GetActionResponseError();
            var fakeAcionJSON = JsonSerializer.Serialize(fakeAction);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.BadRequest, HttpMethod.Post, fakeAcionJSON);
            var client = new ATPClient(httpClient);

            //Act
            ActionResponse actionResponse = client.LikePost(authUser, post);


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
            AuthUser authUser = Utils.GetAuthUser();

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

            ActionResponse ActionResponseAuthNull = client.LikePost(null, postLiked);
            ActionResponse ActionResponsePostLiked = client.LikePost(authUser, postLiked);
            ActionResponse ActionResponsePostNull = client.LikePost(authUser, null);
            ActionResponse ActionResponsePostStringNull = client.LikePost(authUser, postStringNull);
            ActionResponse ActionResponsePostEmpty = client.LikePost(authUser, postEmpty);




            //Verify
            Assert.NotNull(ActionResponseAuthNull);
            Assert.Null(ActionResponseAuthNull.Cid);
            Assert.Null(ActionResponseAuthNull.Uri);
            Assert.Null(ActionResponseAuthNull.ValidationStatus);
            Assert.Null(ActionResponseAuthNull.Commit);
            Assert.NotEmpty(ActionResponseAuthNull.ErrorMessage);
            Assert.Equal(ErrorMessage.USER_NOT_AUTHENTICATED, ActionResponseAuthNull.ErrorMessage);

            Assert.NotNull(ActionResponsePostLiked);
            Assert.Null(ActionResponsePostLiked.Cid);
            Assert.Null(ActionResponsePostLiked.Uri);
            Assert.Null(ActionResponsePostLiked.ValidationStatus);
            Assert.Null(ActionResponsePostLiked.Commit);
            Assert.NotEmpty(ActionResponsePostLiked.ErrorMessage);
            Assert.Equal(ErrorMessage.POST_ALREADY_LIKED, ActionResponsePostLiked.ErrorMessage);

            Assert.NotNull(ActionResponsePostNull);
            Assert.Null(ActionResponsePostNull.Cid);
            Assert.Null(ActionResponsePostNull.Uri);
            Assert.Null(ActionResponsePostNull.ValidationStatus);
            Assert.Null(ActionResponsePostNull.Commit);
            Assert.NotEmpty(ActionResponsePostNull.ErrorMessage);
            Assert.Equal(ErrorMessage.POST_IS_NULL, ActionResponsePostNull.ErrorMessage);

            Assert.NotNull(ActionResponsePostStringNull);
            Assert.Null(ActionResponsePostStringNull.Cid);
            Assert.Null(ActionResponsePostStringNull.Uri);
            Assert.Null(ActionResponsePostStringNull.ValidationStatus);
            Assert.Null(ActionResponsePostStringNull.Commit);
            Assert.NotEmpty(ActionResponsePostStringNull.ErrorMessage);
            Assert.Equal(ErrorMessage.POST_URI_OR_CID_IS_NULL, ActionResponsePostStringNull.ErrorMessage);

            Assert.NotNull(ActionResponsePostEmpty);
            Assert.Null(ActionResponsePostEmpty.Cid);
            Assert.Null(ActionResponsePostEmpty.Uri);
            Assert.Null(ActionResponsePostEmpty.ValidationStatus);
            Assert.Null(ActionResponsePostEmpty.Commit);
            Assert.NotEmpty(ActionResponsePostEmpty.ErrorMessage);
            Assert.Equal(ErrorMessage.VIEWER_IS_NULL, ActionResponsePostEmpty.ErrorMessage);

        }

        [Fact]
        public void UnlikePostSuccess()
        {
            // Arrange
            AuthUser authUser = Utils.GetAuthUser();

            ActionResponse fakeAction = Utils.GetActionResponse();

            Viewer viewer = new Viewer
            {
                Like = "ded//dedwdw/dewdwd/dweretertg"
            };

            Post post = Utils.GetPost();
            post.Viewer = viewer;

            var fakeAcionJSON = JsonSerializer.Serialize(fakeAction);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Post, fakeAcionJSON);
            var client = new ATPClient(httpClient);

            //Act
            ActionResponse actionResponse = client.UnlikePost(authUser, post);


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
            AuthUser authUser = Utils.GetAuthUser();

            Viewer viewer = new Viewer
            {
                Like = "ded//dedwdw/dewdwd/dweretertg"
            };

            Post post = Utils.GetPost();
            post.Viewer = viewer;

            ActionResponse fakeAction = Utils.GetActionResponseError();

            var fakeAcionJSON = JsonSerializer.Serialize(fakeAction);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.BadRequest, HttpMethod.Post, fakeAcionJSON);
            var client = new ATPClient(httpClient);

            //Act
            ActionResponse actionResponse = client.LikePost(authUser, post);


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
            AuthUser authUser = Utils.GetAuthUser();
            Post postNotLiked = Utils.GetPost();
            Post postStringNull = new Post
            {
                Viewer = new Viewer()
            };
            Post postEmpty = new Post();

            //Act
            var client = new ATPClient();

            ActionResponse ActionResponseAuthNull = client.UnlikePost(null, postNotLiked);
            ActionResponse ActionResponsePostLiked = client.UnlikePost(authUser, postNotLiked);
            ActionResponse ActionResponsePostNull = client.UnlikePost(authUser, null);
            ActionResponse ActionResponsePostStringNull = client.UnlikePost(authUser, postStringNull);
            ActionResponse ActionResponsePostEmpty = client.UnlikePost(authUser, postEmpty);

            //Verify
            Assert.NotNull(ActionResponseAuthNull);
            Assert.Null(ActionResponseAuthNull.Cid);
            Assert.Null(ActionResponseAuthNull.Uri);
            Assert.Null(ActionResponseAuthNull.ValidationStatus);
            Assert.Null(ActionResponseAuthNull.Commit);
            Assert.NotEmpty(ActionResponseAuthNull.ErrorMessage);
            Assert.Equal(ErrorMessage.USER_NOT_AUTHENTICATED, ActionResponseAuthNull.ErrorMessage);

            Assert.NotNull(ActionResponsePostLiked);
            Assert.Null(ActionResponsePostLiked.Cid);
            Assert.Null(ActionResponsePostLiked.Uri);
            Assert.Null(ActionResponsePostLiked.ValidationStatus);
            Assert.Null(ActionResponsePostLiked.Commit);
            Assert.NotEmpty(ActionResponsePostLiked.ErrorMessage);
            Assert.Equal(ErrorMessage.POST_NOT_LIKED, ActionResponsePostLiked.ErrorMessage);

            Assert.NotNull(ActionResponsePostNull);
            Assert.Null(ActionResponsePostNull.Cid);
            Assert.Null(ActionResponsePostNull.Uri);
            Assert.Null(ActionResponsePostNull.ValidationStatus);
            Assert.Null(ActionResponsePostNull.Commit);
            Assert.NotEmpty(ActionResponsePostNull.ErrorMessage);
            Assert.Equal(ErrorMessage.POST_IS_NULL, ActionResponsePostNull.ErrorMessage);

            Assert.NotNull(ActionResponsePostStringNull);
            Assert.Null(ActionResponsePostStringNull.Cid);
            Assert.Null(ActionResponsePostStringNull.Uri);
            Assert.Null(ActionResponsePostStringNull.ValidationStatus);
            Assert.Null(ActionResponsePostStringNull.Commit);
            Assert.NotEmpty(ActionResponsePostStringNull.ErrorMessage);
            Assert.Equal(ErrorMessage.POST_URI_OR_CID_IS_NULL, ActionResponsePostStringNull.ErrorMessage);

            Assert.NotNull(ActionResponsePostEmpty);
            Assert.Null(ActionResponsePostEmpty.Cid);
            Assert.Null(ActionResponsePostEmpty.Uri);
            Assert.Null(ActionResponsePostEmpty.ValidationStatus);
            Assert.Null(ActionResponsePostEmpty.Commit);
            Assert.NotEmpty(ActionResponsePostEmpty.ErrorMessage);
            Assert.Equal(ErrorMessage.VIEWER_IS_NULL, ActionResponsePostEmpty.ErrorMessage);

        }

        [Fact]
        public void ThreadSuccess()
        {
            AuthUser authUser = Utils.GetAuthUser();


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

            ThreadResponse threadResponse = new ThreadResponse
            {
                thread = thread
            };

            var fakeAcionJSON = JsonSerializer.Serialize(threadResponse);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Get, fakeAcionJSON);
            var client = new ATPClient(httpClient);

            //Act
            ThreadResponse PostResult = client.GetPostThread(authUser, "uri//dijdsiduoifjufhjeujfef");


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
            AuthUser authUser = Utils.GetAuthUser();

            ThreadResponse threadResponse = new ThreadResponse
            {
                ErrorMessage = "RANDOM ERROR"
            };

            var fakeAcionJSON = JsonSerializer.Serialize(threadResponse);

            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.InternalServerError, HttpMethod.Get, fakeAcionJSON);
            var client = new ATPClient(httpClient);

            //Act
            ThreadResponse PostResult = client.GetPostThread(authUser, "uri//dijdsiduoifjufhjeujfef");

            //Verify
            Assert.NotNull(PostResult);
            Assert.Null(PostResult.thread);
            Assert.NotEmpty(PostResult.ErrorMessage);

        }

        [Fact]
        public void ThreadArgumentFail()
        {
            AuthUser authUser = Utils.GetAuthUser();
            var client = new ATPClient();

            //Act
            ThreadResponse PostResultNotAuth = client.GetPostThread(null, "uri//dijdsiduoifjufhjeujfef");
            ThreadResponse PostResultUriEmpy = client.GetPostThread(authUser, "");
            ThreadResponse PostResultWrongLimit = client.GetPostThread(authUser, "uri//dijdsiduoifjufhjeujfef", -3);


            //Verify
            Assert.NotNull(PostResultNotAuth);
            Assert.Null(PostResultNotAuth.thread);
            Assert.Equal(ErrorMessage.USER_NOT_AUTHENTICATED, PostResultNotAuth.ErrorMessage);

            Assert.NotNull(PostResultUriEmpy);
            Assert.Null(PostResultUriEmpy.thread);
            Assert.Equal(ErrorMessage.POST_URI_IS_NULL, PostResultUriEmpy.ErrorMessage);

            Assert.NotNull(PostResultWrongLimit);
            Assert.Null(PostResultWrongLimit.thread);
            Assert.Equal(ErrorMessage.LIMIT_IS_NEGATIVE, PostResultWrongLimit.ErrorMessage);

        }

    }

}
