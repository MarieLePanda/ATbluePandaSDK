using ATbluePandaSDK.Models;
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
using ATPandaSDK.Models.Feed;
using ATbluePandaSDK;
using ATbluePandaSDK.Models.Account;

namespace TestATbluePandaSDK
{
    public class AccountTest
    {
        [Fact]
        public void FollowSuccess()
        {
            // Arrange
            BskyAuthUser authUser = Utils.GetAuthUser();
            BskyActionResponse fakeAction = Utils.GetActionResponse();
            User user = Utils.GetUser();

            var fakeActionJSON = JsonSerializer.Serialize(fakeAction);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Post, fakeActionJSON);

            var client = new ATPClient(authUser, httpClient);

            //Act
            BskyActionResponse actionResponse = client.Follow(user);


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
        public void FollowFail()
        {
            // Arrange
            BskyAuthUser authUser = Utils.GetAuthUser();
            BskyActionResponse fakeAction = Utils.GetActionResponseError();
            User user = Utils.GetUser();

            var fakeActionJSON = JsonSerializer.Serialize(fakeAction);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.BadRequest, HttpMethod.Post, fakeActionJSON);

            var client = new ATPClient(authUser, httpClient);

            //Act
            BskyActionResponse actionResponse = client.Follow(user);

            //Verify
            Assert.NotNull(actionResponse);
            Assert.Null(actionResponse.Cid);
            Assert.Null(actionResponse.Uri);
            Assert.Null(actionResponse.ValidationStatus);
            Assert.Null(actionResponse.Commit);
            Assert.NotEmpty(actionResponse.ErrorMessage);
        }

        [Fact]
        public void FollowWrongArgumentFail()
        {
            // Arrange
            
            BskyAuthUser authUser = Utils.GetAuthUser();
            User userFollowed = Utils.GetUser();
            User sameUser = Utils.GetUser();
            User userEmpty = new User();
            User userStringNull = new User();

            Viewer viewer = new Viewer
            {
                Following = "ded//dedwdw/dewdwd/dweretertg"
            };

            userFollowed.Viewer = viewer;
            userStringNull.Viewer = viewer;
            sameUser.Did = authUser.Did;

            //Act
            var client = new ATPClient();


            BskyException ActionResponseAuthNull = Assert.Throws<BskyException>(() => client.Follow(userFollowed, null));
            BskyActionResponse ActionResponseUserFollowed = client.Follow(userFollowed, authUser);
            ArgumentNullException ActionResponseUserNull = Assert.Throws<ArgumentNullException>(() => client.Follow(null, authUser));
            ArgumentException ActionResponseUserStringNull = Assert.Throws<ArgumentException>(() => client.Follow(userStringNull, authUser));
            ArgumentException ActionResponseUserEmpty = Assert.Throws<ArgumentException>(() => client.Follow(userEmpty, authUser));
            BskyActionResponse ActionResponseSameUser = client.Follow(sameUser, authUser);



            //Verify
            Assert.NotNull(ActionResponseAuthNull);
            Assert.NotEmpty(ActionResponseAuthNull.Message);
            Assert.Contains(ErrorMessage.USER_NOT_AUTHENTICATED, ActionResponseAuthNull.Message);

            Assert.NotNull(ActionResponseUserFollowed);
            Assert.Null(ActionResponseUserFollowed.Cid);
            Assert.Null(ActionResponseUserFollowed.Uri);
            Assert.Null(ActionResponseUserFollowed.ValidationStatus);
            Assert.Null(ActionResponseUserFollowed.Commit);
            Assert.NotEmpty(ActionResponseUserFollowed.ErrorMessage);
            Assert.Equal(ErrorMessage.USER_ALREADY_FOLLOWED, ActionResponseUserFollowed.ErrorMessage);

            Assert.NotNull(ActionResponseUserNull);
            Assert.NotEmpty(ActionResponseUserNull.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, ActionResponseUserNull.Message);

            Assert.NotNull(ActionResponseUserStringNull);
            Assert.NotEmpty(ActionResponseUserStringNull.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, ActionResponseUserStringNull.Message);

            Assert.NotNull(ActionResponseUserEmpty);
            Assert.NotEmpty(ActionResponseUserEmpty.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, ActionResponseUserEmpty.Message);

            Assert.NotNull(ActionResponseSameUser);
            Assert.Null(ActionResponseSameUser.Cid);
            Assert.Null(ActionResponseSameUser.Uri);
            Assert.Null(ActionResponseSameUser.ValidationStatus);
            Assert.Null(ActionResponseSameUser.Commit);
            Assert.NotEmpty(ActionResponseSameUser.ErrorMessage);
            Assert.Equal(ErrorMessage.SAME_DID_USER, ActionResponseSameUser.ErrorMessage);

        }

        [Fact]
        public void UnfollowSuccess()
        {
            // Arrange
            BskyAuthUser authUser = Utils.GetAuthUser();
            BskyActionResponse fakeAction = Utils.GetActionResponse();
            User user = Utils.GetUser();

            Viewer viewer = new Viewer
            {
                Following = "ded//dedwdw/dewdwd/dweretertg"
            };

            user.Viewer = viewer;

            var fakeActionJSON = JsonSerializer.Serialize(fakeAction);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Post, fakeActionJSON);
            var client = new ATPClient(authUser, httpClient);

            //Act
            BskyActionResponse actionResponse = client.Unfollow(user);

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
        public void UnfollowFail()
        {
            // Arrange
            BskyAuthUser authUser = Utils.GetAuthUser();
            BskyActionResponse fakeAction = Utils.GetActionResponseError();
            User user = Utils.GetUser();

            Viewer viewer = new Viewer
            {
                Following = "ded//dedwdw/dewdwd/dweretertg"
            };

            user.Viewer = viewer;

            var fakeActionJSON = JsonSerializer.Serialize(fakeAction);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.InternalServerError, HttpMethod.Post, fakeActionJSON);
            var client = new ATPClient(authUser, httpClient);

            //Act
            BskyActionResponse actionResponse = client.Unfollow(user);

            //Verify
            Assert.NotNull(actionResponse);
            Assert.Null(actionResponse.Cid);
            Assert.Null(actionResponse.Uri);
            Assert.Null(actionResponse.ValidationStatus);
            Assert.Null(actionResponse.Commit);
            Assert.NotEmpty(actionResponse.ErrorMessage);
        }

        [Fact]
        public void UnfollowWrongArgumentFail()
        {
            // Arrange
            BskyAuthUser authUser = Utils.GetAuthUser();
            Viewer viewer = new Viewer();
            User userNotFollowed = Utils.GetUser();
            User sameUser = Utils.GetUser();
            User userStringNull = new User();
            User userEmpty = new User();

            userStringNull.Viewer = viewer;
            sameUser.Did = authUser.Did;
            
            //Act
            var client = new ATPClient();

            BskyException ActionResponseAuthNull = Assert.Throws<BskyException>(() => client.Unfollow(userNotFollowed, null));
            BskyActionResponse ActionResponseUserNotFollowed = client.Unfollow(userNotFollowed, authUser);
            ArgumentNullException ActionResponseUserNull = Assert.Throws<ArgumentNullException>(() => client.Unfollow(null, authUser));
            ArgumentException ActionResponseUserStringNull = Assert.Throws<ArgumentException>(() => client.Unfollow(userStringNull, authUser));
            ArgumentException ActionResponseUserEmpty = Assert.Throws<ArgumentException>(() => client.Unfollow(userEmpty, authUser));
            BskyActionResponse ActionResponseSameUser = client.Unfollow(sameUser, authUser);

            //Verify
            Assert.NotNull(ActionResponseAuthNull);
            Assert.NotEmpty(ActionResponseAuthNull.Message);
            Assert.Contains(ErrorMessage.USER_NOT_AUTHENTICATED, ActionResponseAuthNull.Message);

            Assert.NotNull(ActionResponseUserNotFollowed);
            Assert.Null(ActionResponseUserNotFollowed.Cid);
            Assert.Null(ActionResponseUserNotFollowed.Uri);
            Assert.Null(ActionResponseUserNotFollowed.ValidationStatus);
            Assert.Null(ActionResponseUserNotFollowed.Commit);
            Assert.NotEmpty(ActionResponseUserNotFollowed.ErrorMessage);
            Assert.Equal(ErrorMessage.USER_NOT_FOLLOWED, ActionResponseUserNotFollowed.ErrorMessage);

            Assert.NotNull(ActionResponseUserNull);
            Assert.NotEmpty(ActionResponseUserNull.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, ActionResponseUserNull.Message);

            Assert.NotNull(ActionResponseUserStringNull);
            Assert.NotEmpty(ActionResponseUserStringNull.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, ActionResponseUserStringNull.Message);

            Assert.NotNull(ActionResponseUserEmpty);
            Assert.NotEmpty(ActionResponseUserEmpty.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, ActionResponseUserEmpty.Message);

            Assert.NotNull(ActionResponseSameUser);
            Assert.Null(ActionResponseSameUser.Cid);
            Assert.Null(ActionResponseSameUser.Uri);
            Assert.Null(ActionResponseSameUser.ValidationStatus);
            Assert.Null(ActionResponseSameUser.Commit);
            Assert.NotEmpty(ActionResponseSameUser.ErrorMessage);
            Assert.Equal(ErrorMessage.SAME_DID_USER, ActionResponseSameUser.ErrorMessage);

        }

        [Fact]
        public void MuteUser()
        {
            BskyAuthUser authUser = Utils.GetAuthUser();
            User user = Utils.GetUser();
            BskyActionResponse fakeAction = Utils.GetActionResponse();

            var fakeActionJSON = JsonSerializer.Serialize(fakeAction);

            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Post, fakeActionJSON);

            var client = new ATPClient(authUser, httpClient);


            BskyActionResponse actionResponse = client.MuteUser(user);

            Assert.NotNull(actionResponse);
            Assert.Equal(HttpStatusCode.OK, actionResponse.StatusCode);
            Assert.Null(actionResponse.ErrorMessage);

        }

        [Fact]
        public void MuteFail()
        {
            BskyAuthUser authUser = Utils.GetAuthUser();
            User user = Utils.GetUser();
            BskyActionResponse fakeAction = Utils.GetActionResponseError();

            var fakeActionJSON = JsonSerializer.Serialize(fakeAction);

            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.BadRequest, HttpMethod.Post, fakeActionJSON);

            var client = new ATPClient(authUser, httpClient);


            BskyActionResponse actionResponse = client.MuteUser(user);

            Assert.NotNull(actionResponse);
            Assert.Equal(HttpStatusCode.BadRequest, actionResponse.StatusCode);
            Assert.NotEmpty(actionResponse.ErrorMessage);

        }

        [Fact]
        public void MuteWrongArgumentFail()
        {
            ATPClient client = new ATPClient();
            BskyAuthUser authUser = Utils.GetAuthUser();
            User userMuted = Utils.GetUser();
            userMuted.Viewer.Muted = true;
            User userEmpty = new User();
            User userStringNull = new User();
            userStringNull.Viewer = new Viewer { Muted = true };
            User sameUser = Utils.GetUser();
            sameUser.Did = authUser.Did;

            BskyException ActionResponseAuthNull = Assert.Throws<BskyException>(() => client.MuteUser(userMuted, null));
            BskyActionResponse ActionResponseUserMutted = client.MuteUser(userMuted, authUser);
            ArgumentNullException ActionResponseUserNull = Assert.Throws<ArgumentNullException>(() => client.MuteUser(null, authUser));
            ArgumentException ActionResponseUserStringNull = Assert.Throws<ArgumentException>(() => client.MuteUser(userStringNull, authUser));
            ArgumentException ActionResponseUserEmpty = Assert.Throws<ArgumentException>(() => client.MuteUser(userEmpty, authUser));
            BskyActionResponse ActionResponseSameUser = client.MuteUser(sameUser, authUser);



            Assert.NotNull(ActionResponseAuthNull);
            Assert.NotEmpty(ActionResponseAuthNull.Message);
            Assert.Contains(ErrorMessage.USER_NOT_AUTHENTICATED, ActionResponseAuthNull.Message);

            Assert.NotNull(ActionResponseUserMutted);
            Assert.NotEmpty(ActionResponseUserMutted.ErrorMessage);
            Assert.Null(ActionResponseUserMutted.Cid);
            Assert.Contains(ErrorMessage.USER_ALREADY_MUTED, ActionResponseUserMutted.ErrorMessage);

            Assert.NotNull(ActionResponseUserNull);
            Assert.NotEmpty(ActionResponseUserNull.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, ActionResponseUserNull.Message);

            Assert.NotNull(ActionResponseUserStringNull);
            Assert.NotEmpty(ActionResponseUserStringNull.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, ActionResponseUserStringNull.Message);

            Assert.NotNull(ActionResponseUserEmpty);
            Assert.NotEmpty(ActionResponseUserEmpty.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, ActionResponseUserEmpty.Message);

            Assert.NotNull(ActionResponseSameUser);
            Assert.NotEmpty(ActionResponseSameUser.ErrorMessage);
            Assert.Null(ActionResponseSameUser.Cid);
            Assert.Contains(ErrorMessage.SAME_DID_USER, ActionResponseSameUser.ErrorMessage);

        }

        [Fact]
        public void UnMuteUserSuccess()
        {
            BskyAuthUser authUser = Utils.GetAuthUser();
            User user = Utils.GetUser();
            user.Viewer.Muted = true;

            BskyActionResponse fakeAction = Utils.GetActionResponse();

            var fakeActionJSON = JsonSerializer.Serialize(fakeAction);

            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Post, fakeActionJSON);

            var client = new ATPClient(authUser, httpClient);

            BskyActionResponse actionResponseUnMute = client.UnMuteUser(user);

            Assert.NotNull(actionResponseUnMute);
            Assert.Equal(HttpStatusCode.OK, actionResponseUnMute.StatusCode);
            Assert.Null(actionResponseUnMute.ErrorMessage);

        }

        [Fact]
        public void UnMuteUserFail()
        {
            BskyAuthUser authUser = Utils.GetAuthUser();
            User user = Utils.GetUser();
            user.Viewer.Muted = true;

            BskyActionResponse fakeAction = Utils.GetActionResponseError();
            fakeAction.StatusCode = HttpStatusCode.BadRequest;

            var fakeActionJSON = JsonSerializer.Serialize(fakeAction);

            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.BadRequest, HttpMethod.Post, fakeActionJSON);

            ATPClient client = new ATPClient(authUser, httpClient);

            BskyActionResponse actionResponseUnMute = client.UnMuteUser(user);

            Assert.NotNull(actionResponseUnMute);
            Assert.Equal(HttpStatusCode.BadRequest, actionResponseUnMute.StatusCode);
            Assert.NotEmpty(actionResponseUnMute.ErrorMessage);
            
        }

        [Fact]
        public void UnMuteWrongArgumentFail()
        {
            ATPClient client = new ATPClient();
            BskyAuthUser authUser = Utils.GetAuthUser();
            User userUnMuted = Utils.GetUser();
            userUnMuted.Viewer.Muted = false;
            User userEmpty = new User();
            User userStringNull = new User();
            userStringNull.Viewer = new Viewer { Muted = true };
            User sameUser = Utils.GetUser();
            sameUser.Did = authUser.Did;

            BskyException ActionResponseAuthNull = Assert.Throws<BskyException>(() => client.UnMuteUser(userUnMuted, null));
            BskyActionResponse ActionResponseUserUnMutted = client.UnMuteUser(userUnMuted, authUser);
            ArgumentNullException ActionResponseUserNull = Assert.Throws<ArgumentNullException>(() => client.UnMuteUser(null, authUser));
            ArgumentException ActionResponseUserStringNull = Assert.Throws<ArgumentException>(() => client.UnMuteUser(userStringNull, authUser));
            ArgumentException ActionResponseUserEmpty = Assert.Throws<ArgumentException>(() => client.UnMuteUser(userEmpty, authUser));
            BskyActionResponse ActionResponseSameUser = client.UnMuteUser(sameUser, authUser);



            Assert.NotNull(ActionResponseAuthNull);
            Assert.NotEmpty(ActionResponseAuthNull.Message);
            Assert.Contains(ErrorMessage.USER_NOT_AUTHENTICATED, ActionResponseAuthNull.Message);

            Assert.NotNull(ActionResponseUserUnMutted);
            Assert.NotEmpty(ActionResponseUserUnMutted.ErrorMessage);
            Assert.Null(ActionResponseUserUnMutted.Cid);
            Assert.Equal(ErrorMessage.USER_NOT_MUTED, ActionResponseUserUnMutted.ErrorMessage);

            Assert.NotNull(ActionResponseUserNull);
            Assert.NotEmpty(ActionResponseUserNull.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, ActionResponseUserNull.Message);

            Assert.NotNull(ActionResponseUserStringNull);
            Assert.NotEmpty(ActionResponseUserStringNull.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, ActionResponseUserStringNull.Message);

            Assert.NotNull(ActionResponseUserEmpty);
            Assert.NotEmpty(ActionResponseUserEmpty.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, ActionResponseUserEmpty.Message);

            Assert.NotNull(ActionResponseSameUser);
            Assert.NotEmpty(ActionResponseSameUser.ErrorMessage);
            Assert.Null(ActionResponseSameUser.Cid);
            Assert.Equal(ErrorMessage.SAME_DID_USER, ActionResponseSameUser.ErrorMessage);

        }

        [Fact]
        public void GetUserProfileSuccess()
        {
            BskyAuthUser authUser = Utils.GetAuthUser();
            User user = Utils.GetUser();
            
            var fakeUserProfile = JsonSerializer.Serialize(Utils.GetUser());
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Get, fakeUserProfile);

            var client = new ATPClient(authUser, httpClient);

            User userProfile = client.GetUserProfil(user.Did);

            Assert.NotNull(userProfile);

        }

        [Fact]
        public void GetUserProfileFail()
        {
            BskyAuthUser authUser = Utils.GetAuthUser();
            User user = Utils.GetUser();

            Response fakeResponse = new Response();
            string errorMessage = "Profile don't exist";

            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.BadRequest, HttpMethod.Get, errorMessage);


            var client = new ATPClient(authUser, httpClient);

            BskyException userProfile = Assert.Throws<BskyException>(() => client.GetUserProfil(user.Did));

            Assert.NotNull(userProfile);
            Assert.NotEmpty(userProfile.Message);

        }
        //Todo
        //[Fact]
        public void BlockUser()
        {
            ATPClient client = new ATPClient();
            User userToBlock = Utils.GetUser();
            BskyTimeline timelineResponse = client.GetAuthorTimeline();
            foreach(var feed in timelineResponse.Feed)
            {
                Console.WriteLine(feed.Post.Author.DisplayName);
                Console.WriteLine(feed.Post.Record.Text);
                var replies = client.GetPostThread(feed.Post.Uri).thread.replies;
                foreach (var reply in replies)
                {
                    if(reply.post.Author.Did != client.AuthUser.Did)
                    {
                        userToBlock = reply.post.Author;
                        break;
                    }
                    Console.WriteLine(reply.post.Author.DisplayName);
                    Console.WriteLine(reply.post.Record.Text);
                }
            }
            
            BskyActionResponse actionResponse = client.MuteUser(userToBlock);

            Assert.NotNull(actionResponse);
            Assert.Equal(HttpStatusCode.OK, actionResponse.StatusCode);
            Assert.Null(actionResponse.ErrorMessage);

        }

    }
}
