using CompanyManagement.Api.Responses;
using CompanyManagement.Application.DTOs.AssignEmployeeToNodeDTO;
using CompanyManagement.Application.DTOs.AssignManagerToNodeDTO;
using CompanyManagement.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManagement.Api.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class AssignEmployeeToNodeController : ControllerBase
    {
        private readonly AssignEmployeeToNode _assignEmployeeToNode;

        public AssignEmployeeToNodeController(AssignEmployeeToNode assignEmployee)
        {
            _assignEmployeeToNode = assignEmployee;
        }

        [HttpPost("assign-employee")]
        public async Task<IActionResult> AssignEmployee(
            [FromBody] AssignEmployeeToNodeRequest request)
        {
            await _assignEmployeeToNode.ExecuteAsync(request.NodeId, request.EmployeeId);

            return Ok(ApiResponse<object>.Ok(null,"Employee assigned to department successfully"));
        }
    }
}
