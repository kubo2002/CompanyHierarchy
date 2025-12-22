using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Application.DTOs.UnassignManagerDTO;
using CompanyManagement.Application.UseCases;
using CompanyManagement.Domain.Entities;
using CompanyManagement.Domain.Enums;
using Moq;

namespace Tests.Api
{
    public class UnassignManagerFromNodeApiTests
    {
        // v priapde ze uzol ma manazera, mal by ho uspesne odstranit z role manazera
        [Fact]
        public async Task ExecuteAsync_Should_Unassign_Manager_When_Node_Has_Manager()
        {
            // Arrange
            var nodeId = Guid.NewGuid();
            var managerId = Guid.NewGuid();

            var node = new Node(nodeId,"IT","IT",NodeType.Department,null);

            node.AssignLeader(managerId);

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

            nodeRepoMock.Verify(r => r.UpdateAsync(node),Times.Once);
        }

        // v priapde ze uzol nema manazera, mal by vyhodit vyjimku
        [Fact]
        public async Task ExecuteAsync_Should_Throw_When_Node_Has_No_Manager()
        {
            // Arrange
            var nodeId = Guid.NewGuid();
            var node = new Node(nodeId, "HR", "HR", NodeType.Department, null);

            var repo = new Mock<INodeRepository>();
            repo.Setup(r => r.GetByIdAsync(nodeId)).ReturnsAsync(node);

            var useCase = new UnassignManagerFromNode(repo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                useCase.ExecuteAsync(new UnassignManagerFromNodeRequest
                {
                    NodeId = nodeId
                })
            );

            repo.Verify(r => r.UpdateAsync(It.IsAny<Node>()),Times.Never);
        }

        // v priapde ze uzol neexistuje, mal by vyhodit vyjimku
        [Fact]
        public async Task ExecuteAsync_Should_Throw_When_Node_Does_Not_Exist()
        {
            // Arrange
            var nodeId = Guid.NewGuid();

            var nodeRepoMock = new Mock<INodeRepository>();
            nodeRepoMock
                .Setup(r => r.GetByIdAsync(nodeId))
                .ReturnsAsync((Node?)null);

            var useCase = new UnassignManagerFromNode(nodeRepoMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                useCase.ExecuteAsync(new UnassignManagerFromNodeRequest
                {
                    NodeId = nodeId
                })
            );

            nodeRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Node>()),Times.Never
            );
        }
    }
}
