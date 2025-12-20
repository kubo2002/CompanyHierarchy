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
        public async Task ExecuteAsync_Should_Remove_Employee_From_Department()
        {
            // Arrange
            var nodeId = Guid.NewGuid();
            var employeeId = Guid.NewGuid();

            var nodeRepoMock = new Mock<INodeRepository>();
            var employeeRepoMock = new Mock<IEmployeeRepository>();

            nodeRepoMock
                .Setup(r => r.GetByIdAsync(nodeId))
                .ReturnsAsync(new Node(nodeId, "HR", "HR", NodeType.Department, null));

            employeeRepoMock
                .Setup(r => r.GetByIdAsync(employeeId))
                .ReturnsAsync(new Employee(employeeId, "John", "Doe", "a@b.com", "123"));

            nodeRepoMock
                .Setup(r => r.IsEmployeeAssignedToNodeAsync(nodeId, employeeId))
                .ReturnsAsync(true);

            var useCase = new RemoveEmployeeFromNode(nodeRepoMock.Object, employeeRepoMock.Object);

            // Act
            await useCase.ExecuteAsync(new RemoveEmployeeFromNodeRequest
            {
                NodeId = nodeId,
                EmployeeId = employeeId
            });

            // Assert
            nodeRepoMock.Verify(r => r.RemoveEmployeeFromNodeAsync(nodeId, employeeId), Times.Once);
        }

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
                .ReturnsAsync(new Employee(employeeId, "Jakub", "Gubany", "jakub@gubany.com", "123"));

            nodeRepoMock
                .Setup(r => r.IsEmployeeAssignedToNodeAsync(nodeId, employeeId))
                .ReturnsAsync(false);

            var useCase = new RemoveEmployeeFromNode(nodeRepoMock.Object, employeeRepoMock.Object);

            await useCase.ExecuteAsync(new RemoveEmployeeFromNodeRequest
            {
                NodeId = nodeId,
                EmployeeId = employeeId
            });

            nodeRepoMock.Verify(r => r.RemoveEmployeeFromNodeAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Throw_When_Node_Is_Not_Department()
        {
            var nodeRepoMock = new Mock<INodeRepository>();
            var employeeRepoMock = new Mock<IEmployeeRepository>();

            var node = new Node(Guid.NewGuid(), "Project", "P", NodeType.Project, null);

            nodeRepoMock
                .Setup(r => r.GetByIdAsync(node.Id))
                .ReturnsAsync(node);

            employeeRepoMock
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Employee(Guid.NewGuid(), "Jakub", "Gubany", "jakub@gubany.com", "123"));

            var useCase = new RemoveEmployeeFromNode(nodeRepoMock.Object, employeeRepoMock.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                useCase.ExecuteAsync(new RemoveEmployeeFromNodeRequest
                {
                    NodeId = node.Id,
                    EmployeeId = Guid.NewGuid()
                }));
        }
    }
}
