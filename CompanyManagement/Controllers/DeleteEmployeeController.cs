using CompanyManagement.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManagement.Api.Controllers
{
    /// <summary>
    /// Controller zodpovedny za mazanie zamestnancov.
    /// Sluzi ako HTTP rozhranie pre DeleteEmployee use case.
    /// </summary>
    [ApiController]
    [Route("api/employees")]
    public class DeleteEmployeeController : ControllerBase
    {
        /// <summary>
        /// Use case zabezpecujuci odstranenie zamestnanca zo systemu.
        /// </summary>
        private readonly DeleteEmployee _deleteEmployee;

        /// <summary>
        /// Inicializuje controller s potrebnym use casom.
        /// </summary>
        /// <param name="deleteEmployee">Use case pre mazanie zamestnanca.</param>
        public DeleteEmployeeController(DeleteEmployee deleteEmployee)
        {
            _deleteEmployee = deleteEmployee;
        }

        /// <summary>
        /// Odstrani zamestnanca zo systemu na zaklade jeho identifikatora.
        /// </summary>
        /// <param name="employeeId">Identifikator zamestnanca.</param>
        /// <returns>
        /// HTTP 204 No Content po uspesnom odstraneni.
        /// </returns>
        [HttpDelete("{employeeId:guid}")]
        public async Task<IActionResult> Delete(Guid employeeId)
        {
            await _deleteEmployee.ExecuteAsync(employeeId);

            return NoContent(); // 204
        }
    }
}
