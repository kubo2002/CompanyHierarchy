
using CompanyManagement.Api.Responses;
using CompanyManagement.Application.DTOs;
using System.Net;
using System.Net.Http.Json;
using Tests.Api.TestClass;

namespace Tests.Api
{
    public class GetNodesByTypeTests
    : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public GetNodesByTypeTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }
        // vrati 400 ak je neplatny typ
        [Fact]
        public async Task GetNodesByType_Should_Return_400_When_Type_Is_Invalid()
        {
            // Act
            var response = await _client.GetAsync("/api/nodes/by-type?types=InvalidType");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var body = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();

            Assert.NotNull(body);
            Assert.False(body!.Success);
            Assert.NotNull(body.Message);
        }
    }
}
