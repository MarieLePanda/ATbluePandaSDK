using ATbluePandaSDK;
using ATPandaSDK;
using ATPandaSDK.Models.Auth;
using ATPandaSDK.Models.Feed;
using ATbluePandaSDK.Models;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestATbluePandaSDK
{
    public class FeedTest
    {

        [Fact]
        public void GetTimelineSuccess()
        {
            BskyAuthUser authUser = Utils.GetAuthUser();
            BskyTimeline timelineResponse = new BskyTimeline();
            timelineResponse.Feed = new List<Feed>();

            var fakeActionJSON = JsonSerializer.Serialize(timelineResponse);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Get, fakeActionJSON);
            var client = new ATPClient(authUser, httpClient);

            BskyTimeline timeline = client.GetTimeline();
            Assert.NotNull(timeline);
            Assert.NotNull(timeline.Feed);
            Assert.Null(timeline.ErrorMessage);

        }

        [Fact]
        public void GetTimelineFail()
        {
            BskyAuthUser authUser = Utils.GetAuthUser();
            BskyTimeline timelineResponse = new BskyTimeline();
            timelineResponse.ErrorMessage = "Error message";

            var fakeActionJSON = JsonSerializer.Serialize(timelineResponse);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.ServiceUnavailable, HttpMethod.Get, fakeActionJSON);
            var client = new ATPClient(authUser, httpClient);

            BskyTimeline timeline = client.GetTimeline();
            Assert.NotNull(timeline);
            Assert.Null(timeline.Feed);
            Assert.NotEmpty(timeline.ErrorMessage);

        }

        [Fact]
        public void GetTimelineWrongArgumentFail()
        {
            //Arrange
            BskyAuthUser authUser = Utils.GetAuthUser();

            var client = new ATPClient();

            //Act
            BskyException timelineUserNull = Assert.Throws<BskyException>(() => client.GetTimeline(auth: null));
            ArgumentOutOfRangeException timelineNegativeLimit = Assert.Throws<ArgumentOutOfRangeException>(() => client.GetTimeline(-30, auth: authUser));

            //Assert
            Assert.NotNull(timelineUserNull);
            Assert.NotEmpty(timelineUserNull.Message);
            Assert.Contains(ErrorMessage.USER_NOT_AUTHENTICATED, timelineUserNull.Message);

            Assert.NotNull(timelineNegativeLimit);
            Assert.NotEmpty(timelineNegativeLimit.Message);
            Assert.Contains(ErrorMessage.LIMIT_NOT_SUPPORTED, timelineNegativeLimit.Message);

        }

        [Fact]
        public void GetCustomTimelineSuccess()
        {
            BskyAuthUser authUser = Utils.GetAuthUser();
            BskyTimeline timelineResponse = new BskyTimeline();
            timelineResponse.Feed = new List<Feed>();

            var fakeActionJSON = JsonSerializer.Serialize(timelineResponse);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Get, fakeActionJSON);
            var client = new ATPClient(authUser, httpClient);

            BskyTimeline timeline = client.GetCustomTimeline("at://did:plc:z72i7hdynmk6r22z27h6tvur/app.bsky.feed.generator/whats-hot");
            
            Assert.NotNull(timeline);
            Assert.NotNull(timeline.Feed);
            Assert.Null(timeline.ErrorMessage);

        }

        [Fact]
        public void GetCustomTimelineFail()
        {
            BskyAuthUser authUser = Utils.GetAuthUser();
            BskyTimeline timelineResponse = new BskyTimeline();
            timelineResponse.ErrorMessage = "Error message";

            var fakeActionJSON = JsonSerializer.Serialize(timelineResponse);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.ServiceUnavailable, HttpMethod.Get, fakeActionJSON);
            var client = new ATPClient(authUser, httpClient);

            BskyTimeline timeline = client.GetCustomTimeline("at://did:plc:z72i7hdynmk6r22z27h6tvur/app.bsky.feed.generator/whats-hot");
            Assert.NotNull(timeline);
            Assert.Null(timeline.Feed);
            Assert.NotEmpty(timeline.ErrorMessage);

        }

        [Fact]
        public void GetCustomTimelineWrongArgumentFail()
        {
            BskyAuthUser authUser = Utils.GetAuthUser();


            var client = new ATPClient();

            BskyException timelineUserNull = Assert.Throws<BskyException>(() => client.GetCustomTimeline("at://did:plc:z72i7hdynmk6r22z27h6tvur/app.bsky.feed.generator/whats-hot", auth: null));
            ArgumentNullException timelineFeedNull = Assert.Throws<ArgumentNullException>(() => client.GetCustomTimeline("", auth:authUser));
            ArgumentOutOfRangeException timelineNegativeLimit = Assert.Throws<ArgumentOutOfRangeException>(() => client.GetCustomTimeline("at://did:plc:z72i7hdynmk6r22z27h6tvur/app.bsky.feed.generator/whats-hot", -30, auth:authUser));

            Assert.NotNull(timelineUserNull);
            Assert.NotEmpty(timelineUserNull.Message);
            Assert.Contains(ErrorMessage.USER_NOT_AUTHENTICATED, timelineUserNull.Message);

            Assert.NotNull(timelineFeedNull);
            Assert.NotEmpty(timelineFeedNull.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, timelineFeedNull.Message);

            Assert.NotNull(timelineNegativeLimit);
            Assert.NotEmpty(timelineNegativeLimit.Message);
            Assert.Contains(ErrorMessage.LIMIT_NOT_SUPPORTED, timelineNegativeLimit.Message);

        }

        [Fact]
        public void GetAuthorTimelineSuccess()
        {
            BskyAuthUser authUser = Utils.GetAuthUser();
            BskyTimeline timelineResponse = new BskyTimeline();
            timelineResponse.Feed = new List<Feed>();

            var fakeActionJSON = JsonSerializer.Serialize(timelineResponse);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Get, fakeActionJSON);
            var client = new ATPClient(authUser, httpClient);

            BskyTimeline timeline = client.GetAuthorTimeline();

            Assert.NotNull(timeline);
            Assert.NotNull(timeline.Feed);
            Assert.Null(timeline.ErrorMessage);

        }

        [Fact]
        public void GetAuthorTimelineFail()
        {
            BskyAuthUser authUser = Utils.GetAuthUser();
            BskyTimeline timelineResponse = new BskyTimeline();
            timelineResponse.ErrorMessage = "Error message";

            var fakeActionJSON = JsonSerializer.Serialize(timelineResponse);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.ServiceUnavailable, HttpMethod.Get, fakeActionJSON);
            var client = new ATPClient(authUser, httpClient);

            BskyTimeline timeline = client.GetAuthorTimeline();
            Assert.NotNull(timeline);
            Assert.Null(timeline.Feed);
            Assert.NotEmpty(timeline.ErrorMessage);

        }

        [Fact]
        public void GetAuthorTimelineWrongArgumentFail()
        {
            BskyAuthUser authUser = Utils.GetAuthUser();


            var client = new ATPClient();


            BskyException timelineUserNull = Assert.Throws<BskyException>(() => client.GetAuthorTimeline(auth:null));
            ArgumentOutOfRangeException timelineNegativeLimit = Assert.Throws<ArgumentOutOfRangeException>(() => client.GetAuthorTimeline(-30, auth:authUser));

            Assert.NotNull(timelineUserNull);
            Assert.NotEmpty(timelineUserNull.Message);
            Assert.Contains(ErrorMessage.USER_NOT_AUTHENTICATED, timelineUserNull.Message);

            Assert.NotNull(timelineNegativeLimit);
            Assert.NotEmpty(timelineNegativeLimit.Message);
            Assert.Contains(ErrorMessage.LIMIT_NOT_SUPPORTED, timelineNegativeLimit.Message);

        }
    }
}