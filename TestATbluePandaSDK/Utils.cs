using ATPandaSDK.Models.Auth;
using Moq.Protected;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATbluePandaSDK.Models;
using ATPandaSDK.Models.Feed;
using System.Net;
using System.Text.Json;
using ATbluePandaSDK.Models.Account;

namespace TestATbluePandaSDK
{
    public class Utils
    {
        public static BskyAuthUser GetAuthUser()
        {
            return new BskyAuthUser
            {
                Did = "did:plc:fakeDidUser",
                Handle = "BskyUser.handle",
                Email = "example@wow.com",
                EmailConfirmed = true,
                AccessJwt = "jdhufshfusmmuehfjsufshfsyfsfjsi,jiojuhsfThisIsAFakeAccessJwtifjsifewjflsufjsdflsdujfdsfjofuhsdjklsj",
                RefreshJwt = "ifjikjiufnclcmcnvamjfwpweivhyhgvalThisIsAFakeRefreshTokenkf[ewalc;oaijc8fuoajlikfuneryhcyanoucm",
                Active = true
            };
        }

        public static HttpClient getMockHTTPClient(HttpStatusCode status, HttpMethod method, string json)
        {

            var responseMessage = new HttpResponseMessage(status)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };


            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == method),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            var mockHttpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://api.example.com")
            };

            return mockHttpClient;
        }

        public static BskyActionResponse GetActionResponse()
        {
            CommitInfo commit = new CommitInfo { cid = "fefffdreidgnfzyy5kojfulfewhuiwe7hwehilszq2dulhosk6asmtrzyy", rev = "467euyi32w2k" };
            BskyActionResponse fakeAction = new BskyActionResponse
            {
                Cid = "467euyi32w2k",
                Commit = commit,
                Uri = "at://did:plc:dewdwfakeDidUrudwqqw/app.bsky.feed.post/467euyi32w2k",
                ValidationStatus = "valid",
                StatusCode = HttpStatusCode.OK
            };

            return fakeAction;
        }

        public static BskyActionResponse GetActionResponseError()
        {
            return new BskyActionResponse
            {
                ErrorMessage = "RANDOM ERROR",
                StatusCode = HttpStatusCode.BadRequest
            };

        }

        public static UserProfileResponse GetUserProfileResponse()
        {

            return new UserProfileResponse
            {
                BskyUser = GetUser(),
                StatusCode = HttpStatusCode.OK,
                ErrorMessage = null
            };
        }
        public static User GetUser()
        {
            return new User
            {
                Did = "did:plc:qwefakeDidUserdwqs",
                Handle = "BskyUser.handle",
                Avatar = "https://example.com/avatar.jpg",
                DisplayName = "BskyUser Name",
                Viewer = new Viewer()
            };

        }

        public static Post GetPost()
        {
            return new Post
            {
                Uri = "at://did:plc:fakeDidUser/app.bsky.feed.post/467euyi32w2k",
                Cid = "467euyi32w2k",
                Author = GetAuthor(),
                Record = new ATPandaSDK.Models.Feed.Record(),
                ReplyCount = 0,
                RepostCount = 0,
                LikeCount = 0,
                QuoteCount = 0,
                Viewer = new Viewer(),
                Labels = new List<Label>(),
                Embed = new Embed(),
            };
        }

        public static Author GetAuthor()
        {
            return new Author
            {
                Did = "did:plc:fakeDidUser",
                Handle = "BskyUser.handle",
                Avatar = "https://example.com/avatar.jpg",
                DisplayName = "BskyUser Name"
            };
        }
    }
}
