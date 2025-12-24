using CompanyManagement.Domain.Entities;

namespace Tests.Domain
{
    public class EmployeeTests
    {
        [Fact]
        public void Constructor_Should_Create_Employee_With_Given_Values()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            // Act
            Employee employee = new Employee(
                id,
                "Ing.",
                "Jakub",
                "Gubany",
                "jakub.gubany@test.com",
                "+421900123456");

            // Assert
            Assert.Equal(id, employee.Id);
            Assert.Equal("Jakub", employee.FirstName);
            Assert.Equal("Gubany", employee.LastName);
            Assert.Equal("jakub.gubany@test.com", employee.Email);
            Assert.Equal("+421900123456", employee.Phone);
        }
    }
}
