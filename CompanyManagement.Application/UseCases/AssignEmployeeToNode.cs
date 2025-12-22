using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Domain.Enums;

namespace CompanyManagement.Application.UseCases
{
    public class AssignEmployeeToNode
    {
        private readonly INodeRepository _nodeRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public AssignEmployeeToNode(INodeRepository nodeRepository, IEmployeeRepository employeeRepository)
        {
            _nodeRepository = nodeRepository;
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Pokusi sa priradit zamestnanca do uzla typu Department.
        /// </summary>
        /// <param name="NodeId">ID cieloveho uzla</param>
        /// <param name="EmployeeId">ID zamestnanca</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">V pripade ze sa pozadovane udaje v databaze nenajdu</exception>
        /// <exception cref="InvalidOperationException">V pripade ze dojde ku konfliktu s uz existujucim zaznamom v databaze.</exception>
        public async Task ExecuteAsync(Guid NodeId, Guid EmployeeId)
        {
            var node = await _nodeRepository.GetByIdAsync(NodeId)
                ?? throw new KeyNotFoundException("Node not found");

            var employee = await _employeeRepository.GetByIdAsync(EmployeeId)
                ?? throw new KeyNotFoundException("Employee not found");

            if (node.Type != NodeType.Department)
            {
                throw new InvalidOperationException("Employees can only be assigned to department nodes");
            }

            var alreadyAssigned = await _nodeRepository.IsEmployeeAssignedToNodeAsync(node.Id, employee.Id);

            if (alreadyAssigned)
            {
                throw new InvalidOperationException("Employee is already assigned to a department");
            }

            var assignedGlobaly = await _nodeRepository.IsEmployeeAssignedToAnyDepartmentAsync(employee.Id);

            if (assignedGlobaly)
            {
                throw new InvalidOperationException("Employee is already assigned to another department");
            }
            await _nodeRepository.AssignEmployeeToNodeAsync(node.Id, employee.Id);
        }
    }
}
