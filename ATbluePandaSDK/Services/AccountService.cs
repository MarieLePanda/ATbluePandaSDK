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

namespace ATbluePandaSDK.Services
{
    public class AccountService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        /// <summary>
        /// Initializes a new instance of <see cref="AccountService"/> with a default HTTP client.
        /// Sets up the HTTP client with the base address defined in the application's configuration and a logger
        /// </summary>
        public AccountService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(Configuration.BaseUrl) };
            _logger = new Logger<AccountService>(new LoggerFactory());
        }

        /// <summary>
        /// Initializes a new instance of <see cref="AccountService"/> with a provided HTTP client.
        /// </summary>
        /// <param name="httpClient">An instance of <see cref="HttpClient"/> to use.</param>
        /// <param name="logger">An instance of <see cref="ILogger"/> to use.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="httpClient"/> or <paramref name="logger"/>is null.</exception>
        public AccountService(HttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient), "HttpClient cannot be null.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "ILogger cannot be null.");
        }

        /// <summary>
        /// Allows a user to follow another user.
        /// </summary>
        /// <param name="accessToken">The authentication token of the user.</param>
        /// <param name="didFollwer">The decentralized identifier (DID) of the follower.</param>
        /// <param name="didFollowee">The decentralized identifier (DID) of the user to follow.</param>
        /// <returns>An asynchronous task containing an <see cref="ActionResponse"/> indicating the result of the operation.</returns>
        public async Task<ActionResponse> FollowAsync(string accessToken, string didFollwer, string didFollowee)
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

            return await ATPUtils.SendRecordAsync(_httpClient, accessToken, recordData, Configuration.CreateRecord, _logger);
        }

        /// <summary>
        /// Allows a user to unfollow another user.
        /// </summary>
        /// <param name="accessToken">The authentication token of the user.</param>
        /// <param name="did">The decentralized identifier (DID) of the user.</param>
        /// <param name="followId">The unique identifier of the follow relationship to remove.</param>
        /// <returns>An asynchronous task containing an <see cref="ActionResponse"/> indicating the result of the operation.</returns>
        public async Task<ActionResponse> UnfollowAsync(string accessToken, string did, string followId)
        {
            var requestData = new
            {
                collection = Configuration.FollowCollection,
                repo = did,
                rkey = followId
            };

            return await ATPUtils.SendRecordAsync(_httpClient, accessToken, requestData, Configuration.DeleteRecord, _logger);
        }

    }
}
