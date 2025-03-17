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
        /// A task containing an <see cref="ActionResponse"/> representing the result of the request.
        /// </returns>
        public static async Task<ActionResponse> SendRecordAsync(HttpClient httpClient, string accessToken, object requestData, string url, ILogger logger)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

            logger.LogInformation("Send data to : {Url}", url);
            logger.LogInformation("Data sent : {RequestData}", JsonSerializer.Serialize(requestData));

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage response = await httpClient.PostAsync(url, jsonContent);
            logger.LogInformation("Status code : {StatusCode}", response.StatusCode);
            ActionResponse actionResponse = null;
            if (response.IsSuccessStatusCode)
            {

                string postResult = await response.Content.ReadAsStringAsync();
                logger.LogInformation("Response content : {ResponseContent}", postResult);
                if(string.IsNullOrEmpty(postResult))
                {
                    actionResponse = new ActionResponse();
                }
                else
                {
                    actionResponse = JsonSerializer.Deserialize<ActionResponse>(postResult);
                }
            }
            else
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                logger.LogInformation("Response content : {ResponseContent}", errorResponse);

                actionResponse =  new ActionResponse
                {
                    ErrorMessage = errorResponse
                };
            }

            actionResponse.StatusCode = response.StatusCode;
            return actionResponse;
        }
    }
}
