using CompanyManagement.Api.Responses;
using CompanyManagement.Application.DTOs.AssignEmployeeToNodeDTO;
using CompanyManagement.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManagement.Api.Controllers
{
    /// <summary>
    /// Controller zodpovedny za priradenie zamestnanca k oddeleniu
    /// (uzlu organizacnej hierarchie).
    /// </summary>
    [ApiController]
    [Route("api/employees")]
    public class AssignEmployeeToDepartmentController : ControllerBase
    {
        /// <summary>
        /// Use case zabezpecujuci priradenie zamestnanca k uzlu.
        /// </summary>
        private readonly AssignEmployeeToNode _assignEmployeeToNode;

        /// <summary>
        /// Inicializuje controller s potrebnym use casom.
        /// </summary>
        /// <param name="assignEmployee">
        /// Use case pre priradenie zamestnanca k uzlu.
        /// </param>
        public AssignEmployeeToDepartmentController(AssignEmployeeToNode assignEmployee)
        {
            _assignEmployeeToNode = assignEmployee;
        }

        /// <summary>
        /// Priradi zamestnanca k oddeleniu na zaklade vstupnych udajov.
        /// </summary>
        /// <param name="request">
        /// DTO obsahujuci identifikator uzla (oddelenia)
        /// a identifikator zamestnanca.
        /// </param>
        /// <returns>
        /// HTTP 200 OK po uspesnom priradeni zamestnanca.
        /// </returns>
        [HttpPost("assign-employee")]
        public async Task<IActionResult> AssignEmployee(
            [FromBody] AssignEmployeeToNodeRequest request)
        {
            await _assignEmployeeToNode.ExecuteAsync(request.NodeId, request.EmployeeId);

            return Ok(ApiResponse<object>.Ok(null,"Employee assigned to department successfully"));
        }
    }
}
