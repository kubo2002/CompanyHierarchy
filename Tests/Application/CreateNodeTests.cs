using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Application.DTOs;
using CompanyManagement.Application.DTOs.CreateNodeDTO;
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
            var parentId = Guid.NewGuid();

            var parentNode = new Node(parentId,"Company A","COMP-A",NodeType.Company,null);

            var nodeRepositoryMock = new Mock<INodeRepository>();

            nodeRepositoryMock
                .Setup(r => r.GetByIdAsync(parentId))
                .ReturnsAsync(parentNode);

            var useCase = new CreateNode(nodeRepositoryMock.Object);

            var request = new CreateNodeRequest
            {
                Name = "IT Division",
                Code = "IT",
                Type = NodeType.Division,
                ParentId = parentId
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
                    n.ParentId == parentId
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
    }
}
