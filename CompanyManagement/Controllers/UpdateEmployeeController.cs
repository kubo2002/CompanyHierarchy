using CompanyManagement.Api.Responses;
using CompanyManagement.Application.DTOs;
using CompanyManagement.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManagement.Api.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class UpdateEmployeeController : ControllerBase
    {

        private readonly UpdateEmployee _updateEmployee;
        public UpdateEmployeeController(UpdateEmployee updateEmployee)
        {
            _updateEmployee = updateEmployee;
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateEmployee([FromBody] UpdateEmployeeRequest request)
        {
            await _updateEmployee.ExecuteAsync(request);

            return Ok(ApiResponse<object>.Ok(null,"Employee updated successfully"));
        }
    }
}
