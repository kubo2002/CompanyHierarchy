using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Domain.Entities;

namespace CompanyManagement.Application.UseCases
{
    /// <summary>
    /// Use case zodpovedny za odstranenie uzla z organizacnej hierarchie.
    ///
    /// Pri odstraneni uzla sa vymaze cely jeho podstrom,
    /// teda vsetky potomkovske uzly.
    /// </summary>
    public class DeleteNode
    {
        /// <summary>
        /// Repozitar pre pracu s uzlami organizacnej hierarchie.
        /// </summary>
        private readonly INodeRepository _nodeRepository;

        /// <summary>
        /// Inicializuje use case s potrebnym repozitarom.
        /// </summary>
        /// <param name="nodeRepository">Repozitar uzlov.</param>
        public DeleteNode(INodeRepository nodeRepository)
        {
            _nodeRepository = nodeRepository;
        }

        /// <summary>
        /// Vykona odstranenie uzla a celeho jeho podstromu.
        ///
        /// Mazanie prebieha post-order sposobom,
        /// teda od listov smerom ku korenu.
        /// </summary>
        /// <param name="nodeId">Identifikator uzla, ktory sa ma odstranit.</param>
        /// <exception cref="KeyNotFoundException">
        /// Vyhodi sa, ak uzol so zadanym ID neexistuje.
        /// </exception>
        public async Task ExecuteAsync(Guid nodeId)
        {
            var root = await _nodeRepository.GetByIdAsync(nodeId)
            ?? throw new KeyNotFoundException("Node not found");

            var nodesToDelete = new List<Node>();

            // Zostavenie zoznamu uzlov v podstrome (post order)
            await CollectSubtreeAsync(root, nodesToDelete);

            // Mazanie uzlov od listov smerom hore
            foreach (var node in nodesToDelete.AsEnumerable().Reverse())
            {
                await _nodeRepository.DeleteAsync(node);
            }
        }

        /// <summary>
        /// Rekurzivne prejde cely podstrom daneho uzla
        /// post-order algoritmom a ulozi uzly do zoznamu.
        /// </summary>
        /// <param name="node">
        /// Uzol, ktory je korenom aktualne spracovavaneho podstromu.
        /// </param>
        /// <param name="accumulator">
        /// Zoznam, do ktoreho sa postupne ukladaju uzly na odstranenie.
        /// </param>
        private async Task CollectSubtreeAsync(Node node, List<Node> accumulator)
        {
            var children = await _nodeRepository.GetChildrenAsync(node.Id);

            foreach (var child in children)
            {
                await CollectSubtreeAsync(child, accumulator);
            }

            accumulator.Add(node);
        }
    }
}
