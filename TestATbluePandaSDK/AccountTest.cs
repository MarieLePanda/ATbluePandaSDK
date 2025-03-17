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

namespace TestATbluePandaSDK
{
    public class AccountTest
    {
        [Fact]
        public void FollowSuccess()
        {
            // Arrange
            AuthUser authUser = Utils.GetAuthUser();
            ActionResponse fakeAction = Utils.GetActionResponse();
            User user = Utils.GetUser();

            var fakeActionJSON = JsonSerializer.Serialize(fakeAction);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Post, fakeActionJSON);

            var client = new ATPClient(httpClient);

            //Act
            ActionResponse actionResponse = client.Follow(authUser, user);


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
            AuthUser authUser = Utils.GetAuthUser();
            ActionResponse fakeAction = Utils.GetActionResponseError();
            User user = Utils.GetUser();

            var fakeActionJSON = JsonSerializer.Serialize(fakeAction);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.BadRequest, HttpMethod.Post, fakeActionJSON);

            var client = new ATPClient(httpClient);

            //Act
            ActionResponse actionResponse = client.Follow(authUser, user);

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
            
            AuthUser authUser = Utils.GetAuthUser();
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

            ActionResponse ActionResponseAuthNull = client.Follow(null, userFollowed);
            ActionResponse ActionResponseUserFollowed = client.Follow(authUser, userFollowed);
            ActionResponse ActionResponseUserNull = client.Follow(authUser, null);
            ActionResponse ActionResponseUserStringNull = client.Follow(authUser, userStringNull);
            ActionResponse ActionResponseUserEmpty = client.Follow(authUser, userEmpty);
            ActionResponse ActionResponseSameUser = client.Follow(authUser, sameUser);



            //Verify
            Assert.NotNull(ActionResponseAuthNull);
            Assert.Null(ActionResponseAuthNull.Cid);
            Assert.Null(ActionResponseAuthNull.Uri);
            Assert.Null(ActionResponseAuthNull.ValidationStatus);
            Assert.Null(ActionResponseAuthNull.Commit);
            Assert.NotEmpty(ActionResponseAuthNull.ErrorMessage);
            Assert.Equal(ErrorMessage.USER_NOT_AUTHENTICATED, ActionResponseAuthNull.ErrorMessage);

            Assert.NotNull(ActionResponseUserFollowed);
            Assert.Null(ActionResponseUserFollowed.Cid);
            Assert.Null(ActionResponseUserFollowed.Uri);
            Assert.Null(ActionResponseUserFollowed.ValidationStatus);
            Assert.Null(ActionResponseUserFollowed.Commit);
            Assert.NotEmpty(ActionResponseUserFollowed.ErrorMessage);
            Assert.Equal(ErrorMessage.USER_ALREADY_FOLLOWED, ActionResponseUserFollowed.ErrorMessage);

            Assert.NotNull(ActionResponseUserNull);
            Assert.Null(ActionResponseUserNull.Cid);
            Assert.Null(ActionResponseUserNull.Uri);
            Assert.Null(ActionResponseUserNull.ValidationStatus);
            Assert.Null(ActionResponseUserNull.Commit);
            Assert.NotEmpty(ActionResponseUserNull.ErrorMessage);
            Assert.Equal(ErrorMessage.USER_IS_NULL, ActionResponseUserNull.ErrorMessage);

            Assert.NotNull(ActionResponseUserStringNull);
            Assert.Null(ActionResponseUserStringNull.Cid);
            Assert.Null(ActionResponseUserStringNull.Uri);
            Assert.Null(ActionResponseUserStringNull.ValidationStatus);
            Assert.Null(ActionResponseUserStringNull.Commit);
            Assert.NotEmpty(ActionResponseUserStringNull.ErrorMessage);
            Assert.Equal(ErrorMessage.USER_DID_IS_NULL, ActionResponseUserStringNull.ErrorMessage);

            Assert.NotNull(ActionResponseUserEmpty);
            Assert.Null(ActionResponseUserEmpty.Cid);
            Assert.Null(ActionResponseUserEmpty.Uri);
            Assert.Null(ActionResponseUserEmpty.ValidationStatus);
            Assert.Null(ActionResponseUserEmpty.Commit);
            Assert.NotEmpty(ActionResponseUserEmpty.ErrorMessage);
            Assert.Equal(ErrorMessage.VIEWER_IS_NULL, ActionResponseUserEmpty.ErrorMessage);

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
            AuthUser authUser = Utils.GetAuthUser();
            ActionResponse fakeAction = Utils.GetActionResponse();
            User user = Utils.GetUser();

            Viewer viewer = new Viewer
            {
                Following = "ded//dedwdw/dewdwd/dweretertg"
            };

            user.Viewer = viewer;

            var fakeActionJSON = JsonSerializer.Serialize(fakeAction);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Post, fakeActionJSON);
            var client = new ATPClient(httpClient);

            //Act
            ActionResponse actionResponse = client.Unfollow(authUser, user);

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
            AuthUser authUser = Utils.GetAuthUser();
            ActionResponse fakeAction = Utils.GetActionResponseError();
            User user = Utils.GetUser();

            Viewer viewer = new Viewer
            {
                Following = "ded//dedwdw/dewdwd/dweretertg"
            };

            user.Viewer = viewer;

            var fakeActionJSON = JsonSerializer.Serialize(fakeAction);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.InternalServerError, HttpMethod.Post, fakeActionJSON);
            var client = new ATPClient(httpClient);

            //Act
            ActionResponse actionResponse = client.Unfollow(authUser, user);

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
            AuthUser authUser = Utils.GetAuthUser();
            Viewer viewer = new Viewer();
            User userNotFollowed = Utils.GetUser();
            User sameUser = Utils.GetUser();
            User userStringNull = new User();
            User userEmpty = new User();

            userStringNull.Viewer = viewer;
            sameUser.Did = authUser.Did;
            
            //Act
            var client = new ATPClient();

            ActionResponse ActionResponseAuthNull = client.Unfollow(null, userNotFollowed);
            ActionResponse ActionResponseUserNotFollowed = client.Unfollow(authUser, userNotFollowed);
            ActionResponse ActionResponseUserNull = client.Unfollow(authUser, null);
            ActionResponse ActionResponseUserStringNull = client.Unfollow(authUser, userStringNull);
            ActionResponse ActionResponseUserEmpty = client.Unfollow(authUser, userEmpty);
            ActionResponse ActionResponseSameUser = client.Unfollow(authUser, sameUser);

            //Verify
            Assert.NotNull(ActionResponseAuthNull);
            Assert.Null(ActionResponseAuthNull.Cid);
            Assert.Null(ActionResponseAuthNull.Uri);
            Assert.Null(ActionResponseAuthNull.ValidationStatus);
            Assert.Null(ActionResponseAuthNull.Commit);
            Assert.NotEmpty(ActionResponseAuthNull.ErrorMessage);
            Assert.Equal(ErrorMessage.USER_NOT_AUTHENTICATED, ActionResponseAuthNull.ErrorMessage);

            Assert.NotNull(ActionResponseUserNotFollowed);
            Assert.Null(ActionResponseUserNotFollowed.Cid);
            Assert.Null(ActionResponseUserNotFollowed.Uri);
            Assert.Null(ActionResponseUserNotFollowed.ValidationStatus);
            Assert.Null(ActionResponseUserNotFollowed.Commit);
            Assert.NotEmpty(ActionResponseUserNotFollowed.ErrorMessage);
            Assert.Equal(ErrorMessage.USER_NOT_FOLLOWED, ActionResponseUserNotFollowed.ErrorMessage);

            Assert.NotNull(ActionResponseUserNull);
            Assert.Null(ActionResponseUserNull.Cid);
            Assert.Null(ActionResponseUserNull.Uri);
            Assert.Null(ActionResponseUserNull.ValidationStatus);
            Assert.Null(ActionResponseUserNull.Commit);
            Assert.NotEmpty(ActionResponseUserNull.ErrorMessage);
            Assert.Equal(ErrorMessage.USER_IS_NULL, ActionResponseUserNull.ErrorMessage);

            Assert.NotNull(ActionResponseUserStringNull);
            Assert.Null(ActionResponseUserStringNull.Cid);
            Assert.Null(ActionResponseUserStringNull.Uri);
            Assert.Null(ActionResponseUserStringNull.ValidationStatus);
            Assert.Null(ActionResponseUserStringNull.Commit);
            Assert.NotEmpty(ActionResponseUserStringNull.ErrorMessage);
            Assert.Equal(ErrorMessage.USER_DID_IS_NULL, ActionResponseUserStringNull.ErrorMessage);

            Assert.NotNull(ActionResponseUserEmpty);
            Assert.Null(ActionResponseUserEmpty.Cid);
            Assert.Null(ActionResponseUserEmpty.Uri);
            Assert.Null(ActionResponseUserEmpty.ValidationStatus);
            Assert.Null(ActionResponseUserEmpty.Commit);
            Assert.NotEmpty(ActionResponseUserEmpty.ErrorMessage);
            Assert.Equal(ErrorMessage.VIEWER_IS_NULL, ActionResponseUserEmpty.ErrorMessage);

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
            AuthUser authUser = Utils.GetAuthUser();
            User user = Utils.GetUser();
            ActionResponse fakeAction = Utils.GetActionResponse();

            var fakeActionJSON = JsonSerializer.Serialize(fakeAction);

            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Post, fakeActionJSON);

            ATPClient client = new ATPClient(httpClient);


            ActionResponse actionResponse = client.MuteUser(authUser, user);

            Assert.NotNull(actionResponse);
            Assert.Equal(HttpStatusCode.OK, actionResponse.StatusCode);
            Assert.Null(actionResponse.ErrorMessage);

        }

        [Fact]
        public void MuteFail()
        {
            AuthUser authUser = Utils.GetAuthUser();
            User user = Utils.GetUser();
            //user.Did = "did:plc:oxslbmjeqfhddwb5eac3knqm";
            ActionResponse fakeAction = Utils.GetActionResponseError();

            var fakeActionJSON = JsonSerializer.Serialize(fakeAction);

            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.BadRequest, HttpMethod.Post, fakeActionJSON);

            ATPClient client = new ATPClient(httpClient);


            ActionResponse actionResponse = client.MuteUser(authUser, user);

            Assert.NotNull(actionResponse);
            Assert.Equal(HttpStatusCode.BadRequest, actionResponse.StatusCode);
            Assert.NotEmpty(actionResponse.ErrorMessage);

        }

        [Fact]
        public void MuteWrongArgumentFail()
        {
            ATPClient client = new ATPClient();
            AuthUser authUser = Utils.GetAuthUser();
            User userMuted = Utils.GetUser();
            userMuted.Viewer.Muted = true;
            User userEmpty = new User();
            User userStringNull = new User();
            userStringNull.Viewer = new Viewer { Muted = true };
            User sameUser = Utils.GetUser();
            sameUser.Did = authUser.Did;

            ActionResponse ActionResponseAuthNull = client.MuteUser(null, userMuted);
            ActionResponse ActionResponseUserMutted = client.MuteUser(authUser, userMuted);
            ActionResponse ActionResponseUserNull = client.MuteUser(authUser, null);
            ActionResponse ActionResponseUserStringNull = client.MuteUser(authUser, userStringNull);
            ActionResponse ActionResponseUserEmpty = client.MuteUser(authUser, userEmpty);
            ActionResponse ActionResponseSameUser = client.MuteUser(authUser, sameUser);



            Assert.NotNull(ActionResponseAuthNull);
            Assert.NotEmpty(ActionResponseAuthNull.ErrorMessage);
            Assert.Null(ActionResponseAuthNull.Cid);
            Assert.Equal(ErrorMessage.USER_NOT_AUTHENTICATED, ActionResponseAuthNull.ErrorMessage);

            Assert.NotNull(ActionResponseUserMutted);
            Assert.NotEmpty(ActionResponseUserMutted.ErrorMessage);
            Assert.Null(ActionResponseUserMutted.Cid);
            Assert.Equal(ErrorMessage.USER_ALREADY_MUTED, ActionResponseUserMutted.ErrorMessage);

            Assert.NotNull(ActionResponseUserNull);
            Assert.NotEmpty(ActionResponseUserNull.ErrorMessage);
            Assert.Null(ActionResponseUserNull.Cid);
            Assert.Equal(ErrorMessage.USER_IS_NULL, ActionResponseUserNull.ErrorMessage);

            Assert.NotNull(ActionResponseUserStringNull);
            Assert.NotEmpty(ActionResponseUserStringNull.ErrorMessage);
            Assert.Null(ActionResponseUserStringNull.Cid);
            Assert.Equal(ErrorMessage.USER_DID_IS_NULL, ActionResponseUserStringNull.ErrorMessage);

            Assert.NotNull(ActionResponseUserEmpty);
            Assert.NotEmpty(ActionResponseUserEmpty.ErrorMessage);
            Assert.Null(ActionResponseUserEmpty.Cid);
            Assert.Equal(ErrorMessage.VIEWER_IS_NULL, ActionResponseUserEmpty.ErrorMessage);

            Assert.NotNull(ActionResponseSameUser);
            Assert.NotEmpty(ActionResponseSameUser.ErrorMessage);
            Assert.Null(ActionResponseSameUser.Cid);
            Assert.Equal(ErrorMessage.SAME_DID_USER, ActionResponseSameUser.ErrorMessage);

        }

        [Fact]
        public void UnMuteUserSuccess()
        {
            AuthUser authUser = Utils.GetAuthUser();
            User user = Utils.GetUser();
            user.Viewer.Muted = true;

            ActionResponse fakeAction = Utils.GetActionResponse();

            var fakeActionJSON = JsonSerializer.Serialize(fakeAction);

            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Post, fakeActionJSON);

            ATPClient client = new ATPClient(httpClient);

            ActionResponse actionResponseUnMute = client.UnMuteUser(authUser, user);

            Assert.NotNull(actionResponseUnMute);
            Assert.Equal(HttpStatusCode.OK, actionResponseUnMute.StatusCode);
            Assert.Null(actionResponseUnMute.ErrorMessage);

        }

        [Fact]
        public void UnMuteUserFail()
        {
            AuthUser authUser = Utils.GetAuthUser();
            User user = Utils.GetUser();
            user.Viewer.Muted = true;

            ActionResponse fakeAction = Utils.GetActionResponseError();
            fakeAction.StatusCode = HttpStatusCode.BadRequest;

            var fakeActionJSON = JsonSerializer.Serialize(fakeAction);

            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.BadRequest, HttpMethod.Post, fakeActionJSON);

            ATPClient client = new ATPClient(httpClient);

            ActionResponse actionResponseUnMute = client.UnMuteUser(authUser, user);

            Assert.NotNull(actionResponseUnMute);
            Assert.Equal(HttpStatusCode.BadRequest, actionResponseUnMute.StatusCode);
            Assert.NotEmpty(actionResponseUnMute.ErrorMessage);
            
        }

        [Fact]
        public void UnMuteWrongArgumentFail()
        {
            ATPClient client = new ATPClient();
            AuthUser authUser = Utils.GetAuthUser();
            User userUnMuted = Utils.GetUser();
            userUnMuted.Viewer.Muted = false;
            User userEmpty = new User();
            User userStringNull = new User();
            userStringNull.Viewer = new Viewer { Muted = true };
            User sameUser = Utils.GetUser();
            sameUser.Did = authUser.Did;

            ActionResponse ActionResponseAuthNull = client.UnMuteUser(null, userUnMuted);
            ActionResponse ActionResponseUserUnMutted = client.UnMuteUser(authUser, userUnMuted);
            ActionResponse ActionResponseUserNull = client.UnMuteUser(authUser, null);
            ActionResponse ActionResponseUserStringNull = client.UnMuteUser(authUser, userStringNull);
            ActionResponse ActionResponseUserEmpty = client.UnMuteUser(authUser, userEmpty);
            ActionResponse ActionResponseSameUser = client.UnMuteUser(authUser, sameUser);



            Assert.NotNull(ActionResponseAuthNull);
            Assert.NotEmpty(ActionResponseAuthNull.ErrorMessage);
            Assert.Null(ActionResponseAuthNull.Cid);
            Assert.Equal(ErrorMessage.USER_NOT_AUTHENTICATED, ActionResponseAuthNull.ErrorMessage);

            Assert.NotNull(ActionResponseUserUnMutted);
            Assert.NotEmpty(ActionResponseUserUnMutted.ErrorMessage);
            Assert.Null(ActionResponseUserUnMutted.Cid);
            Assert.Equal(ErrorMessage.USER_NOT_MUTED, ActionResponseUserUnMutted.ErrorMessage);

            Assert.NotNull(ActionResponseUserNull);
            Assert.NotEmpty(ActionResponseUserNull.ErrorMessage);
            Assert.Null(ActionResponseUserNull.Cid);
            Assert.Equal(ErrorMessage.USER_IS_NULL, ActionResponseUserNull.ErrorMessage);

            Assert.NotNull(ActionResponseUserStringNull);
            Assert.NotEmpty(ActionResponseUserStringNull.ErrorMessage);
            Assert.Null(ActionResponseUserStringNull.Cid);
            Assert.Equal(ErrorMessage.USER_DID_IS_NULL, ActionResponseUserStringNull.ErrorMessage);

            Assert.NotNull(ActionResponseUserEmpty);
            Assert.NotEmpty(ActionResponseUserEmpty.ErrorMessage);
            Assert.Null(ActionResponseUserEmpty.Cid);
            Assert.Equal(ErrorMessage.VIEWER_IS_NULL, ActionResponseUserEmpty.ErrorMessage);

            Assert.NotNull(ActionResponseSameUser);
            Assert.NotEmpty(ActionResponseSameUser.ErrorMessage);
            Assert.Null(ActionResponseSameUser.Cid);
            Assert.Equal(ErrorMessage.SAME_DID_USER, ActionResponseSameUser.ErrorMessage);

        }
    }
}
