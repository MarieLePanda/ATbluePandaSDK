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
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostService"/> class.
        /// Sets up the HTTP client with the base address defined in the application's configuration and a logger
        /// </summary>
        public PostService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(Configuration.BaseUrl) };
            _logger = new Logger<PostService>(new LoggerFactory());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PostService"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client used for making API requests.</param>
        /// <param name="logger">An instance of <see cref="ILogger"/> to use.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="httpClient"/> or <paramref name="logger"/>is null.</exception>
        public PostService(HttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient), "HttpClient cannot be null.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "ILogger cannot be null.");
            _logger.LogDebug("PostService initialized");
        }

        /// <summary>
        /// Likes a post by creating a like record.
        /// </summary>
        /// <param name="accessToken">The authentication token for authorization.</param>
        /// <param name="did">The decentralized identifier (DID) of the user liking the post.</param>
        /// <param name="postUri">The URI of the post to like.</param>
        /// <param name="postCid">The content ID (CID) of the post.</param>
        /// <returns>A task containing an <see cref="ActionResponse"/> indicating the result of the operation.</returns>
        public async Task<ActionResponse> LikePostAsync(string accessToken, string did, string postUri, string postCid)
        {
            _logger.LogInformation("LikePost function");
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

            return await ATPUtils.SendRecordAsync(_httpClient, accessToken, requestData, Configuration.CreateRecord, _logger);
        }

        /// <summary>
        /// Removes a like from a post by deleting the like record.
        /// </summary>
        /// <param name="accessToken">The authentication token for authorization.</param>
        /// <param name="did">The decentralized identifier (DID) of the user unliking the post.</param>
        /// <param name="likeId">The identifier of the like record to delete.</param>
        /// <returns>A task containing an <see cref="ActionResponse"/> indicating the result of the operation.</returns>
        public async Task<ActionResponse> UnlikePostAsync(string accessToken, string did, string likeId)
        {           
            var requestData = new
            {
                collection = Configuration.LikeCollection,
                repo = did, 
                rkey = likeId
            };

            return await ATPUtils.SendRecordAsync(_httpClient, accessToken, requestData, Configuration.DeleteRecord, _logger);
        }

        /// <summary>
        /// Retrieves a threaded conversation for a given post.
        /// </summary>
        /// <param name="accessToken">The authentication token for authorization.</param>
        /// <param name="postUri">The URI of the post for which to fetch the thread.</param>
        /// <param name="depth">The depth of the thread to retrieve.</param>
        /// <returns>A task containing a <see cref="ThreadResponse"/> representing the post thread.</returns>
        public async Task<ThreadResponse> GetPostThreadAsync(string accessToken, string postUri, int depth)
        {
            var url = $"{Configuration.GetPostThread}?uri={Uri.EscapeDataString(postUri)}&depth={depth}";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                string jsonResult = await response.Content.ReadAsStringAsync();
                ThreadResponse thread = JsonSerializer.Deserialize<ThreadResponse>(jsonResult);
                return thread;
            }
            else
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                return new ThreadResponse
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
        /// <returns>A task containing an <see cref="ActionResponse"/> indicating the result of the operation.</returns>
        public async Task<ActionResponse> CreatePostAsync(string accessToken, string did, string text)
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

            return await ATPUtils.SendRecordAsync(_httpClient, accessToken, payload, Configuration.CreateRecord, _logger);
        }


    }
}
