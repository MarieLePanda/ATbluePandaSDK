using ATbluePandaSDK.Models;
using ATbluePandaSDK.Services;
using ATPandaSDK.Models.Feed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ATPandaSDK.Services
{
    public class PostService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostService"/> class.
        /// Sets up the HTTP client with the base address defined in the application's configuration and a logger
        /// </summary>
        public PostService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(Configuration.BaseUrl) };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PostService"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client used for making API requests.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="httpClient"/> is null.</exception>
        public PostService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient), "HttpClient cannot be null.");
        }

        /// <summary>
        /// Likes a post by creating a like record.
        /// </summary>
        /// <param name="accessToken">The authentication token for authorization.</param>
        /// <param name="did">The decentralized identifier (DID) of the user liking the post.</param>
        /// <param name="postUri">The URI of the post to like.</param>
        /// <param name="postCid">The content ID (CID) of the post.</param>
        /// <returns>A task containing an <see cref="BskyActionResponse"/> indicating the result of the operation.</returns>
        public async Task<BskyActionResponse> LikePostAsync(string accessToken, string did, string postUri, string postCid)
        {
            var requestData = new
            {
                collection = Configuration.LikeCollection,
                repo = did, 
                record = new
                {
                    subject = new
                    {
                        uri = postUri,
                        cid = postCid
                    },
                    createdAt = DateTime.UtcNow.ToString("o") // ISO 8601 format
                }
            };

            Response response = await ATPUtils.SendRecordAsync(_httpClient, accessToken, Configuration.CreateRecord, HttpMethod.Post, requestData);
            return ATPUtils.GetActionResponse(response);

        }

        /// <summary>
        /// Removes a like from a post by deleting the like record.
        /// </summary>
        /// <param name="accessToken">The authentication token for authorization.</param>
        /// <param name="did">The decentralized identifier (DID) of the user unliking the post.</param>
        /// <param name="likeId">The identifier of the like record to delete.</param>
        /// <returns>A task containing an <see cref="BskyActionResponse"/> indicating the result of the operation.</returns>
        public async Task<BskyActionResponse> UnlikePostAsync(string accessToken, string did, string likeId)
        {           
            var requestData = new
            {
                collection = Configuration.LikeCollection,
                repo = did, 
                rkey = likeId
            };

            Response response = await ATPUtils.SendRecordAsync(_httpClient, accessToken, Configuration.DeleteRecord, HttpMethod.Post, requestData);
            return ATPUtils.GetActionResponse(response);

        }

        /// <summary>
        /// Retrieves a threaded conversation for a given post.
        /// </summary>
        /// <param name="accessToken">The authentication token for authorization.</param>
        /// <param name="postUri">The URI of the post for which to fetch the thread.</param>
        /// <param name="depth">The depth of the thread to retrieve.</param>
        /// <returns>A task containing a <see cref="BskyThread"/> representing the post thread.</returns>
        public async Task<BskyThread> GetPostThreadAsync(string accessToken, string postUri, int depth)
        {
            var url = $"{Configuration.GetPostThread}?uri={Uri.EscapeDataString(postUri)}&depth={depth}";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonResult = await response.Content.ReadAsStringAsync();
                BskyThread thread = JsonSerializer.Deserialize<BskyThread>(jsonResult);
                return thread;
            }
            else
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                return new BskyThread
                {
                    ErrorMessage = errorResponse
                };
            }
        }

        /// <summary>
        /// Creates a new post.
        /// </summary>
        /// <param name="accessToken">The authentication token for authorization.</param>
        /// <param name="did">The decentralized identifier (DID) of the user creating the post.</param>
        /// <param name="text">The text content of the post.</param>
        /// <returns>A task containing an <see cref="BskyActionResponse"/> indicating the result of the operation.</returns>
        public async Task<BskyActionResponse> CreatePostAsync(string accessToken, string did, string text)
        {
            
            var payload = new
            {
                collection = Configuration.Feed,
                repo = did,
                record = new
                {
                    text = text,
                    createdAt = DateTime.UtcNow.ToString("o")
                }
            };

            string postJson = JsonSerializer.Serialize(payload);
            var postContent = new StringContent(postJson, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage postResponse = await _httpClient.PostAsync($"{Configuration.CreateRecord}", postContent);

            Response response = await ATPUtils.SendRecordAsync(_httpClient, accessToken, Configuration.CreateRecord, HttpMethod.Post, payload);
            return ATPUtils.GetActionResponse(response);

        }


    }
}
