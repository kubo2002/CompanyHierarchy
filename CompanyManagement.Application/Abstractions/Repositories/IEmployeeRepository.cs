using CompanyManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyManagement.Application.Abstractions.Repositories
{
    public interface IEmployeeRepository
    {
        Task AddAsync(Employee employee);
        Task<Employee?> GetByIdAsync(Guid id);
    }
}
