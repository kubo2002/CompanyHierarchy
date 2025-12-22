using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Application.DTOs.UnassignManagerDTO;
using CompanyManagement.Application.UseCases;
using CompanyManagement.Domain.Entities;
using CompanyManagement.Domain.Enums;
using Moq;

namespace Tests.Application
{
    public class UnassignManagerFromNodeTests
    {
        [Fact]
        public async Task ExecuteAsync_Should_Unassign_Manager_From_Node()
        {
            // Arrange
            var nodeId = Guid.NewGuid();
            var employeeId = Guid.NewGuid();

            var node = new Node(nodeId, "Division A", "DIV-A", NodeType.Division, null);

            node.AssignLeader(employeeId);

            var nodeRepoMock = new Mock<INodeRepository>();
            nodeRepoMock
                .Setup(r => r.GetByIdAsync(nodeId))
                .ReturnsAsync(node);

            var useCase = new UnassignManagerFromNode(nodeRepoMock.Object);

            // Act
            await useCase.ExecuteAsync(new UnassignManagerFromNodeRequest
            {
                NodeId = nodeId
            });

            // Assert
            Assert.Null(node.LeaderEmployeeId);

            nodeRepoMock.Verify(r => r.UpdateAsync(node), Times.Once);
        }
    
        [Fact]
        public async Task ExecuteAsync_Should_Throw_When_Node_Does_Not_Exist()
        {
            var nodeRepoMock = new Mock<INodeRepository>();

            var useCase = new UnassignManagerFromNode(nodeRepoMock.Object);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                useCase.ExecuteAsync(new UnassignManagerFromNodeRequest
                {
                    NodeId = Guid.NewGuid()
                }));
        }
    }
}
