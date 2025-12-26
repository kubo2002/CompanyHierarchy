using CompanyManagement.Domain.Entities;
using CompanyManagement.Domain.Enums;
namespace Tests.Domain
{
    public class NodeTests
    {
        [Fact]
        public void Constructor_Should_Create_Node_With_Given_Values()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            Guid parentId = Guid.NewGuid();

            // Act
            Node node = new Node(
                id,
                "IT Divizia",
                "IT",
                NodeType.Division,
                parentId);

            // Assert
            Assert.Equal(id, node.Id);
            Assert.Equal("IT Divizia", node.Name);
            Assert.Equal("IT", node.Code);
            Assert.Equal(NodeType.Division, node.Type);
            Assert.Equal(parentId, node.ParentId);
            Assert.Null(node.LeaderEmployeeId);
        }

      

        [Fact]
        public void AssignLeader_Should_Set_LeaderEmployeeId()
        {
            // Arrange
            Node node = new Node(
                Guid.NewGuid(),
                "HR",
                "HR",
                NodeType.Department,
                null);

            Guid employeeId = Guid.NewGuid();

            // Act
            node.AssignLeader(employeeId);

            // Assert
            Assert.Equal(employeeId, node.LeaderEmployeeId);
        }
    }
}
