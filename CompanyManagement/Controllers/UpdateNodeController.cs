using CompanyManagement.Api.Responses;
using CompanyManagement.Application.DTOs;
using CompanyManagement.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManagement.Api.Controllers
{
    [ApiController]
    [Route("api/nodes")]
    public class UpdateNodeController : ControllerBase
    {
        private readonly UpdateNode _updateNode;

        public UpdateNodeController(UpdateNode updateNode)
        {
            _updateNode = updateNode;
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateNode([FromBody] UpdateNodeDto request)
        {
            await _updateNode.ExecuteAsync(request);

            return Ok(ApiResponse<object>.Ok(null,"Node updated successfully"));
        }
    }
}
