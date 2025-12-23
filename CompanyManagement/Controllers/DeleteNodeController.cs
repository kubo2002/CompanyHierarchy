using CompanyManagement.Api.Responses;
using CompanyManagement.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManagement.Api.Controllers
{
    /// <summary>
    /// Controller zodpovedny za mazanie uzlov organizacnej hierarchie.
    /// Umoznuje odstranenie uzla vratane celeho jeho podstromu.
    /// </summary>
    [ApiController]
    [Route("api/nodes")]
    public class DeleteNodeController : ControllerBase
    {
        /// <summary>
        /// Use case zabezpecujuci odstranenie uzla a jeho podstromu.
        /// </summary>
        private readonly DeleteNode _deleteNode;

        /// <summary>
        /// Inicializuje controller s potrebnym use casom.
        /// </summary>
        /// <param name="deleteNode">Use case pre mazanie uzlov.</param>
        public DeleteNodeController(DeleteNode deleteNode)
        {
            _deleteNode = deleteNode;
        }

        /// <summary>
        /// Odstrani uzol organizacnej hierarchie vratane celeho jeho podstromu.
        /// </summary>
        /// <param name="nodeId">Identifikator uzla.</param>
        /// <returns>
        /// HTTP 200 OK s informaciou o uspesnom odstraneni.
        /// </returns>
        [HttpDelete("{nodeId:guid}")]
        public async Task<IActionResult> Delete(Guid nodeId)
        {
            await _deleteNode.ExecuteAsync(nodeId);

            return Ok(ApiResponse<object>.Ok(null,"Node (subtree) deleted successfully"));
        }
    }
}
