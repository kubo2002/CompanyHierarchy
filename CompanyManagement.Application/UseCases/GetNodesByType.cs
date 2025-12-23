
using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Application.DTOs;
using CompanyManagement.Domain.Enums;

namespace CompanyManagement.Application.UseCases
{
    /// <summary>
    /// Use case pre ziskanie zoznamu uzlov na zaklade ich typu.
    /// Sluzba vracia zakladne informacie o uzloch vratane informacii
    /// o rodicovskom uzle, ak existuje.
    /// </summary>
    public class GetNodesByType
    {
        /// <summary>
        /// Repozitar pre pristup k uzlom organizacnej hierarchie.
        /// </summary>
        private readonly INodeRepository _nodeRepository;

        /// <summary>
        /// Inicializuje use case s potrebnym repozitarom.
        /// </summary>
        /// <param name="nodeRepository">Repozitar uzlov.</param>
        public GetNodesByType(INodeRepository nodeRepository)
        {
            _nodeRepository = nodeRepository;
        }

        /// <summary>
        /// Vrati zoznam uzlov filtrovanych podla zadanych typov.
        /// Ku kazdemu uzlu pripoji informacie o jeho rodicovi, ak existuje.
        /// </summary>
        /// <param name="types">Kolekcia typov uzlov, ktore sa maju ziskat.</param>
        /// <returns>
        /// Zoznam DTO objektov reprezentujucich uzly.
        /// </returns>
        public async Task<List<NodeListDto>> ExecuteAsync(IEnumerable<NodeType> types)
        {
            // Ziskanie uzlov podla typu z repozitara
            var nodes = await _nodeRepository.GetByTypesAsync(types);

            var result = new List<NodeListDto>();

            // Transformacia domenovych entit na DTO objekty
            foreach (var node in nodes)
            {
                ParentNodeDto? parentDto = null;

                // Ak ma uzol rodica, nacitame jeho zakladne informacie
                if (node.ParentId != null)
                {
                    var parent = await _nodeRepository.GetByIdAsync(node.ParentId.Value);

                    if (parent != null)
                    {
                        parentDto = new ParentNodeDto
                        {
                            Id = parent.Id,
                            Name = parent.Name,
                            Code = parent.Code,
                            Type = parent.Type
                        };
                    }
                }

                result.Add(new NodeListDto
                {
                    Id = node.Id,
                    Name = node.Name,
                    Code = node.Code,
                    Type = node.Type,
                    Parent = parentDto
                });
            }

            return result;
        }
    }
}
