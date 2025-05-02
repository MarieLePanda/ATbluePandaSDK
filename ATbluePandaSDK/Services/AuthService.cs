using ATPandaSDK.Models.Auth;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ATPandaSDK.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// Sets up the HTTP client with the base address defined in the application's configuration and a logger
        /// </summary>
        public AuthService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(Configuration.BaseUrl) };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="httpClient">An instance of <see cref="HttpClient"/> to use.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="httpClient"/> or <paramref name="logger"/>is null.</exception>
        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <summary>
        /// Authenticates a user by sending their username and password to the authentication service.
        /// </summary>
        /// <param name="authRequest">An object containing the username and password of the user attempting to authenticate.</param>
        /// <returns>
        /// An <see cref="BskyAuthUser"/> object containing the authenticated user. If authentication is successful,
        /// the response will include the authentication token and any other relevant user data. If authentication fails,
        /// the response will include an error message describing the failure.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="authRequest"/> is <c>null</c>.</exception>
        public async Task<BskyAuthUser> AuthenticateAsync(AuthRequest authRequest)
        {

            var loginPayload = new { identifier = authRequest.Username, password = authRequest.Password };
            string loginJson = JsonSerializer.Serialize(loginPayload);
            var loginContent = new StringContent(loginJson, Encoding.UTF8, "application/json");

            HttpResponseMessage loginResponse = await _httpClient.PostAsync(Configuration.Auth, loginContent);


            if (loginResponse.IsSuccessStatusCode)
            {
                string loginResult = await loginResponse.Content.ReadAsStringAsync();
                BskyAuthUser authUser = JsonSerializer.Deserialize<BskyAuthUser>(loginResult);
                return authUser;
            }

            Console.WriteLine($"Authentication failed : {loginResponse}");
            return new BskyAuthUser
            {
                ErrorMessage = loginResponse.ToString()
            };


        }
    }
}
