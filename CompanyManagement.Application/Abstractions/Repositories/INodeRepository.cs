using CompanyManagement.Domain.Entities;

namespace CompanyManagement.Application.Abstractions.Repositories
{
    public interface INodeRepository
    {
        Task AddAsync(Node node);
        Task<Node?> GetByIdAsync(Guid id);
        Task UpdateAsync(Node node);

        Task<Node?> GetNodeManagedByEmployeeAsync(Guid employeeId);

        // členstvo zamestnancov
        Task<bool> IsEmployeeAssignedToNodeAsync(Guid nodeId, Guid employeeId);
        Task AssignEmployeeToNodeAsync(Guid nodeId, Guid employeeId);
        Task RemoveEmployeeFromNodeAsync(Guid nodeId, Guid employeeId);

        // overenie ci je zamestnanec priradeny aspon k jednomu oddeleniu globalne medzi hierarchiami
        Task<bool> IsEmployeeAssignedToAnyDepartmentAsync(Guid employeeId);

    }
}
