using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Application.DTOs;
using CompanyManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Application
{
    public class MoveEmployeeBetweenCompanies
    {
        private readonly INodeRepository _nodeRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public MoveEmployeeBetweenCompanies(INodeRepository nodeRepository, IEmployeeRepository employeeRepository)
        {
            _nodeRepository = nodeRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task ExecuteAsync(MoveEmployeeBetweenCompaniesRequest request)
        {
            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId) ?? throw new ArgumentException("Employee not found");

            var targetCompany = await _nodeRepository.GetByIdAsync(request.TargetCompanyNodeId) ?? throw new ArgumentException("Target company not found");

            if (targetCompany.Type != NodeType.Company)
            {
                throw new InvalidOperationException("Target node is not a company");
            }
               

            var managedNode = await _nodeRepository.GetNodeManagedByEmployeeAsync(employee.Id);

            if (managedNode != null)
            {
                managedNode.AssignLeader(null);
                await _nodeRepository.UpdateAsync(managedNode);
            }

            // sem ešte pridam hierachicke pravidla a členstvo, pripadne automatikce priradenie
         
        }
    }
}
