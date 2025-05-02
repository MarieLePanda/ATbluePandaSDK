using ATPandaSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Security.Cryptography;
using ATbluePandaSDK.Models;
using Microsoft.Extensions.Logging;
using ATPandaSDK.Models.Feed;
using ATbluePandaSDK.Models.Account;

namespace ATbluePandaSDK.Services
{
    public class AccountService
    {
        private readonly HttpClient _httpClient;
        /// <summary>
        /// Initializes a new instance of <see cref="AccountService"/> with a default HTTP client.
        /// Sets up the HTTP client with the base address defined in the application's configuration and a logger
        /// </summary>
        public AccountService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(Configuration.BaseUrl) };
        }

        /// <summary>
        /// Initializes a new instance of <see cref="AccountService"/> with a provided HTTP client.
        /// </summary>
        /// <param name="httpClient">An instance of <see cref="HttpClient"/> to use.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="httpClient"/>is null.</exception>
        public AccountService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient), "HttpClient cannot be null.");
        }

        /// <summary>
        /// Allows a user to follow another user.
        /// </summary>
        /// <param name="accessToken">The authentication token of the user.</param>
        /// <param name="didFollwer">The decentralized identifier (DID) of the follower.</param>
        /// <param name="didFollowee">The decentralized identifier (DID) of the user to follow.</param>
        /// <returns>An asynchronous task containing an <see cref="BskyActionResponse"/> indicating the result of the operation.</returns>
        public async Task<BskyActionResponse> FollowAsync(string accessToken, string didFollwer, string didFollowee)
        {
            var recordData = new
            {
                collection = Configuration.FollowCollection,
                repo = didFollwer,
                record = new
                {
                    subject = didFollowee,
                    createdAt = DateTime.UtcNow.ToString("o")
                }
            };

            Response response = await ATPUtils.SendRecordAsync(_httpClient, accessToken, Configuration.CreateRecord, HttpMethod.Post, recordData);
            return ATPUtils.GetActionResponse(response);

        }

        /// <summary>
        /// Allows a user to unfollow another user.
        /// </summary>
        /// <param name="accessToken">The authentication token of the user.</param>
        /// <param name="did">The decentralized identifier (DID) of the user.</param>
        /// <param name="followId">The unique identifier of the follow relationship to remove.</param>
        /// <returns>An asynchronous task containing an <see cref="BskyActionResponse"/> indicating the result of the operation.</returns>
        public async Task<BskyActionResponse> UnfollowAsync(string accessToken, string did, string followId)
        {
            var requestData = new
            {
                collection = Configuration.FollowCollection,
                repo = did,
                rkey = followId
            };

            Response response = await ATPUtils.SendRecordAsync(_httpClient, accessToken, Configuration.DeleteRecord, HttpMethod.Post, requestData);
            return ATPUtils.GetActionResponse(response);

        }

        /// <summary>
        /// Allows a user to mute another user.
        /// </summary>
        /// <param name="accessToken">The authentication token of the user.</param>
        /// <param name="did">The decentralized identifier (DID) of the user.</param>
        /// <param name="muteDid">The unique identifier of the user to mute.</param>
        /// <returns>An asynchronous task containing an <see cref="BskyActionResponse"/> indicating the result of the operation.</returns>
        public async Task<BskyActionResponse> MuteUserAsync(string accessJwt, string did, string muteDid)
        {
            var recordData = new
            {
                actor = muteDid
            };

            Response response = await ATPUtils.SendRecordAsync(_httpClient, accessJwt, Configuration.MuteUser, HttpMethod.Post, recordData);
            return ATPUtils.GetActionResponse(response);
        }

        /// <summary>
        /// Allows a user to unmute another user.
        /// </summary>
        /// <param name="accessToken">The authentication token of the user.</param>
        /// <param name="did">The decentralized identifier (DID) of the user.</param>
        /// <param name="unMuteDid">The unique identifier of the user to unmute.</param>
        /// <returns>An asynchronous task containing an <see cref="BskyActionResponse"/> indicating the result of the operation.</returns>
        public async Task<BskyActionResponse> UnMuteUserAsync(string accessJwt, string did, string unMuteDid)
        {
            var recordData = new
            {
                actor = unMuteDid
            };

            Response response = await ATPUtils.SendRecordAsync(_httpClient, accessJwt, Configuration.UnMuteUser, HttpMethod.Post, recordData);
            return ATPUtils.GetActionResponse(response);
        }

        public async Task<UserProfileResponse> GetUserProfileAysnc(string accessJwt, string userDid)
        {
            var url = $"{Configuration.GetProfile}?actor={Uri.EscapeDataString(userDid)}";
            Response response = await ATPUtils.SendRecordAsync(_httpClient, accessJwt, url, HttpMethod.Get);
            if (response.isSuccess())
            {
                User user = JsonSerializer.Deserialize<User>(response.Result);
                UserProfileResponse userProfileResponse = new UserProfileResponse();
                userProfileResponse.BskyUser = user;
                userProfileResponse.StatusCode = response.StatusCode;
                return userProfileResponse;
            }
            else
            {
                return new UserProfileResponse(response);
            }
        }


    }
}
