using CompanyManagement.Api.Responses;
using CompanyManagement.Application.DTOs.CreateEmployeeDTO;
using CompanyManagement.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManagement.Api.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly CreateEmployee _createEmployee;

        /// <summary>
        /// API controller pre spravu zamestnancov.
        /// Zodpoveda za spracovanie HTTP poziadaviek tykajucich sa zamestnancov.
        /// </summary>
        public EmployeesController(CreateEmployee createEmployee)
        {
            _createEmployee = createEmployee;
        }

        /// <summary>
        /// Vytvori noveho zamestnanca.
        /// </summary>
        /// <param name="request">
        /// Vstupne udaje potrebne na vytvorenie noveho zamestnanca.
        /// </param>
        /// <returns>
        /// HTTP 201 Created s identifikatorom vytvoreneho zamestnanca.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> CreateEmployee(
            [FromBody] CreateEmployeeRequest request)
        {
            var employeeId = await _createEmployee.ExecuteAsync(request);

            return CreatedAtAction(nameof(CreateEmployee), new { id = employeeId }, ApiResponse<CreateEmployeeResponse>.Ok(new CreateEmployeeResponse { Id = employeeId },"Employee created successfully"));
        }
    }
}
