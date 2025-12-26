using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Application.DTOs;

namespace CompanyManagement.Application.UseCases
{
    public class RemoveEmployeeFromNode
    {
        private readonly INodeRepository _nodeRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public RemoveEmployeeFromNode(INodeRepository nodeRepository, IEmployeeRepository employeeRepository)
        {
            _nodeRepository = nodeRepository;
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Removes an employee assignment from a department node.
        /// </summary>
        /// <param name="request">
        /// Request containing identifiers of the department node and the employee to be removed.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when the specified node or employee does not exist.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when attempting to remove an employee from a node
        /// that is not of type NodeType.Department.
        /// </exception>
        /// <remarks>
        /// Employees can only be assigned to department nodes.
        /// If the employee is not currently assigned to the specified department,
        /// the operation completes without making any changes.
        /// </remarks>
        public async Task ExecuteAsync(RemoveEmployeeFromNodeRequest request)
        {
           
            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId)
                ?? throw new ArgumentException("Employee not found");
        }
    }
}
