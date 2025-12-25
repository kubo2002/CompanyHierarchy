using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CompanyManagement.Application.UseCases
{
    /// <summary>
    /// Use case zodpovedny za ziskanie zoznamu zamestnancov
    /// priradenych ku konkretnemu oddeleniu.
    /// </summary>
    public class GetEmployeesByDepartment
    {
        /// <summary>
        /// Repozitar pre pracu s vazbou medzi oddeleniami a zamestnancami.
        /// </summary>
        private readonly IDepartmentEmployeeRepository _departmentEmployeeRepository;

        /// <summary>
        /// Repozitar pre pracu s uzlami organizacnej hierarchie.
        /// </summary>
        private readonly INodeRepository _nodeRepository;

        /// <summary>
        /// Inicializuje use case s potrebnym repozitarom.
        /// </summary>
        /// <param name="departmentEmployeeRepository">
        /// Repozitar pre priradenia zamestnancov k oddeleniam.
        /// </param>
        public GetEmployeesByDepartment(IDepartmentEmployeeRepository departmentEmployeeRepository, INodeRepository nodeRepository)
        {
            _departmentEmployeeRepository = departmentEmployeeRepository;
            _nodeRepository = nodeRepository;
        }

        /// <summary>
        /// Vrati zoznam zamestnancov priradenych ku konkretnemu oddeleniu.
        /// </summary>
        /// <param name="departmentId">Identifikator oddelenia.</param>
        /// <returns>
        /// Zoznam zamestnancov priradenych k danemu oddeleniu.
        /// </returns>
        public async Task<List<Employee>> ExecuteAsync(Guid departmentId)
        {
            var exists = await _nodeRepository.GetByIdAsync(departmentId);

            if (exists == null)
            {
                throw new KeyNotFoundException("Department not found");
            }

            if ((int)exists.Type != 4) 
            {
                throw new ValidationException("The specified node is not a department");
            }
            return await _departmentEmployeeRepository.GetEmployeesByDepartmentIdAsync(departmentId);
        }
    }
}
