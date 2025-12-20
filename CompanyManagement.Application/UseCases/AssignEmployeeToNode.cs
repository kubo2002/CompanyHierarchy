using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Application.DTOs;
using CompanyManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyManagement.Application.UseCases
{
    public class AssignEmployeeToNode
    {
        private readonly INodeRepository _nodeRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public AssignEmployeeToNode(INodeRepository nodeRepository,IEmployeeRepository employeeRepository)
        {
            _nodeRepository = nodeRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task ExecuteAsync(AssignEmployeeToNodeRequest request)
        {
            var node = await _nodeRepository.GetByIdAsync(request.NodeId)
                ?? throw new ArgumentException("Node not found");

            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId)
                ?? throw new ArgumentException("Employee not found");

            if (node.Type != NodeType.Department)
            {
                throw new InvalidOperationException("Employees can only be assigned to department nodes");
            }
                
            var alreadyAssigned = await _nodeRepository.IsEmployeeAssignedToNodeAsync(node.Id, employee.Id);

            if (alreadyAssigned)
                return; 

            await _nodeRepository.AssignEmployeeToNodeAsync(node.Id, employee.Id);
        }
    }
}
