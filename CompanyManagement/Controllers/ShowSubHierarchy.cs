using CompanyManagement.Api.Responses;
using CompanyManagement.Application.DTOs.NodeTreeDTO;
using CompanyManagement.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManagement.Api.Controllers
{
    /// <summary>
    /// Controller zodpovedny za nacitanie stromovej struktury uzlov.
    /// Umoznuje ziskat cely podstrom od zvoleneho uzla.
    /// </summary>
    [ApiController]
    [Route("api/nodes")]
    public class ShowSubHierarchy : ControllerBase
    {
        /// <summary>
        /// Use case pre zostavenie stromovej struktury uzlov.
        /// </summary>
        private readonly GetNodeTree _getNodeTree;

        /// <summary>
        /// Inicializuje controller s potrebnym use casom.
        /// </summary>
        /// <param name="getNodeTree">Use case pre zostavenie stromu uzlov.</param>
        public ShowSubHierarchy(GetNodeTree getNodeTree)
        {
            _getNodeTree = getNodeTree;
        }

        /// <summary>
        /// Vrati stromovu strukturu uzlov od zadaneho uzla.
        /// </summary>
        /// <param name="nodeId">Identifikator uzla.</param>
        /// <returns>
        /// HTTP 200 OK so stromovou strukturou uzlov.
        /// </returns>
        [HttpGet("{nodeId}/tree")]
        public async Task<IActionResult> GetTree(Guid nodeId)
        {
            var tree = await _getNodeTree.ExecuteAsync(nodeId);

            return Ok(ApiResponse<NodeTreeDto>.Ok(tree,"Node tree loaded successfully"));
        }
    }
}
