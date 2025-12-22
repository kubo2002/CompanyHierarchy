using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Application.DTOs.UnassignManagerDTO;

namespace CompanyManagement.Application.UseCases
{
    public class UnassignManagerFromNode
    {
        private readonly INodeRepository _nodeRepository;

        public UnassignManagerFromNode(INodeRepository nodeRepository)
        {
            _nodeRepository = nodeRepository;
        }

        /// <summary>
        /// Removes the assigned manager (leader) from an organizational node.
        /// </summary>
        /// <param name="request">
        /// Request containing the identifier of the node from which the manager should be unassigned.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when the specified node does not exist.
        /// </exception>
        /// <remarks>
        /// If the node does not currently have a manager assigned,
        /// the operation completes without making any changes.
        /// </remarks>
        public async Task ExecuteAsync(UnassignManagerFromNodeRequest request)
        {
            var node = await _nodeRepository.GetByIdAsync(request.NodeId)
                        ?? throw new KeyNotFoundException("Node not found");

            if (node.LeaderEmployeeId == null)
            {
                throw new InvalidOperationException("Node does not have an assigned manager.");
            }

            node.UnassignLeader();

            await _nodeRepository.UpdateAsync(node);
        }
    }
}
