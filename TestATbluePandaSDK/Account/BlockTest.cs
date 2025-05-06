using ATbluePandaSDK.Models;
using ATPandaSDK.Models.Auth;
using ATPandaSDK.Models.Feed;
using ATPandaSDK;
using System.Net;
using System.Text.Json;
using ATbluePandaSDK;
using ATbluePandaSDK.Models.Account;
using Xunit.Sdk;
using ErrorMessage = ATbluePandaSDK.ErrorMessage;

namespace TestATbluePandaSDK.Account
{
    public class BlockTest
    {
        [Fact]
        public void BlockUserSuccess()
        {
            BskyAuthUser authUser = Utils.GetAuthUser();
            User user = Utils.GetUser();

            BskyActionResponse fakeAction = Utils.GetActionResponse();

            var fakeActionJSON = JsonSerializer.Serialize(fakeAction);

            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Post, fakeActionJSON);

            var client = new ATPClient(authUser, httpClient);


            BskyActionResponse actionResponse = client.BlockUser(user);

            Assert.NotNull(actionResponse);
            Assert.Equal(HttpStatusCode.OK, actionResponse.StatusCode);
            Assert.Null(actionResponse.ErrorMessage);

        }

        [Fact]
        public void BlockUserWithStringParameterSuccess()
        {
            BskyAuthUser authUser = Utils.GetAuthUser();
            User user = Utils.GetUser();

            BskyActionResponse fakeAction = Utils.GetActionResponse();

            var fakeActionJSON = JsonSerializer.Serialize(fakeAction);

            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Post, fakeActionJSON);

            var client = new ATPClient(authUser, httpClient);


            BskyActionResponse actionResponse = client.BlockUser(user.Did);

            Assert.NotNull(actionResponse);
            Assert.Equal(HttpStatusCode.OK, actionResponse.StatusCode);
            Assert.Null(actionResponse.ErrorMessage);

        }


        [Fact]
        public void BlockErrorFromBskyFail()
        {
            BskyAuthUser authUser = Utils.GetAuthUser();
            User user = Utils.GetUser();
            BskyActionResponse fakeAction = Utils.GetActionResponseError();

            var fakeActionJSON = JsonSerializer.Serialize(fakeAction);

            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.BadRequest, HttpMethod.Post, fakeActionJSON);

            var client = new ATPClient(authUser, httpClient);


            BskyActionResponse actionResponse = client.BlockUser(user);

            Assert.NotNull(actionResponse);
            Assert.Equal(HttpStatusCode.BadRequest, actionResponse.StatusCode);
            Assert.NotEmpty(actionResponse.ErrorMessage);
        }

        [Fact]
        public void BlockWithStringParameterErrorFromBskyFail()
        {
            BskyAuthUser authUser = Utils.GetAuthUser();
            User user = Utils.GetUser();
            BskyActionResponse fakeAction = Utils.GetActionResponseError();

            var fakeActionJSON = JsonSerializer.Serialize(fakeAction);

            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.BadRequest, HttpMethod.Post, fakeActionJSON);

            var client = new ATPClient(authUser, httpClient);


            BskyActionResponse actionResponse = client.BlockUser(user.Did);

            Assert.NotNull(actionResponse);
            Assert.Equal(HttpStatusCode.BadRequest, actionResponse.StatusCode);
            Assert.NotEmpty(actionResponse.ErrorMessage);
        }

        [Fact]
        public void BlockAuthNotConnectedOrNullFail()
        {
            ATPClient client = new ATPClient();
            BskyAuthUser authUser = Utils.GetAuthUser();
            User userBlocked = Utils.GetUser(blocked: true);

            BskyException ActionResponseAuthNull = Assert.Throws<BskyException>(() => client.BlockUser(userBlocked, null));

            Assert.NotNull(ActionResponseAuthNull);
            Assert.NotEmpty(ActionResponseAuthNull.Message);
            Assert.Contains(ErrorMessage.USER_NOT_AUTHENTICATED, ActionResponseAuthNull.Message);

        }

        [Fact]
        public void BlockWithStringParameterAuthNotConnectedOrNullFail()
        {
            ATPClient client = new ATPClient();
            BskyAuthUser authUser = Utils.GetAuthUser();
            User userBlocked = Utils.GetUser(blocked: true);

            BskyException ActionResponseAuthNull = Assert.Throws<BskyException>(() => client.BlockUser(userBlocked.Did, "", ""));

            Assert.NotNull(ActionResponseAuthNull);
            Assert.NotEmpty(ActionResponseAuthNull.Message);
            Assert.Contains(ErrorMessage.USER_NOT_AUTHENTICATED, ActionResponseAuthNull.Message);

        }

        [Fact]
        public void BlockMySelfFail()
        {

            ATPClient client = new ATPClient();
            BskyAuthUser authUser = Utils.GetAuthUser();
            User sameUser = Utils.GetUser();
            sameUser.Did = authUser.Did;

            BskyActionResponse ActionResponseSameUser = client.BlockUser(sameUser, authUser);

            Assert.NotNull(ActionResponseSameUser);
            Assert.NotEmpty(ActionResponseSameUser.ErrorMessage);
            Assert.Null(ActionResponseSameUser.Cid);
            Assert.Contains(ErrorMessage.SAME_DID_USER, ActionResponseSameUser.ErrorMessage);

        }

        [Fact]
        public void BlockWithStringParameterMySelfFail()
        {

            ATPClient client = new ATPClient();
            BskyAuthUser authUser = Utils.GetAuthUser();
            User sameUser = Utils.GetUser();
            sameUser.Did = authUser.Did;

            BskyActionResponse ActionResponseSameUser = client.BlockUser(sameUser.Did, authUser.Did, authUser.AccessJwt);

            Assert.NotNull(ActionResponseSameUser);
            Assert.NotEmpty(ActionResponseSameUser.ErrorMessage);
            Assert.Null(ActionResponseSameUser.Cid);
            Assert.Contains(ErrorMessage.SAME_DID_USER, ActionResponseSameUser.ErrorMessage);

        }

        [Fact]
        public void BlockUserBlockedFailed()
        {

            ATPClient client = new ATPClient();
            BskyAuthUser authUser = Utils.GetAuthUser();
            User userBlocked = Utils.GetUser(blocked: true);
            userBlocked.Viewer.Blocking = "blocked";

            BskyActionResponse ActionResponseUserBlocked = client.BlockUser(userBlocked, authUser);

            Assert.NotNull(ActionResponseUserBlocked);
            Assert.NotEmpty(ActionResponseUserBlocked.ErrorMessage);
            Assert.Null(ActionResponseUserBlocked.Cid);
            Assert.Contains(ErrorMessage.USER_ALREADY_BLOCKED, ActionResponseUserBlocked.ErrorMessage);

        }


        [Fact]
        public void BlockUserIsNullOrEmptyFail()
        {
            ATPClient client = new ATPClient();
            BskyAuthUser authUser = Utils.GetAuthUser();

            User userEmpty = new User();
            User userStringNull = new User();

            ArgumentNullException ActionResponseUserNull = Assert.Throws<ArgumentNullException>(() => client.BlockUser(null, authUser));
            ArgumentException ActionResponseUserStringNull = Assert.Throws<ArgumentException>(() => client.BlockUser(userStringNull, authUser));
            ArgumentException ActionResponseUserEmpty = Assert.Throws<ArgumentException>(() => client.BlockUser(userEmpty, authUser));

            Assert.NotNull(ActionResponseUserNull);
            Assert.NotEmpty(ActionResponseUserNull.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, ActionResponseUserNull.Message);

            Assert.NotNull(ActionResponseUserStringNull);
            Assert.NotEmpty(ActionResponseUserStringNull.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, ActionResponseUserStringNull.Message);

            Assert.NotNull(ActionResponseUserEmpty);
            Assert.NotEmpty(ActionResponseUserEmpty.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, ActionResponseUserEmpty.Message);

        }

        [Fact]
        public void BlockWithStringParameterIsNullOrEmptyFail()
        {
            ATPClient client = new ATPClient();
            BskyAuthUser authUser = Utils.GetAuthUser();

            User userEmpty = new User();
            User userStringNull = new User();

            ArgumentException ActionResponseUserNull = Assert.Throws<ArgumentException>(() => client.BlockUser(null, authUser.Did, authUser.AccessJwt));
            ArgumentException ActionResponseUserStringNull = Assert.Throws<ArgumentException>(() => client.BlockUser("", authUser.Did, authUser.AccessJwt));

            Assert.NotNull(ActionResponseUserNull);
            Assert.NotEmpty(ActionResponseUserNull.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, ActionResponseUserNull.Message);

            Assert.NotNull(ActionResponseUserStringNull);
            Assert.NotEmpty(ActionResponseUserStringNull.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, ActionResponseUserStringNull.Message);

        }

        [Fact]
        public void UnblockSuccess()
        {
            BskyAuthUser authUser = Utils.GetAuthUser();
            User user = Utils.GetUser(blocked: true);
            BskyActionResponse fakeAction = Utils.GetActionResponse();

            var fakeActionJSON = JsonSerializer.Serialize(fakeAction);

            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Post, fakeActionJSON);

            var client = new ATPClient(authUser, httpClient);


            BskyActionResponse actionResponse = client.UnblockUser(user);

            Assert.NotNull(actionResponse);
            Assert.Equal(HttpStatusCode.OK, actionResponse.StatusCode);
            Assert.Null(actionResponse.ErrorMessage);
        }

        [Fact]
        public void UnblockErrorFromBskyFail()
        {
            BskyAuthUser authUser = Utils.GetAuthUser();
            User user = Utils.GetUser(blocked: true);
            BskyActionResponse fakeAction = Utils.GetActionResponseError();

            var fakeActionJSON = JsonSerializer.Serialize(fakeAction);

            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.BadRequest, HttpMethod.Post, fakeActionJSON);

            var client = new ATPClient(authUser, httpClient);


            BskyActionResponse actionResponse = client.UnblockUser(user);

            Assert.NotNull(actionResponse);
            Assert.Equal(HttpStatusCode.BadRequest, actionResponse.StatusCode);
            Assert.NotEmpty(actionResponse.ErrorMessage);
        }

        [Fact]
        public void UnblockAuthNotConnectedOrNullFail()
        {
            ATPClient client = new ATPClient();
            BskyAuthUser authUser = Utils.GetAuthUser();
            User userBlocked = Utils.GetUser(blocked: true);

            BskyException ActionResponseAuthNull = Assert.Throws<BskyException>(() => client.UnblockUser(userBlocked, null));

            Assert.NotNull(ActionResponseAuthNull);
            Assert.NotEmpty(ActionResponseAuthNull.Message);
            Assert.Contains(ErrorMessage.USER_NOT_AUTHENTICATED, ActionResponseAuthNull.Message);

        }

        [Fact]
        public void UnblockMySelfFail()
        {

            ATPClient client = new ATPClient();
            BskyAuthUser authUser = Utils.GetAuthUser();
            User sameUser = Utils.GetUser(blocked: true);
            sameUser.Did = authUser.Did;

            BskyActionResponse ActionResponseSameUser = client.UnblockUser(sameUser, authUser);

            Assert.NotNull(ActionResponseSameUser);
            Assert.NotEmpty(ActionResponseSameUser.ErrorMessage);
            Assert.Null(ActionResponseSameUser.Cid);
            Assert.Contains(ErrorMessage.SAME_DID_USER, ActionResponseSameUser.ErrorMessage);

        }

        [Fact]
        public void UnblockUserBlockedFailed()
        {

            ATPClient client = new ATPClient();
            BskyAuthUser authUser = Utils.GetAuthUser();
            User userBlocked = Utils.GetUser();

            BskyActionResponse ActionResponseUserBlocked = client.UnblockUser(userBlocked, authUser);

            Assert.NotNull(ActionResponseUserBlocked);
            Assert.NotEmpty(ActionResponseUserBlocked.ErrorMessage);
            Assert.Null(ActionResponseUserBlocked.Cid);
            Assert.Contains(ErrorMessage.USER_NOT_BLOCKED, ActionResponseUserBlocked.ErrorMessage);

        }

        [Fact]
        public void UnblockUserIsNullOrEmptyFail()
        {
            ATPClient client = new ATPClient();
            BskyAuthUser authUser = Utils.GetAuthUser();

            User userEmpty = new User();
            User userStringNull = new User { Viewer = new Viewer() };

            ArgumentNullException ActionResponseUserNull = Assert.Throws<ArgumentNullException>(() => client.BlockUser(null, authUser));
            ArgumentException ActionResponseUserStringNull = Assert.Throws<ArgumentException>(() => client.BlockUser(userStringNull, authUser));
            ArgumentException ActionResponseUserEmpty = Assert.Throws<ArgumentException>(() => client.BlockUser(userEmpty, authUser));

            Assert.NotNull(ActionResponseUserNull);
            Assert.NotEmpty(ActionResponseUserNull.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, ActionResponseUserNull.Message);

            Assert.NotNull(ActionResponseUserStringNull);
            Assert.NotEmpty(ActionResponseUserStringNull.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, ActionResponseUserStringNull.Message);

            Assert.NotNull(ActionResponseUserEmpty);
            Assert.NotEmpty(ActionResponseUserEmpty.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, ActionResponseUserEmpty.Message);
        }

        [Fact]
        public void GetBlocksSuccess()
        {
            BskyAuthUser authUser = Utils.GetAuthUser();
            List<Block> blocks = new List<Block>();
            blocks.Add(new Block());
            blocks.Add(new Block());
            blocks.Add(new Block());

            BskyBlockByResponse fakeResponse = new BskyBlockByResponse();
            fakeResponse.Blocks = blocks;
            var fakeActionJSON = JsonSerializer.Serialize(fakeResponse);

            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Get, fakeActionJSON);

            var client = new ATPClient(authUser, httpClient);

            List<Block> blocksResult = client.GetBlocks();

            Assert.NotNull(blocksResult);
            Assert.NotEmpty(blocksResult);
            Assert.Equal(3, blocksResult.Count);

        }

        [Fact]
        public void GetBlocksErrorFromBskyFail()
        {
            BskyAuthUser authUser = Utils.GetAuthUser();
            
            BskyBlockByResponse fakeResponse = new BskyBlockByResponse();
            fakeResponse.ErrorMessage = "Error message";
            var fakeActionJSON = JsonSerializer.Serialize(fakeResponse);

            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.InternalServerError, HttpMethod.Get, fakeActionJSON);

            var client = new ATPClient(authUser, httpClient);

            BskyException blocksResult = Assert.Throws<BskyException>(() => client.GetBlocks());

            Assert.NotNull(blocksResult);
            Assert.NotEmpty(blocksResult.Message);

        }

        [Fact]
        public void GetBlocksAuthNotConnectedOrNullFail()
        {

            var client = new ATPClient();

            BskyException authNull = Assert.Throws<BskyException>(() => client.GetBlocks(null));

            Assert.NotNull(authNull);
            Assert.NotEmpty(authNull.Message);
            Assert.Contains(ErrorMessage.USER_NOT_AUTHENTICATED, authNull.Message);

        }

        [Fact]
        public void GetBlocksWrongLimitFail()
        {
            BskyAuthUser authUser = Utils.GetAuthUser();

            var client = new ATPClient();

            ArgumentOutOfRangeException limitNegative = Assert.Throws<ArgumentOutOfRangeException>(() => client.GetBlocks(authUser, -50));
            ArgumentOutOfRangeException limitToHigh = Assert.Throws<ArgumentOutOfRangeException>(() => client.GetBlocks(authUser, 150));

            Assert.NotNull(limitNegative);
            Assert.NotEmpty(limitNegative.Message);
            Assert.Contains(ErrorMessage.LIMIT_NOT_SUPPORTED, limitNegative.Message);

            Assert.NotNull(limitToHigh);
            Assert.NotEmpty(limitToHigh.Message);
            Assert.Contains(ErrorMessage.LIMIT_NOT_SUPPORTED, limitToHigh.Message);

        }

        [Fact]
        public void GetAccountsBlockedSuccess()
        {
            BskyAuthUser authUser = Utils.GetAuthUser();
            User user = Utils.GetUser();

            List<Records> records = new List<Records>();
            records.Add(new Records());
            records.Add(new Records());
            
            ListRecordResponse fakeResponse = new ListRecordResponse();
            fakeResponse.Records = records;
            var fakeActionJSON = JsonSerializer.Serialize(fakeResponse);

            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Get, fakeActionJSON);

            var client = new ATPClient(authUser, httpClient);

            List<Records> recordsResult = client.GetAccountsBlocked(user);

            Assert.NotNull(recordsResult);
            Assert.NotEmpty(recordsResult);
            Assert.Equal(2, recordsResult.Count);

        }

        [Fact]
        public void GetAccountsBlockedErrorFromBskyFail()
        {
            BskyAuthUser authUser = Utils.GetAuthUser();
            User user = Utils.GetUser();

            ListRecordResponse fakeResponse = new ListRecordResponse();
            fakeResponse.ErrorMessage = "Error message";
            var fakeActionJSON = JsonSerializer.Serialize(fakeResponse);

            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.InternalServerError, HttpMethod.Get, fakeActionJSON);

            var client = new ATPClient(authUser, httpClient);

            BskyException recordsResult = Assert.Throws<BskyException>(() => client.GetAccountsBlocked(user));

            Assert.NotNull(recordsResult);
            Assert.NotEmpty(recordsResult.Message);

        }

    }
}
