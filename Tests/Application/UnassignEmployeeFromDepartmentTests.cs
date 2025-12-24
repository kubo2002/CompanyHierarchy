using Azure.Core;
using CompanyManagement.Application.UseCases;
using CompanyManagement.Domain.Entities;
using CompanyManagement.Domain.Enums;
using CompanyManagement.Infrastructure.Repositories;

using Tests.Infrastructure;

namespace Tests.Application
{
    public class UnassignEmployeeFromDepartmentTests
    {
        [Fact]
        public async Task ExecuteAsync_Should_Remove_Employee_From_Department()
        {
            // Arrange
            var db = TestDbContextFactory.Create();

            var departmentId = Guid.NewGuid();
            var employeeId = Guid.NewGuid();

            db.Nodes.Add(new Node(departmentId, "IT Department", "IT", NodeType.Department, null));
            db.Employees.Add(new Employee(employeeId, "Ing.", "Jakub", "Gubany", "jakub@test.sk", "0900000000"));
            db.DepartmentEmployees.Add(new DepartmentEmployee
            {
                NodeId = departmentId,
                EmployeeId = employeeId
            });

            await db.SaveChangesAsync();

            var repo = new EfDepartmentEmployeeRepository(db);
            var useCase = new UnassignEmployeeFromDepartment(repo);

            // Act
            await useCase.ExecuteAsync(departmentId, employeeId);

            // Assert
            Assert.Empty(db.DepartmentEmployees);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Do_Nothing_When_Employee_Is_Not_Assigned()
        {
            // Arrange
            var db = TestDbContextFactory.Create();

            var departmentId = Guid.NewGuid();
            var employeeId = Guid.NewGuid();

            db.Nodes.Add(new Node(departmentId, "HR Department", "HR", NodeType.Department, null));

            db.Employees.Add(new Employee(employeeId, "Ing.", "Test", "User", "test@test.sk", "0900111111"));
            await db.SaveChangesAsync();

            var repo = new EfDepartmentEmployeeRepository(db);
            var useCase = new UnassignEmployeeFromDepartment(repo);

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(departmentId, employeeId));
        }

        [Fact]
        public async Task ExecuteAsync_Should_Remove_Only_Target_Employee()
        {
            // Arrange
            var db = TestDbContextFactory.Create();

            var departmentId = Guid.NewGuid();
            var employee1Id = Guid.NewGuid();
            var employee2Id = Guid.NewGuid();

            db.Nodes.Add(new Node(departmentId,"Finance Department","FIN",NodeType.Department,null));

            db.Employees.Add(new Employee(employee1Id,"Ing.","Employee","One","one@test.sk", "0900222222"));

            db.Employees.Add(new Employee(employee2Id,"Ing.","Employee","Two","two@test.sk","0900333333"));

            db.DepartmentEmployees.Add(new DepartmentEmployee
            {
                NodeId = departmentId,
                EmployeeId = employee1Id
            });

            db.DepartmentEmployees.Add(new DepartmentEmployee
            {
                NodeId = departmentId,
                EmployeeId = employee2Id
            });

            await db.SaveChangesAsync();

            var repo = new EfDepartmentEmployeeRepository(db);
            var useCase = new UnassignEmployeeFromDepartment(repo);

            // Act
            await useCase.ExecuteAsync(departmentId, employee1Id);

            // Assert
            var remaining = db.DepartmentEmployees
                .Select(de => de.EmployeeId)
                .ToList();

            Assert.Single(remaining);
            Assert.Contains(employee2Id, remaining);
            Assert.DoesNotContain(employee1Id, remaining);
        }
    }
}
