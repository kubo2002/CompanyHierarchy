using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using Tests.Api.TestClass;


namespace Tests.Api
{
    public class CreateEmployeeApiTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient client;

        public CreateEmployeeApiTests(CustomWebApplicationFactory factory)
        {
            client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateEmployee_Should_Return_400_When_Request_Is_Empty()
        {
            // Arrange
           

            var content = new StringContent("{}", Encoding.UTF8, "application/json"
            );

            // Act
            var response = await client.PostAsync("/api/createEmployee", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    

        [Fact]
        public async Task CreateEmployee_Should_Return_400_When_FirstName_Is_Missing()
        {
            var request = new
            {
                LastName = "Gubany",
                Email = "test@test.com"
            };

            var content = JsonContent.Create(request);

            var response = await client.PostAsync("/api/createEmployee", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
