using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Application.DTOs;
using CompanyManagement.Domain.Entities;
using CompanyManagement.Domain.Enums;
using Moq;

namespace Tests.Application
{
    public class MoveEmployeeBetweenCompaniesTests
    {
        [Fact]
        public async Task ExecuteAsync_Should_Remove_Manager_Role_When_Moving_Employee()
        {
            // Arrange
            var employeeId = Guid.NewGuid();

            var company = new Node(Guid.NewGuid(), "Fri Uniza", "FU", NodeType.Company, null);

            var managedNode = new Node(Guid.NewGuid(), "Old Division", "OLD", NodeType.Division, company.Id);

            managedNode.AssignLeader(employeeId);

            var nodeRepoMock = new Mock<INodeRepository>();
            var employeeRepoMock = new Mock<IEmployeeRepository>();

            employeeRepoMock
                .Setup(r => r.GetByIdAsync(employeeId))
                .ReturnsAsync(new Employee(employeeId, "Jakub", "Gubany", "j@g.com", "123"));

            nodeRepoMock
                .Setup(r => r.GetByIdAsync(company.Id))
                .ReturnsAsync(company);

            nodeRepoMock
                .Setup(r => r.GetNodeManagedByEmployeeAsync(employeeId))
                .ReturnsAsync(managedNode);

            var useCase = new MoveEmployeeBetweenCompanies(nodeRepoMock.Object, employeeRepoMock.Object);

            // Act
            await useCase.ExecuteAsync(new MoveEmployeeBetweenCompaniesRequest
            {
                EmployeeId = employeeId,
                TargetCompanyNodeId = company.Id
            });

            // Assert
            Assert.Null(managedNode.LeaderEmployeeId);
        }
    }
}
