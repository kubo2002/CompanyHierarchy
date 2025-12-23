using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Domain.Entities;

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
        /// Inicializuje use case s potrebnym repozitarom.
        /// </summary>
        /// <param name="departmentEmployeeRepository">
        /// Repozitar pre priradenia zamestnancov k oddeleniam.
        /// </param>
        public GetEmployeesByDepartment(IDepartmentEmployeeRepository departmentEmployeeRepository)
        {
            _departmentEmployeeRepository = departmentEmployeeRepository;
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
            return await _departmentEmployeeRepository.GetEmployeesByDepartmentIdAsync(departmentId);
        }
    }
}
