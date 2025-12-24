using CompanyManagement.Api.Responses;
using CompanyManagement.Application.DTOs.CreateNodeDTO;
using CompanyManagement.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManagement.Api.Controllers
{
    [ApiController]
    [Route("api/nodes")]
    public class CreateNodeController : ControllerBase
    {
        private readonly CreateNode _createNode;

        /// <summary>
        /// API controller pre pracu s uzlami organizacnej struktury.
        /// Zodpoveda za spracovanie HTTP poziadaviek tykajucich sa uzlov.
        /// </summary>
        public CreateNodeController(CreateNode createNode)
        {
            _createNode = createNode;
        }

        /// <summary>
        /// Vytvori novy uzol organizacnej struktury.
        /// </summary>
        /// <param name="request">
        /// Vstupne udaje potrebne na vytvorenie noveho uzla.
        /// </param>
        /// <returns>
        /// HTTP 201 Created s identifikatorom vytvoreneho uzla.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> CreateNode([FromBody] CreateNodeRequest request)
        {
            var nodeId = await _createNode.ExecuteAsync(request);

            return CreatedAtAction(nameof(CreateNode), new { id = nodeId }, ApiResponse<CreateNodeResponse>.Ok(new CreateNodeResponse { Id = nodeId },"Node created successfully"));
        }
    }
}
