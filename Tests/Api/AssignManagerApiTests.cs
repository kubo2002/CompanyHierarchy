using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Application.DTOs.AssignManagerToNodeDTO;
using CompanyManagement.Application.DTOs.CreateEmployeeDTO;
using CompanyManagement.Application.UseCases;
using CompanyManagement.Domain.Entities;
using CompanyManagement.Domain.Enums;
using Moq;

namespace Tests.Api
{
    public class AssignManagerApiTests
    {
        private readonly Mock<INodeRepository> _nodeRepositoryMock;
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
        private readonly AssignManagerToNode _useCase;

        public AssignManagerApiTests()
        {
            _nodeRepositoryMock = new Mock<INodeRepository>();
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();

            _useCase = new AssignManagerToNode(
                _nodeRepositoryMock.Object,
                _employeeRepositoryMock.Object
            );
        }

        // uspesne priradeni managera
        [Fact]
        public async Task ExecuteAsync_AssignsManager_WhenValid()
        {
            var nodeId = Guid.NewGuid();
            var employeeId = Guid.NewGuid();

            var node = new Node(nodeId,"Company","C01",NodeType.Company,null);

            var employee = new Employee(employeeId,null,"Jakub","Gubany","jakub@gubany.com","123");

            _nodeRepositoryMock
                .Setup(r => r.GetByIdAsync(nodeId))
                .ReturnsAsync(node);

            _employeeRepositoryMock
                .Setup(r => r.GetByIdAsync(employeeId))
                .ReturnsAsync(employee);

            _nodeRepositoryMock
                .Setup(r => r.GetNodeManagedByEmployeeAsync(employeeId))
                .ReturnsAsync((Node?)null);

            await _useCase.ExecuteAsync(new AssignManagerRequest
            {
                NodeId = nodeId,
                EmployeeId = employeeId
            });

            Assert.Equal(employeeId, node.LeaderEmployeeId);
            _nodeRepositoryMock.Verify(r => r.UpdateAsync(node), Times.Once);
        }

        // cielovy uzol neexistuje
        [Fact]
        public async Task ExecuteAsync_Throws_WhenNodeDoesNotExist()
        {
            var request = new AssignManagerRequest
            {
                NodeId = Guid.NewGuid(),
                EmployeeId = Guid.NewGuid()
            };

            _nodeRepositoryMock
                .Setup(r => r.GetByIdAsync(request.NodeId))
                .ReturnsAsync((Node?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _useCase.ExecuteAsync(request));
        }

        // employee neexistuje
        [Fact]
        public async Task ExecuteAsync_Throws_WhenEmployeeDoesNotExist()
        {
            var nodeId = Guid.NewGuid();
            var employeeId = Guid.NewGuid();

            var node = new Node(nodeId,"Company","C01",NodeType.Company,null);

            _nodeRepositoryMock
                .Setup(r => r.GetByIdAsync(nodeId))
                .ReturnsAsync(node);

            _employeeRepositoryMock
                .Setup(r => r.GetByIdAsync(employeeId))
                .ReturnsAsync((Employee?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _useCase.ExecuteAsync(new AssignManagerRequest
                { NodeId = nodeId, EmployeeId = employeeId}));
        }

        // uzol uz ma priradeneho manazera
        [Fact]
        public async Task ExecuteAsync_Throws_WhenEmployeeAlreadyManagesThisNode()
        {
            var nodeId = Guid.NewGuid();
            var employeeId = Guid.NewGuid();

            var node = new Node(nodeId,"Company","C01",NodeType.Company,null);

            node.AssignLeader(employeeId);

            var employee = new Employee(employeeId,null,"Jakub","Gubany","jakub@gubany.com","123");

            _nodeRepositoryMock.Setup(r => r.GetByIdAsync(nodeId)).ReturnsAsync(node);
            _employeeRepositoryMock.Setup(r => r.GetByIdAsync(employeeId)).ReturnsAsync(employee);

            _nodeRepositoryMock
                .Setup(r => r.GetNodeManagedByEmployeeAsync(employeeId))
                .ReturnsAsync(node);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.ExecuteAsync(new AssignManagerRequest
                { NodeId = nodeId, EmployeeId = employeeId}));

            Assert.Equal("Employee is already manager of this node", ex.Message);
        }

        // zamestnanec uz riadi iny uzol
        [Fact]
        public async Task ExecuteAsync_Throws_WhenEmployeeManagesAnotherNode()
        {
            var nodeId = Guid.NewGuid();
            var otherNodeId = Guid.NewGuid();
            var employeeId = Guid.NewGuid();

            var node = new Node(nodeId,"Company","C01",NodeType.Company, null);

            var otherNode = new Node(otherNodeId,"Division","D01",NodeType.Division,nodeId);

            var employee = new Employee(employeeId,null, "Jakub","Gubany","jakub@gubany.com","123");

            _nodeRepositoryMock.Setup(r => r.GetByIdAsync(nodeId)).ReturnsAsync(node);
            _employeeRepositoryMock.Setup(r => r.GetByIdAsync(employeeId)).ReturnsAsync(employee);

            _nodeRepositoryMock
                .Setup(r => r.GetNodeManagedByEmployeeAsync(employeeId))
                .ReturnsAsync(otherNode);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.ExecuteAsync(new AssignManagerRequest
                {NodeId = nodeId, EmployeeId = employeeId}));

            Assert.Equal("Employee is already manager of another node", ex.Message);
        }
    }
}
