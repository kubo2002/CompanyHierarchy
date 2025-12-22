using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Application.DTOs;
using CompanyManagement.Application.UseCases;
using CompanyManagement.Domain.Entities;
using CompanyManagement.Domain.Enums;
using Moq;

namespace Tests.Application
{
    public class AssignEmployeeToNodeTests
    {
        [Fact]
        public async Task ExecuteAsync_Should_Assign_Employee_To_Node()
        {
            // Arrange
            var nodeId = Guid.NewGuid();
            var employeeId = Guid.NewGuid();

            var nodeRepoMock = new Mock<INodeRepository>();
            var employeeRepoMock = new Mock<IEmployeeRepository>();

            // V pripade zmeny z NoteType.Department na iny typ, test neprejde.
            // Tym je uspesne otestovana podmienka pridavania zamestnancov len na Department uzly (listy hierarchie).
            nodeRepoMock
                .Setup(r => r.GetByIdAsync(nodeId))
                .ReturnsAsync(new Node(nodeId, "Project A", "PA", NodeType.Department, null)); 

            employeeRepoMock
                .Setup(r => r.GetByIdAsync(employeeId))
                .ReturnsAsync(new Employee(employeeId, "Jakub", "Gubany", "jakub@gubany.com", "123"));

            nodeRepoMock
                .Setup(r => r.IsEmployeeAssignedToNodeAsync(nodeId, employeeId))
                .ReturnsAsync(false);

            var useCase = new AssignEmployeeToNode(nodeRepoMock.Object, employeeRepoMock.Object);

            // Act
            await useCase.ExecuteAsync(nodeId, employeeId);
            

            // Assert
            nodeRepoMock.Verify(r => r.AssignEmployeeToNodeAsync(nodeId, employeeId), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Throw_When_Assigning_Employee_To_Non_Department()
        {
            var nodeRepoMock = new Mock<INodeRepository>();
            var employeeRepoMock = new Mock<IEmployeeRepository>();

            var node = new Node(Guid.NewGuid(),"Project A","PA",NodeType.Project,null);

            nodeRepoMock
                .Setup(r => r.GetByIdAsync(node.Id))
                .ReturnsAsync(node);

            employeeRepoMock
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Employee(Guid.NewGuid(), "Jakub", "Gubany", "jakub@gubany.com", "123"));

            var useCase = new AssignEmployeeToNode(nodeRepoMock.Object,employeeRepoMock.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => useCase.ExecuteAsync(node.Id, Guid.NewGuid()));
        }
    }
}
