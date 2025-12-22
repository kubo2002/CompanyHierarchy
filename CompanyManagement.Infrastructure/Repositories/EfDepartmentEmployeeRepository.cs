using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyManagement.Infrastructure.Repositories
{
    public class EfDepartmentEmployeeRepository : IDepartmentEmployeeRepository
    {
        private readonly ManagementDbContext _dbContext;

        public EfDepartmentEmployeeRepository(ManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsEmployeeAssignedAsync(Guid employeeId)
        {
            return await _dbContext.DepartmentEmployees.AnyAsync(de => de.EmployeeId == employeeId);
        }

        public async Task AddAsync(Guid departmentId, Guid employeeId)
        {
            _dbContext.DepartmentEmployees.Add(
                new DepartmentEmployee
                {
                    NodeId = departmentId,
                    EmployeeId = employeeId
                });

            await _dbContext.SaveChangesAsync();
        }
    }
}
