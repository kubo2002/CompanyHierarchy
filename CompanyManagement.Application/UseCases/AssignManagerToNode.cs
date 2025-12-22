using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Application.DTOs.AssignManagerToNodeDTO;

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
        /// <exception cref="InvalidOperationException">
        /// Thrown when the employee is already managing another node.
        /// </exception>
        /// <remarks>
        /// This use case validates the existence of the node and employee,
        /// then delegates the assignment logic to the domain entity.
        /// </remarks>
        public async Task ExecuteAsync(AssignManagerRequest request)
        {
            var node = await _nodeRepository.GetByIdAsync(request.NodeId);

            // v pripade ze node neexistuje v databaze
            if (node == null) 
            {
                throw new KeyNotFoundException("Node not found");
            }
                

            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);

            // v pripade ze employee neexistuje v databaze
            if (employee == null) 
            {
                throw new KeyNotFoundException("Employee not found");
            }
                

            var alreadyManagedNode = await _nodeRepository.GetNodeManagedByEmployeeAsync(employee.Id);

            if (alreadyManagedNode != null)
            {
                if (alreadyManagedNode.LeaderEmployeeId == request.EmployeeId)
                {
                    throw new InvalidOperationException("Employee is already manager of this node");
                }
                if (alreadyManagedNode != null)
                {
                    throw new InvalidOperationException("Employee is already manager of another node");
                }
            }
           
            node.AssignLeader(employee.Id);
            await _nodeRepository.UpdateAsync(node);
        }
    }
}
