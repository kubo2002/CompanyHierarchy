using CompanyManagement.Api.Responses;
using CompanyManagement.Application.DTOs;
using CompanyManagement.Application.UseCases;
using CompanyManagement.Domain.Enums;
using Microsoft.AspNetCore.Mvc;


namespace CompanyManagement.Api.Controllers
{
    /// <summary>
    /// Controller zodpovedny za dotazovanie uzlov organizacnej hierarchie.
    /// Umoznuje ziskat uzly filtrovanie podla ich typu.
    /// </summary>
    [ApiController]
    [Route("api/nodes")]
    public class NodesQueryController : ControllerBase
    {
        /// <summary>
        /// Use case pre ziskanie uzlov na zaklade typu.
        /// </summary>
        private readonly GetNodesByType _getNodesByType;

        /// <summary>
        /// Inicializuje controller s potrebnym use casom.
        /// </summary>
        /// <param name="getNodesByType">Use case pre nacitanie uzlov podla typu.</param>
        public NodesQueryController(GetNodesByType getNodesByType)
        {
            _getNodesByType = getNodesByType;
        }

        /// <summary>
        /// Vrati zoznam uzlov filtrovanich podla zadanych typov.
        ///
        /// Typy uzlov sa zadavaju ako retazec oddeleny ciarkami,
        /// napr. "Division,Department".
        /// </summary>
        /// <param name="types">
        /// Zoznam typov uzlov vo forme retazca oddeleneho ciarkami.
        /// </param>
        /// <returns>
        /// HTTP 200 OK so zoznamom uzlov.
        /// </returns>
        [HttpGet("by-type")]
        public async Task<IActionResult> GetByType(
            [FromQuery] string types)
        {
            // Rozparsovanie retazca typov (napr. "Division,Department")
            var parsedTypes = types
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => Enum.Parse<NodeType>(t, ignoreCase: true))
                .ToList();

            // Vykonanie use casu
            var nodes = await _getNodesByType.ExecuteAsync(parsedTypes);

            // Mapovanie vysledku na DTO objekty
            var result = nodes
                .Select(node => new NodeListDto
                {
                    Id = node.Id,
                    Name = node.Name,
                    Code = node.Code,
                    Type = node.Type,
                    Parent = node.Parent
                })
                .ToList();

            return Ok(ApiResponse<List<NodeListDto>>.Ok(result,"Nodes loaded by type"));
        }
    }
}
