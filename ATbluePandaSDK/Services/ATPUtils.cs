using ATbluePandaSDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ATbluePandaSDK.Services
{
    public class ATPUtils
    {
        /// <summary>
        /// Sends a record to the specified API endpoint.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> instance used for sending the request.</param>
        /// <param name="accessToken">The authentication token for authorization.</param>
        /// <param name="requestData">The data payload to be sent in the request body.</param>
        /// <param name="url">The target API endpoint URL.</param>
        /// <returns>
        /// A task containing an <see cref="BskyActionResponse"/> representing the result of the request.
        /// </returns>
        public static async Task<Response> SendRecordAsync(HttpClient httpClient, string accessToken, string url, HttpMethod method, object requestData = null)
        {
            StringContent? jsonContent = null;
            if (requestData != null)
            {
                jsonContent = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");
            }

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage? response = null;
            if (method.Equals(HttpMethod.Post))
            {
                response = await httpClient.PostAsync(url, jsonContent);
            }
            else if (method.Equals(HttpMethod.Get))
            {

                response = await httpClient.GetAsync(url);
            }
            else
            {
                throw new NotSupportedException($"HTTP method {method} is not supported.");
            }
            Response? actionResponse;

            if (response == null)
            {
                actionResponse = new Response()
                {
                    ErrorMessage = "No response from server."
                };
                return actionResponse;
            }
            if (response.IsSuccessStatusCode)
            {
                string postResult = await response.Content.ReadAsStringAsync();
                actionResponse = new Response();

                if (!string.IsNullOrEmpty(postResult))
                {
                    actionResponse.Result = postResult;
                }
            }
            else
            {
                string errorResponse = await response.Content.ReadAsStringAsync();

                actionResponse =  new Response
                {
                    ErrorMessage = errorResponse
                };
            }

            actionResponse.StatusCode = response.StatusCode;
            return actionResponse;
        }

        public static BskyActionResponse GetActionResponse(Response response)
        {
            if (response.isSuccess())
            {
                BskyActionResponse actionResponse = JsonSerializer.Deserialize<BskyActionResponse>(response.Result);
                actionResponse.StatusCode = response.StatusCode;
                return actionResponse;
            }
            else
            {
                return new BskyActionResponse(response);
            }
        }
    }
}
