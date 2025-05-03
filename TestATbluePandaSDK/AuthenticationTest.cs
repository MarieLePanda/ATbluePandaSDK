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
            BskyAuthUser authUser = Utils.GetAuthUser();
            var authRequest = new AuthRequest("BskyUser", "password");
            var authUserJson = JsonSerializer.Serialize(authUser);

            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.OK, HttpMethod.Post, authUserJson);
            var client = new ATPClient(null, httpClient);

            //Act
            BskyAuthUser user = client.Authenticate(authRequest);

            //Verify
            Assert.NotNull(user);
            Assert.Equal(authUser.Did, user.Did);
            Assert.Null(user.ErrorMessage);
        }

        [Fact]
        public void AuthentificationFail()
        {
            // Arrange
            var authRequest = new AuthRequest("BskyUser", "password");
            var authUser = new BskyAuthUser
            {
                ErrorMessage = "Unauthorized"
            };

            var authUserJson = JsonSerializer.Serialize(authUser);


            HttpClient httpClient = Utils.getMockHTTPClient(HttpStatusCode.Unauthorized, HttpMethod.Post, authUserJson);
            var client = new ATPClient(null, httpClient);

            //Act
            BskyAuthUser user = client.Authenticate(authRequest);

            //Verify
            Assert.NotNull(user);
            Assert.Null(user.Did);
            Assert.NotEmpty(user.ErrorMessage);
        }

        [Fact]
        public void AuthentificationWithEmptyCredentialFail()
        {
            // Arrange
            AuthRequest authRequestNull = null;
            AuthRequest authRequestWithoutUser = new AuthRequest("", "password");
            AuthRequest authRequestWithoutPassword = new AuthRequest("userNull", "");
            AuthRequest authRequestWithoutUserAndPassword = new AuthRequest("", "");


            var client = new ATPClient();

            //Act
            ArgumentNullException userNull = Assert.Throws<ArgumentNullException>(() => client.Authenticate(authRequestNull));
            ArgumentException userWithoutUser = Assert.Throws<ArgumentException>(() => client.Authenticate(authRequestWithoutUser));
            ArgumentException userWithoutPassword = Assert.Throws<ArgumentException>(() => client.Authenticate(authRequestWithoutPassword));
            ArgumentException userWithoutUserAndPassword = Assert.Throws<ArgumentException>(() => client.Authenticate(authRequestWithoutUserAndPassword));

            //Verify
            Assert.NotNull(userNull);
            Assert.NotEmpty(userNull.Message);
            Assert.Contains(ErrorMessage.ARG_IS_NULL, userNull.Message);

            Assert.NotNull(userWithoutUser);
            Assert.NotEmpty(userWithoutUser.Message);
            Assert.Contains(ErrorMessage.ARG_IS_INVALID, userWithoutUser.Message);

            Assert.NotNull(userWithoutPassword);
            Assert.NotEmpty(userWithoutPassword.Message);
            Assert.Contains(ErrorMessage.ARG_IS_INVALID, userWithoutPassword.Message);

            Assert.NotNull(userWithoutUserAndPassword);
            Assert.NotEmpty(userWithoutUserAndPassword.Message);
            Assert.Contains(ErrorMessage.ARG_IS_INVALID, userWithoutUserAndPassword.Message);

        }
    }
}