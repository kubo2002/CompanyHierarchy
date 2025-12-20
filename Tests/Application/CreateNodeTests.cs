using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Application.DTOs;
using CompanyManagement.Application.UseCases;
using CompanyManagement.Domain.Entities;
using CompanyManagement.Domain.Enums;
using Moq;

namespace Tests.Application
{
    public class CreateNodeTests
    {
        [Fact]
        public async Task ExecuteAsync_Should_Create_Node_And_Save_It()
        {
            // Arrange
            var nodeRepositoryMock = new Mock<INodeRepository>();

            var useCase = new CreateNode(nodeRepositoryMock.Object);

            var request = new CreateNodeRequest
            {
                Name = "IT Division",
                Code = "IT",
                Type = NodeType.Division,
                ParentId = Guid.NewGuid()
            };

            // Act
            var resultId = await useCase.ExecuteAsync(request);

            // Assert
            Assert.NotEqual(Guid.Empty, resultId);

            nodeRepositoryMock.Verify(
                r => r.AddAsync(It.Is<Node>(n =>
                    n.Name == "IT Division" &&
                    n.Code == "IT" &&
                    n.Type == NodeType.Division &&
                    n.ParentId == request.ParentId
                )),
                Times.Once
            );
        }

        [Fact]
        public async Task ExecuteAsync_Should_Throw_When_Name_Is_Empty()
        {
            // Arrange
            var repoMock = new Mock<INodeRepository>();
            var useCase = new CreateNode(repoMock.Object);

            var request = new CreateNodeRequest
            {
                Name = "",
                Code = "CODE",
                Type = NodeType.Company
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => useCase.ExecuteAsync(request)
            );
        }

        [Fact]
        public async Task ExecuteAsync_Should_Throw_When_Company_Has_Parent()
        {
            var repoMock = new Mock<INodeRepository>();
            var useCase = new CreateNode(repoMock.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                useCase.ExecuteAsync(new CreateNodeRequest
                {
                    Name = "Company",
                    Code = "C",
                    Type = NodeType.Company,
                    ParentId = Guid.NewGuid()
                }));
        }

        [Fact]
        public async Task ExecuteAsync_Should_Throw_When_Parent_Is_Department()
        {
            var parentId = Guid.NewGuid();

            var repoMock = new Mock<INodeRepository>();
            repoMock
                .Setup(r => r.GetByIdAsync(parentId))
                .ReturnsAsync(new Node(parentId, "HR", "HR", NodeType.Department, null));

            var useCase = new CreateNode(repoMock.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                useCase.ExecuteAsync(new CreateNodeRequest
                {
                    Name = "Sub node",
                    Code = "SUB",
                    Type = NodeType.Project,
                    ParentId = parentId
                }));
        }
    }
}
