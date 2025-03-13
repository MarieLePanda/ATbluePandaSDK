using ATbluePandaSDK;
using ATPandaSDK;
using ATPandaSDK.Models.Auth;
using ATPandaSDK.Models.Feed;
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
            AuthUser authUser = Utils.GetAuthUser();
            TimelineResponse timelineResponse = new TimelineResponse();
            timelineResponse.Feed = new List<Feed>();

            var fakeActionJSON = JsonSerializer.Serialize(timelineResponse);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Get, fakeActionJSON);
            var client = new ATPClient(httpClient);

            TimelineResponse timeline = client.GetTimeline(authUser);
            Assert.NotNull(timeline);
            Assert.NotNull(timeline.Feed);
            Assert.Null(timeline.ErrorMessage);

        }

        [Fact]
        public void GetTimelineFail()
        {
            AuthUser authUser = Utils.GetAuthUser();
            TimelineResponse timelineResponse = new TimelineResponse();
            timelineResponse.ErrorMessage = "Error message";

            var fakeActionJSON = JsonSerializer.Serialize(timelineResponse);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.ServiceUnavailable, HttpMethod.Get, fakeActionJSON);
            var client = new ATPClient(httpClient);

            TimelineResponse timeline = client.GetTimeline(authUser);
            Assert.NotNull(timeline);
            Assert.Null(timeline.Feed);
            Assert.NotEmpty(timeline.ErrorMessage);

        }

        [Fact]
        public void GetTimelineWrongArgumentFail()
        {
            AuthUser authUser = Utils.GetAuthUser();

            
            var client = new ATPClient();

            TimelineResponse timelineUserNull = client.GetTimeline(null);
            TimelineResponse timelineNegativeLimit = client.GetTimeline(authUser, -30);

            Assert.NotNull(timelineUserNull);
            Assert.Null(timelineUserNull.Feed);
            Assert.NotEmpty(timelineUserNull.ErrorMessage);
            Assert.Equal(ErrorMessage.USER_NOT_AUTHENTICATED, timelineUserNull.ErrorMessage);

            Assert.NotNull(timelineNegativeLimit);
            Assert.Null(timelineNegativeLimit.Feed);
            Assert.NotEmpty(timelineNegativeLimit.ErrorMessage);
            Assert.Equal(ErrorMessage.LIMIT_IS_NEGATIVE, timelineNegativeLimit.ErrorMessage);

        }

        [Fact]
        public void GetCustomTimelineSuccess()
        {
            AuthUser authUser = Utils.GetAuthUser();
            TimelineResponse timelineResponse = new TimelineResponse();
            timelineResponse.Feed = new List<Feed>();

            var fakeActionJSON = JsonSerializer.Serialize(timelineResponse);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Get, fakeActionJSON);
            var client = new ATPClient(httpClient);

            TimelineResponse timeline = client.GetCustomTimeline(authUser, "at://did:plc:z72i7hdynmk6r22z27h6tvur/app.bsky.feed.generator/whats-hot");
            
            Assert.NotNull(timeline);
            Assert.NotNull(timeline.Feed);
            Assert.Null(timeline.ErrorMessage);

        }

        [Fact]
        public void GetCustomTimelineFail()
        {
            AuthUser authUser = Utils.GetAuthUser();
            TimelineResponse timelineResponse = new TimelineResponse();
            timelineResponse.ErrorMessage = "Error message";

            var fakeActionJSON = JsonSerializer.Serialize(timelineResponse);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.ServiceUnavailable, HttpMethod.Get, fakeActionJSON);
            var client = new ATPClient(httpClient);

            TimelineResponse timeline = client.GetCustomTimeline(authUser, "at://did:plc:z72i7hdynmk6r22z27h6tvur/app.bsky.feed.generator/whats-hot");
            Assert.NotNull(timeline);
            Assert.Null(timeline.Feed);
            Assert.NotEmpty(timeline.ErrorMessage);

        }

        [Fact]
        public void GetCustomTimelineWrongArgumentFail()
        {
            AuthUser authUser = Utils.GetAuthUser();


            var client = new ATPClient();

            TimelineResponse timelineUserNull = client.GetCustomTimeline(null, "at://did:plc:z72i7hdynmk6r22z27h6tvur/app.bsky.feed.generator/whats-hot");
            TimelineResponse timelineFeedNull = client.GetCustomTimeline(authUser, "");
            TimelineResponse timelineNegativeLimit = client.GetCustomTimeline(authUser, "at://did:plc:z72i7hdynmk6r22z27h6tvur/app.bsky.feed.generator/whats-hot", -30);

            Assert.NotNull(timelineUserNull);
            Assert.Null(timelineUserNull.Feed);
            Assert.NotEmpty(timelineUserNull.ErrorMessage);
            Assert.Equal(ErrorMessage.USER_NOT_AUTHENTICATED, timelineUserNull.ErrorMessage);

            Assert.NotNull(timelineFeedNull);
            Assert.Null(timelineFeedNull.Feed);
            Assert.NotEmpty(timelineFeedNull.ErrorMessage);
            Assert.Equal(ErrorMessage.FEED_IS_NULL, timelineFeedNull.ErrorMessage);

            Assert.NotNull(timelineNegativeLimit);
            Assert.Null(timelineNegativeLimit.Feed);
            Assert.NotEmpty(timelineNegativeLimit.ErrorMessage);
            Assert.Equal(ErrorMessage.LIMIT_IS_NEGATIVE, timelineNegativeLimit.ErrorMessage);

        }

        [Fact]
        public void GetAuthorTimelineSuccess()
        {
            AuthUser authUser = Utils.GetAuthUser();
            TimelineResponse timelineResponse = new TimelineResponse();
            timelineResponse.Feed = new List<Feed>();

            var fakeActionJSON = JsonSerializer.Serialize(timelineResponse);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Get, fakeActionJSON);
            var client = new ATPClient(httpClient);

            TimelineResponse timeline = client.GetAuthorTimeline(authUser);

            Assert.NotNull(timeline);
            Assert.NotNull(timeline.Feed);
            Assert.Null(timeline.ErrorMessage);

        }

        [Fact]
        public void GetAuthorTimelineFail()
        {
            AuthUser authUser = Utils.GetAuthUser();
            TimelineResponse timelineResponse = new TimelineResponse();
            timelineResponse.ErrorMessage = "Error message";

            var fakeActionJSON = JsonSerializer.Serialize(timelineResponse);
            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.ServiceUnavailable, HttpMethod.Get, fakeActionJSON);
            var client = new ATPClient(httpClient);

            TimelineResponse timeline = client.GetAuthorTimeline(authUser);
            Assert.NotNull(timeline);
            Assert.Null(timeline.Feed);
            Assert.NotEmpty(timeline.ErrorMessage);

        }

        [Fact]
        public void GetAuthorTimelineWrongArgumentFail()
        {
            AuthUser authUser = Utils.GetAuthUser();


            var client = new ATPClient();

            TimelineResponse timelineUserNull = client.GetAuthorTimeline(null);
            TimelineResponse timelineNegativeLimit = client.GetAuthorTimeline(authUser, -30);

            Assert.NotNull(timelineUserNull);
            Assert.Null(timelineUserNull.Feed);
            Assert.NotEmpty(timelineUserNull.ErrorMessage);
            Assert.Equal(ErrorMessage.USER_NOT_AUTHENTICATED, timelineUserNull.ErrorMessage);

            Assert.NotNull(timelineNegativeLimit);
            Assert.Null(timelineNegativeLimit.Feed);
            Assert.NotEmpty(timelineNegativeLimit.ErrorMessage);
            Assert.Equal(ErrorMessage.LIMIT_IS_NEGATIVE, timelineNegativeLimit.ErrorMessage);

        }
    }
}