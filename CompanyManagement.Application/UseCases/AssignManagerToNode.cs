using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Application.DTOs;

namespace CompanyManagement.Application.UseCases
{
    public class AssignManagerToNode
    {
        private readonly INodeRepository _nodeRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public AssignManagerToNode(
            INodeRepository nodeRepository,
            IEmployeeRepository employeeRepository)
        {
            _nodeRepository = nodeRepository;
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Assigns an employee as a manager (leader) of an organizational node.
        /// </summary>
        /// <param name="request">
        /// Request containing identifiers of the node and the employee to be assigned as manager.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when the specified node or employee does not exist.
        /// </exception>
        /// <remarks>
        /// This use case validates the existence of the node and employee,
        /// then delegates the assignment logic to the domain entity.
        /// </remarks>
        public async Task ExecuteAsync(AssignManagerRequest request)
        {
            var node = await _nodeRepository.GetByIdAsync(request.NodeId);
            if (node is null)
                throw new ArgumentException("Node not found");

            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
            if (employee is null)
                throw new ArgumentException("Employee not found");

            node.AssignLeader(employee.Id);

            await _nodeRepository.UpdateAsync(node);
        }
    }
}
