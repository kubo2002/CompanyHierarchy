using CompanyManagement.Api.Responses;
using CompanyManagement.Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using CompanyManagement.Application.DTOs;

namespace CompanyManagement.Api.Controllers
{
    /// <summary>
    /// Controller zodpovedny za ziskanie zoznamu zamestnancov
    /// priradenych ku konkretnemu oddeleniu.
    /// </summary>
    [ApiController]
    [Route("api/departments")]
    public class DepartmentEmployeesController : ControllerBase
    {
        /// <summary>
        /// Use case pre ziskanie zamestnancov podla oddelenia.
        /// </summary>
        private readonly GetEmployeesByDepartment _getEmployeesByDepartment;

        /// <summary>
        /// Inicializuje controller s potrebnym use casom.
        /// </summary>
        /// <param name="getEmployeesByDepartment">
        /// Use case pre nacitanie zamestnancov z oddelenia.
        /// </param>
        public DepartmentEmployeesController(GetEmployeesByDepartment getEmployeesByDepartment)
        {
            _getEmployeesByDepartment = getEmployeesByDepartment;
        }

        /// <summary>
        /// Vrati zoznam zamestnancov priradenych ku konkretnemu oddeleniu.
        /// </summary>
        /// <param name="departmentId">Identifikator oddelenia.</param>
        /// <returns>
        /// HTTP 200 OK so zoznamom zamestnancov.
        /// </returns>
        [HttpGet("{departmentId:guid}/employees")]
        public async Task<IActionResult> GetEmployees(Guid departmentId)
        {
            var employees = await _getEmployeesByDepartment.ExecuteAsync(departmentId);

            var result = employees.Select(EmployeeListDto.From).ToList();

            return Ok(ApiResponse<List<EmployeeListDto>>.Ok(result,"Employees loaded successfully"));
        }
    }
}
