using CompanyManagement.Application.UseCases;
using CompanyManagement.Domain.Entities;
using CompanyManagement.Domain.Enums;
using CompanyManagement.Infrastructure.Persistence;
using CompanyManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Tests.Infrastructure;

namespace Tests.Api
{
    public class AssignEmployeeToNodeIntegrationTests
    {
        private readonly ManagementDbContext _dbContext;
        private readonly AssignEmployeeToNode _useCase;

        public AssignEmployeeToNodeIntegrationTests()
        {
            _dbContext = TestDbContextFactory.Create();

            var nodeRepository = new EfNodeRepository(_dbContext);
            var employeeRepository = new EfEmployeeRepository(_dbContext);

            _useCase = new AssignEmployeeToNode(nodeRepository, employeeRepository);
        }


        // uspesne priradenie zamestnanca do oddelenia
        [Fact]
        public async Task ExecuteAsync_Should_Assign_Employee_To_Department()
        {
            var department = new Node(Guid.NewGuid(), "IT", "IT01", NodeType.Department, null);
            var employee = new Employee(Guid.NewGuid(), "Jakub", "Gubany", "jakub@gubany.com", "123");

            _dbContext.Nodes.Add(department);
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();

            await _useCase.ExecuteAsync(department.Id, employee.Id);

            var exists = await _dbContext.DepartmentEmployees
                .AnyAsync(de => de.NodeId == department.Id && de.EmployeeId == employee.Id);

            Assert.True(exists);
        }


        // priradenie zamestnanca do neexistujuceho uzla
        [Fact]
        public async Task ExecuteAsync_Should_Throw_When_Node_Not_Found()
        {
            var employee = new Employee(Guid.NewGuid(), "Jakub", "Gubany", "jakub@gubany.com", "123");
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();

            var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _useCase.ExecuteAsync(Guid.NewGuid(), employee.Id));

            Assert.Equal("Node not found", ex.Message);
        }

        // priaradenie neexistujuceho zamestnanca do oddelenia
        [Fact]
        public async Task ExecuteAsync_Should_Throw_When_Employee_Not_Found()
        {
            var department = new Node(Guid.NewGuid(), "IT", "IT01", NodeType.Department, null);
            _dbContext.Nodes.Add(department);
            await _dbContext.SaveChangesAsync();

            var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _useCase.ExecuteAsync(department.Id, Guid.NewGuid()));

            Assert.Equal("Employee not found", ex.Message);
        }


        // priradenie zamestnanca do uzla, ktory nie je oddelenim
        [Fact]
        public async Task ExecuteAsync_Should_Throw_When_Node_Is_Not_Department()
        {
            var company = new Node(Guid.NewGuid(), "Company", "C01", NodeType.Company, null);
            var employee = new Employee(Guid.NewGuid(), "Jakub", "Gubany", "jakub@gubany.com", "123");

            _dbContext.Nodes.Add(company);
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _useCase.ExecuteAsync(company.Id, employee.Id));

            Assert.Equal("Employees can only be assigned to department nodes",ex.Message);
        }

        // priradenie zamestnanca do uzla v ktorm uz je priradeny
        [Fact]
        public async Task ExecuteAsync_Should_Throw_When_Employee_Already_In_Same_Department()
        {
            var department = new Node(Guid.NewGuid(), "IT", "IT01", NodeType.Department, null);
            var employee = new Employee(Guid.NewGuid(), "Jakub", "Gubany", "jakub@gubany.com", "123");

            _dbContext.Nodes.Add(department);
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();

            _dbContext.DepartmentEmployees.Add(new DepartmentEmployee
            {
                NodeId = department.Id,
                EmployeeId = employee.Id
            });
            await _dbContext.SaveChangesAsync();

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.ExecuteAsync(department.Id, employee.Id));

            Assert.Equal("Employee is already assigned to a department",ex.Message);
        }

        // priradenie zamestnanca do oddelenia, pricom uz je registrovany v inom oddeleni.
        [Fact]
        public async Task ExecuteAsync_Should_Throw_When_Employee_In_Another_Department()
        {
            var department1 = new Node(Guid.NewGuid(), "IT", "IT01", NodeType.Department, null);
            var department2 = new Node(Guid.NewGuid(), "HR", "HR01", NodeType.Department, null);
            var employee = new Employee(Guid.NewGuid(), "Jakub", "Gubany", "jakub@gubany.com", "123");

            _dbContext.Nodes.AddRange(department1, department2);
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();

            _dbContext.DepartmentEmployees.Add(new DepartmentEmployee
            {
                NodeId = department1.Id,
                EmployeeId = employee.Id
            });
            await _dbContext.SaveChangesAsync();

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.ExecuteAsync(department2.Id, employee.Id));

            Assert.Equal("Employee is already assigned to another department", ex.Message);
        }
    }

}
