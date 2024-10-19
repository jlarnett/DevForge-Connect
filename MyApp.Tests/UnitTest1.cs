using System.Net;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;


namespace MyApp.Tests
{
    public class PageAccessibilityTests : IClassFixture<WebApplicationFactory<DevForge_Connect.Startup>>
    {
        private readonly WebApplicationFactory<DevForge_Connect.Startup> _factory;

        public PageAccessibilityTests(WebApplicationFactory<DevForge_Connect.Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Home/Privacy")]
        [InlineData("/ProjectSubmissions")]
        [InlineData("/ProjectBids")]
        [InlineData("/Status")]
        [InlineData("/Teams")]
        [InlineData("/UserTeams")]
        [InlineData("/Administration/CreateRole")]
        [InlineData("/UserProfiles")]
        [InlineData("/Identity/Account/Register")]


        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}