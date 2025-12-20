using CompanyManagement.Domain.Entities;

namespace CompanyManagement.Application.Abstractions.Repositories
{
    public interface INodeRepository
    {
        Task AddAsync(Node node);
        Task<Node?> GetByIdAsync(Guid id);
        Task UpdateAsync(Node node);

        /// <summary>
        /// Gives the node managed by the specified employee.
        /// </summary>
        Task<Node?> GetNodeManagedByEmployeeAsync(Guid employeeId);
    }
}
