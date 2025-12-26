using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Application.DTOs;
using CompanyManagement.Application.UseCases;
using CompanyManagement.Domain.Entities;
using CompanyManagement.Domain.Enums;
using Moq;

namespace Tests.Application
{
    public class RemoveEmployeeFromNodeTests
    {
        [Fact]
        public async Task ExecuteAsync_Should_Do_Nothing_When_Employee_Is_Not_Assigned()
        {
            var nodeId = Guid.NewGuid();
            var employeeId = Guid.NewGuid();

            var nodeRepoMock = new Mock<INodeRepository>();
            var employeeRepoMock = new Mock<IEmployeeRepository>();

            nodeRepoMock
                .Setup(r => r.GetByIdAsync(nodeId))
                .ReturnsAsync(new Node(nodeId, "HR", "HR", NodeType.Department, null));

            employeeRepoMock
                .Setup(r => r.GetByIdAsync(employeeId))
                .ReturnsAsync(new Employee(employeeId, "Ing","Jakub", "Gubany", "jakub@gubany.com", "123"));

            nodeRepoMock
                .Setup(r => r.IsEmployeeAssignedToNodeAsync(nodeId, employeeId))
                .ReturnsAsync(false);

            var useCase = new RemoveEmployeeFromNode(nodeRepoMock.Object, employeeRepoMock.Object);

            await useCase.ExecuteAsync(new RemoveEmployeeFromNodeRequest
            {
                EmployeeId = employeeId
            });

            nodeRepoMock.Verify(r => r.RemoveEmployeeFromNodeAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
        }
    }
}
