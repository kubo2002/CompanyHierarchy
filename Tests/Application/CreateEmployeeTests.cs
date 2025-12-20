using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Application.DTOs;
using CompanyManagement.Application.UseCases;
using CompanyManagement.Domain.Entities;
using Moq;

namespace Tests.Application
{

    public class CreateEmployeeTests
    {
        [Fact]
        public async Task ExecuteAsync_Should_Create_Employee_And_Save_It()
        {
            // Arrange
            var repoMock = new Mock<IEmployeeRepository>();
            var useCase = new CreateEmployee(repoMock.Object);

            var request = new CreateEmployeeRequest
            {
                FirstName = "Jakub",
                LastName = "Gubany",
                Email = "jakub.gubany@test.com",
                Phone = "+421900123456"
            };

            // Act
            var resultId = await useCase.ExecuteAsync(request);

            // Assert
            Assert.NotEqual(Guid.Empty, resultId);

            repoMock.Verify(
                r => r.AddAsync(It.Is<Employee>(e =>
                    e.FirstName == "Jakub" &&
                    e.LastName == "Gubany" &&
                    e.Email == "jakub.gubany@test.com" &&
                    e.Phone == "+421900123456"
                )),
                Times.Once
            );
        }

        [Fact]
        public async Task ExecuteAsync_Should_Throw_When_FirstName_Is_Empty()
        {
            // Arrange
            var repoMock = new Mock<IEmployeeRepository>();
            var useCase = new CreateEmployee(repoMock.Object);

            var request = new CreateEmployeeRequest
            {
                FirstName = "",
                LastName = "Gubany",
                Email = "test@test.com"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => useCase.ExecuteAsync(request)
            );
        }

        //TODO : Add more tests for other validations (email regex etc.)
    }
}
