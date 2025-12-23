using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Application.DTOs;
using CompanyManagement.Application.DTOs.NodeTreeDTO;
using CompanyManagement.Domain.Entities;

namespace CompanyManagement.Application.UseCases
{
    /// <summary>
    /// Use case zodpovedny za zostavenie stromovej struktury uzlov
    /// (Node) na zaklade hierarchie parent-child vztahov.
    /// 
    /// Umoznuje ziskat cely podstrom od zvoleneho uzla,
    /// napr. Company -> Division -> Department.
    /// </summary>
    public class GetNodeTree
    {
        /// <summary>
        /// Repozitar pre pristup k uzlom organizacnej hierarchie.
        /// </summary>
        private readonly INodeRepository _nodeRepository;

        /// <summary>
        /// Repozitar pre pristup k zamestnancom.
        /// </summary>
        private readonly IEmployeeRepository _employeeRepository;

        /// <summary>
        /// Inicializuje use case s potrebnymi repozitarmi.
        /// </summary>
        /// <param name="nodeRepository">
        /// Repozitar pouzivany na nacitanie uzlov a ich potomkov.
        /// </param>
        /// <param name="employeeRepository">
        /// Repozitar pouzivany na nacitanie udajov o zamestnancoch.
        /// </param>
        public GetNodeTree(INodeRepository nodeRepository, IEmployeeRepository employeeRepository)
        {
            _nodeRepository = nodeRepository;
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Spusti zostavenie stromu uzlov od zadaneho root uzla.
        ///
        /// Root uzol je sucastou vysledku a nachadza sa
        /// na najvyssej urovni stromu.
        /// </summary>
        /// <param name="rootNodeId">
        /// Identifikator uzla, od ktoreho sa ma zacat budovanie stromu.
        /// </param>
        /// <returns>
        /// Stromova struktura uzlov reprezentovana pomocou NodeTreeDto.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// Vyhodi sa, ak uzol so zadanym ID neexistuje.
        /// </exception>
        public async Task<NodeTreeDto> ExecuteAsync(Guid rootNodeId)
        {
            var root = await _nodeRepository.GetByIdAsync(rootNodeId)
                ?? throw new KeyNotFoundException("Node not found");

            return await BuildTreeAsync(root);
        }

        /// <summary>
        /// Rekurzivne zostavi strom uzlov od zadaneho uzla.
        ///
        /// Metoda:
        /// 1. Vytvori DTO pre aktualny uzol
        /// 2. Nacita jeho priamych potomkov
        /// 3. Pre kazde dieta rekurzivne zostavi jeho podstrom
        /// </summary>
        /// <param name="node">
        /// Aktualny uzol, pre ktory sa vytvara stromova struktura.
        /// </param>
        /// <returns>
        /// Strom uzlov reprezentovany ako NodeTreeDto.
        /// </returns>
        private async Task<NodeTreeDto> BuildTreeAsync(Node node)
        {
            ManagerDto? manager = null;

            // Ak ma uzol priradeneho veduceho, nacitame jeho udaje
            if (node.LeaderEmployeeId != null)
            {
                var leader = await _employeeRepository.GetByIdAsync(node.LeaderEmployeeId.Value);
                if (leader != null)
                {
                    manager = new ManagerDto
                    {
                        Id = leader.Id,
                        FirstName = leader.FirstName,
                        LastName = leader.LastName,
                        Email = leader.Email
                    };
                }
            }

            // Vytvorenie DTO pre aktualny uzol
            var dto = new NodeTreeDto
            {
                Id = node.Id,
                Name = node.Name,
                Code = node.Code,
                Type = node.Type,
                Manager = manager,
                Children = new List<NodeTreeDto>()
            };

            // Nacitanie priamych potomkov aktualneho uzla
            var children = await _nodeRepository.GetChildrenAsync(node.Id);

            // Rekurzivne zostavenie podstromov pre kazde dieta
            foreach (var child in children)
            {
                dto.Children.Add(await BuildTreeAsync(child));
            }

            return dto;
        }
    }
}
