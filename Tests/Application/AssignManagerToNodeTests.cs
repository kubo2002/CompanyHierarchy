using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Application.DTOs.AssignManagerToNodeDTO;
using CompanyManagement.Application.UseCases;
using CompanyManagement.Domain.Entities;
using CompanyManagement.Domain.Enums;
using Moq;

namespace Tests.Application
{
    public class AssignManagerToNodeTests
    {
        [Fact]
        public async Task ExecuteAsync_Should_Assign_Manager_To_Node()
        {
            // Arrange
            var nodeId = Guid.NewGuid();
            var employeeId = Guid.NewGuid();

            var node = new Node(
                nodeId,
                "IT Divizia",
                "IT",
                NodeType.Division,
                null);

            var employee = new Employee(
                employeeId,
                "Jakub",
                "Gubany",
                "jakub@test.com",
                "123");

            var nodeRepoMock = new Mock<INodeRepository>();
            var employeeRepoMock = new Mock<IEmployeeRepository>();

            nodeRepoMock
                .Setup(r => r.GetByIdAsync(nodeId))
                .ReturnsAsync(node);

            employeeRepoMock
                .Setup(r => r.GetByIdAsync(employeeId))
                .ReturnsAsync(employee);

            var useCase = new AssignManagerToNode(
                nodeRepoMock.Object,
                employeeRepoMock.Object);

            var request = new AssignManagerRequest
            {
                NodeId = nodeId,
                EmployeeId = employeeId
            };

            // Act
            await useCase.ExecuteAsync(request);

            // Assert
            Assert.Equal(employeeId, node.LeaderEmployeeId);

            nodeRepoMock.Verify(
                r => r.UpdateAsync(node),
                Times.Once);
        }


        [Fact]
        public async Task ExecuteAsync_Should_Throw_When_Node_Does_Not_Exist()
        {
            // Arrange
            var nodeRepoMock = new Mock<INodeRepository>();
            var employeeRepoMock = new Mock<IEmployeeRepository>();

            var useCase = new AssignManagerToNode(
                nodeRepoMock.Object,
                employeeRepoMock.Object);

            var request = new AssignManagerRequest
            {
                NodeId = Guid.NewGuid(),
                EmployeeId = Guid.NewGuid()
            };

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => useCase.ExecuteAsync(request));
        }

        [Fact]
        public async Task ExecuteAsync_Should_Throw_When_Employee_Does_Not_Exist()
        {
            // Arrange
            var node = new Node(Guid.NewGuid(),"HR","HR",NodeType.Department,null);

            var nodeRepoMock = new Mock<INodeRepository>();
            var employeeRepoMock = new Mock<IEmployeeRepository>();

            nodeRepoMock
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(node);

            var useCase = new AssignManagerToNode(
                nodeRepoMock.Object,
                employeeRepoMock.Object);

            var request = new AssignManagerRequest
            {
                NodeId = node.Id,
                EmployeeId = Guid.NewGuid()
            };

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => useCase.ExecuteAsync(request));
        }

        [Fact]
        public async Task ExecuteAsync_Should_Throw_When_Employee_Already_Manages_Node()
        {
            // Arrange
            var employeeId = Guid.NewGuid();

            var existingNode = new Node(Guid.NewGuid(),"Division A","DIV-A",NodeType.Division,null);

            existingNode.AssignLeader(employeeId);

            var targetNode = new Node(Guid.NewGuid(), "Project X", "PRJ-X", NodeType.Project, existingNode.Id);

            var nodeRepoMock = new Mock<INodeRepository>();
            var employeeRepoMock = new Mock<IEmployeeRepository>();

            nodeRepoMock
                .Setup(r => r.GetByIdAsync(targetNode.Id))
                .ReturnsAsync(targetNode);

            nodeRepoMock
                .Setup(r => r.GetNodeManagedByEmployeeAsync(employeeId))
                .ReturnsAsync(existingNode);

            employeeRepoMock
                .Setup(r => r.GetByIdAsync(employeeId))
                .ReturnsAsync(new Employee(
                    employeeId, "Jakub", "Gubany", "j@g.com", "123"));

            var useCase = new AssignManagerToNode(
                nodeRepoMock.Object,
                employeeRepoMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => useCase.ExecuteAsync(new AssignManagerRequest
                {
                    NodeId = targetNode.Id,
                    EmployeeId = employeeId
                }));
        }
    }
}
