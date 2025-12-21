using CompanyManagement.Domain.Entities;
using CompanyManagement.Domain.Enums;
using CompanyManagement.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Infrastructure
{
    /// <summary>
    /// Infrastructure testujem integračne priamo, aby som mal istotu, že mapovanie a perzistencia fungujú ešte pred API vrstvou.
    /// </summary>
    public class EfNodeRepositoryTests
    {
        [Fact]
        public async Task AddAndGetNode_Should_Work()
        {
            var db = TestDbContextFactory.Create();
            var repo = new EfNodeRepository(db);

            var node = new Node(Guid.NewGuid(),"IT","IT",NodeType.Department,null);

            await repo.AddAsync(node);

            var loaded = await repo.GetByIdAsync(node.Id);

            Assert.NotNull(loaded);
            Assert.Equal("IT", loaded!.Name);
        }

        [Fact]
        public async Task AssignEmployeeToNode_Should_Persist()
        {
            // vytvorím testovací DbContext (SQLite in-memory)
            var db = TestDbContextFactory.Create();
            var nodeRepo = new EfNodeRepository(db);
            var empRepo = new EfEmployeeRepository(db);

            // vytvorím zamestnanca a oddelenie
            var employee = new Employee(Guid.NewGuid(), "Jakub", "Gubany", "jakub@gubany.com", "123");
            var node = new Node(Guid.NewGuid(), "Vyvojari", "Vyvojari", NodeType.Department, null);

            // uložím ich do databázy
            await empRepo.AddAsync(employee);
            await nodeRepo.AddAsync(node);

            await nodeRepo.AssignEmployeeToNodeAsync(node.Id, employee.Id);

            // spätne ich načítam
            var isAssigned = await nodeRepo.IsEmployeeAssignedToNodeAsync(node.Id, employee.Id);

            // a overím, že je zamestnanec priradený k oddeleniu
            Assert.True(isAssigned);
        }

        [Fact]
        public async Task AddAndGetEmployee_Should_Persist_Employee()
        {
            // vytvorím testovací DbContext (SQLite in-memory)
            var db = TestDbContextFactory.Create();
            var repo = new EfEmployeeRepository(db);

            // vytvorím zamestnanca
            var employee = new Employee(Guid.NewGuid(),"Ján","Novák","jan.novak@test.sk","0900123456");

            // uložím ho do databázy
            await repo.AddAsync(employee);

            // načítam ho späť
            var loaded = await repo.GetByIdAsync(employee.Id);

            // overím, že bol správne uložený
            Assert.NotNull(loaded);
            Assert.Equal("Ján", loaded!.FirstName);
            Assert.Equal("Novák", loaded.LastName);
        }

        [Fact]
        public async Task RemoveEmployeeFromDepartment_Should_Work()
        {
            var db = TestDbContextFactory.Create();
            var nodeRepo = new EfNodeRepository(db);
            var employeeRepo = new EfEmployeeRepository(db);

            var department = new Node(Guid.NewGuid(),"Kamenolom","KMLNM",NodeType.Department,null);
            var employee = new Employee(Guid.NewGuid(),"Jakub", "Gubány", "jakub@gubany.sk", "0900123456");

            await nodeRepo.AddAsync(department);
            await employeeRepo.AddAsync(employee);

            await nodeRepo.AssignEmployeeToNodeAsync(department.Id, employee.Id);
            await nodeRepo.RemoveEmployeeFromNodeAsync(department.Id, employee.Id);

            var isAssigned = await nodeRepo.IsEmployeeAssignedToNodeAsync(department.Id, employee.Id);

            // overíme, že zamestnanec už v departmente nie je
            Assert.False(isAssigned);
        }
    }
}
