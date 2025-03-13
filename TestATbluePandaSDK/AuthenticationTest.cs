using ATbluePandaSDK;
using ATbluePandaSDK.Models;
using ATbluePandaSDK.Services;
using ATPandaSDK;
using ATPandaSDK.Models.Auth;
using ATPandaSDK.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;

namespace TestATbluePandaSDK
{
    public class AuthenticationTest
    {
        [Fact]
        public void AuthentificationSuccess()
        {
            // Arrange
            AuthUser authUser = Utils.GetAuthUser();
            var authRequest = new AuthRequest("User", "password");
            var authUserJson = JsonSerializer.Serialize(authUser);

            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Post, authUserJson);
            var client = new ATPClient(httpClient);

            //Act
            AuthUser user = client.Authenticate(authRequest);

            //Verify
            Assert.NotNull(user);
            Assert.Equal(authUser.Did, user.Did);
            Assert.Null(user.ErrorMessage);
        }

        [Fact]
        public void AuthentificationFail()
        {
            // Arrange
            var authRequest = new AuthRequest("User", "password");
            var authUser = new AuthUser
            {
                ErrorMessage = "Unauthorized"
            };

            var authUserJson = JsonSerializer.Serialize(authUser);


            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.Unauthorized, HttpMethod.Post, authUserJson);
            var client = new ATPClient(httpClient);

            //Act
            AuthUser user = client.Authenticate(authRequest);

            //Verify
            Assert.NotNull(user);
            Assert.Null(user.Did);
            Assert.NotEmpty(user.ErrorMessage);
        }

        [Fact]
        public void AuthentificationWithEmptyCredentialFail()
        {
            // Arrange
            AuthRequest authRequest = null;
            AuthRequest authRequestWithoutUser = new AuthRequest("", "password");
            AuthRequest authRequestWithoutPassword = new AuthRequest("user", "");
            AuthRequest authRequestWithoutUserAndPassword = new AuthRequest("", "");


            var client = new ATPClient();

            //Act
            AuthUser user = client.Authenticate(authRequest);
            AuthUser userWithoutUser = client.Authenticate(authRequestWithoutUser);
            AuthUser userWithoutPassword = client.Authenticate(authRequestWithoutPassword);
            AuthUser userWithoutUserAndPassword = client.Authenticate(authRequestWithoutUserAndPassword);


            //Verify
            Assert.NotNull(user);
            Assert.Null(user.Did);
            Assert.NotEmpty(user.ErrorMessage);
            Assert.Equal(ErrorMessage.AUTH_REQUEST_IS_NULL, user.ErrorMessage);

            Assert.NotNull(userWithoutUser);
            Assert.NotNull(userWithoutPassword);
            Assert.NotNull(userWithoutUserAndPassword);

            Assert.Null(userWithoutUser.Did);
            Assert.Null(userWithoutPassword.Did);
            Assert.Null(userWithoutUserAndPassword.Did);

            Assert.NotEmpty(userWithoutPassword.ErrorMessage);
            Assert.NotEmpty(userWithoutUser.ErrorMessage);
            Assert.NotEmpty(userWithoutUserAndPassword.ErrorMessage);

            Assert.Equal(ErrorMessage.USER_NAME_OR_PASSWORD_IS_NULL, userWithoutUser.ErrorMessage);
            Assert.Equal(ErrorMessage.USER_NAME_OR_PASSWORD_IS_NULL, userWithoutPassword.ErrorMessage);
            Assert.Equal(ErrorMessage.USER_NAME_OR_PASSWORD_IS_NULL, userWithoutUserAndPassword.ErrorMessage);

        }       
    }
}