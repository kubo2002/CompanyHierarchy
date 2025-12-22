using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyManagement.Application.Abstractions.Repositories
{
    public interface IDepartmentEmployeeRepository
    {
        Task<bool> IsEmployeeAssignedAsync(Guid employeeId);
        Task AddAsync(Guid departmentId, Guid employeeId);
    }
}
