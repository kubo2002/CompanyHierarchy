using CompanyManagement.Api.Responses;
using CompanyManagement.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManagement.Api.Controllers
{
    /// <summary>
    /// Controller zodpovedny za odobratie zamestnanca z oddelenia.
    /// Deleguje spracovanie na prislusny use case.
    /// </summary>
    [ApiController]
    [Route("api/employees")]
    public class UnassignEmployeeFromDepartmentController : ControllerBase
    {
        /// <summary>
        /// Use case zabezpecujuci odobratie zamestnanca z oddelenia.
        /// </summary>
        private readonly UnassignEmployeeFromDepartment _unassignEmployee;

        /// <summary>
        /// Inicializuje controller s potrebnym use casom.
        /// </summary>
        /// <param name="unassignEmployee">
        /// Use case pre odobratie zamestnanca z oddelenia.
        /// </param>
        public UnassignEmployeeFromDepartmentController(UnassignEmployeeFromDepartment unassignEmployee)
        {
            _unassignEmployee = unassignEmployee;
        }

        /// <summary>
        /// Odoberie zamestnanca z oddelenia.
        /// </summary>
        /// <param name="departmentId">Identifikator oddelenia.</param>
        /// <param name="employeeId">Identifikator zamestnanca.</param>
        /// <returns>
        /// HTTP 200 OK po uspesnom odobrati zamestnanca.
        /// </returns>
        [HttpPost("{employeeId:guid}/unassign")]
        public async Task<IActionResult> Unassign(Guid employeeId)
        {
            await _unassignEmployee.ExecuteAsync(employeeId);

            return Ok(ApiResponse<object>.Ok(null,"Employee unassigned from department"));
        }
    }
}
