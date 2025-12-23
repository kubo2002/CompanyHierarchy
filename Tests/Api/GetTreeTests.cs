using CompanyManagement.Domain.Entities;
using CompanyManagement.Domain.Enums;
using CompanyManagement.Infrastructure.Repositories;
using Tests.Infrastructure;

namespace Tests.Api
{
    public class GetTreeTests
    {
        [Fact]
        public async Task GetChildrenAsync_Should_Return_Only_Direct_Children()
        {
            // Arrange
            var parentId = Guid.NewGuid();

            var parent = new Node(parentId,"Division","DIV",NodeType.Division,null);

            var child1 = new Node(Guid.NewGuid(),"Department 1","DEP1",NodeType.Department,parentId);

            var child2 = new Node(Guid.NewGuid(),"Department 2","DEP2",NodeType.Department,parentId);

            var unrelated = new Node(Guid.NewGuid(),"Other","OTH",NodeType.Department, null);

            var db = TestDbContextFactory.Create();

            // postupne pridam uzly do db
            db.Nodes.Add(parent);
            db.Nodes.Add(child1);
            db.Nodes.Add(child2);
            db.Nodes.Add(unrelated);

            await db.SaveChangesAsync();

            var repo = new EfNodeRepository(db);

            // Act
            var children = await repo.GetChildrenAsync(parentId);

            // Assert
            Assert.Equal(2, children.Count);
            Assert.All(children, c => Assert.Equal(parentId, c.ParentId));
        }
    }
}
