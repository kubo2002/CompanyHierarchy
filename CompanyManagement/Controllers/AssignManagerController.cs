using CompanyManagement.Api.Responses;
using CompanyManagement.Application.DTOs.AssignManagerToNodeDTO;
using CompanyManagement.Application.DTOs.CreateEmployeeDTO;
using CompanyManagement.Application.UseCases;
using CompanyManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManagement.Api.Controllers
{
    
    [ApiController]
    [Route("api/employees")]
    public class AssignManagerController : ControllerBase
    {

        private readonly AssignManagerToNode _assignManagerToNode;

        public AssignManagerController(AssignManagerToNode assignManager)
        {
            _assignManagerToNode = assignManager;
        }

        [HttpPost("assign-manager")]
        public async Task<IActionResult> AssignManager([FromBody] AssignManagerRequest request)
        {
            await _assignManagerToNode.ExecuteAsync(request);

            return Ok(ApiResponse<object>.Ok(null,"Manager assigned successfully"));
        }
    }
}
