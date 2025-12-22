using CompanyManagement.Domain.Entities;

namespace CompanyManagement.Application.Abstractions.Repositories
{
    public interface IEmployeeRepository
    {
        Task AddAsync(Employee employee);
        Task<Employee?> GetByIdAsync(Guid id);
        Task<bool> ExistsByEmailAsync(string email);
    }
}
