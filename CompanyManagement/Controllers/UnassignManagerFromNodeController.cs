using CompanyManagement.Api.Responses;
using CompanyManagement.Application.DTOs.UnassignManagerDTO;
using CompanyManagement.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManagement.Api.Controllers
{
    
    [ApiController]
    [Route("api/employees")]
    public class UnassignManagerFromNodeController : ControllerBase
    {
        private readonly UnassignManagerFromNode _unassignManagerFromNode;

        public UnassignManagerFromNodeController(UnassignManagerFromNode unassignManagerFromNode)
        {
            _unassignManagerFromNode = unassignManagerFromNode;
        }

        [HttpPost("unassign-manager")]
        public async Task<IActionResult> AssignEmployee(
            [FromBody] UnassignManagerFromNodeRequest request)
        {
            await _unassignManagerFromNode.ExecuteAsync(request);
            return Ok(ApiResponse<object>.Ok(null, "Manager removed from node successfully."));

            //TODO treba lepsie vypisovat chybove hlasky v pripade neuspechu
        }
    }
    
}
