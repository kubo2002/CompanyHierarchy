using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Application.DTOs.CreateNodeDTO;
using CompanyManagement.Domain.Entities;
using CompanyManagement.Domain.Enums;

namespace CompanyManagement.Application.UseCases
{
    /// <summary>
    /// Represents the use case for creating a new node in the company management system.
    /// </summary>
    public class CreateNode
    {
        private readonly INodeRepository _nodeRepository;

        public CreateNode(INodeRepository nodeRepository)
        {
            _nodeRepository = nodeRepository;
        }

        /// <summary>
        /// Creates a new organizational node and persists it using the node repository.
        /// </summary>
        /// <param name="request">
        /// Request object containing the data required to create a new node
        /// (name, code, type, and optional parent identifier).
        /// </param>
        /// <returns>
        /// The unique identifier (<see cref="Guid"/>) of the newly created node.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the node name or node code is null, empty, or contains only whitespace.
        /// </exception>
        /// <remarks>
        /// This method performs basic input validation and delegates persistence
        /// to the underlying repository. It does not verify the existence of the parent node.
        /// </remarks>
        public async Task<Guid> ExecuteAsync(CreateNodeRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name)) 
            {
                throw new ArgumentException("Node name is required");
            }
             
            if (string.IsNullOrWhiteSpace(request.Code)) 
            {
                throw new ArgumentException("Node code is required");
            }
              
            if (request.Type == NodeType.Company && request.ParentId != null) 
            {
                throw new InvalidOperationException("Company node cannot have a parent");
            }

            if (request.ParentId != null)
            {
                var parent = await _nodeRepository.GetByIdAsync(request.ParentId.Value)
                    ?? throw new ArgumentException("Parent node not found");

                // Department nemôže mať deti, ma len zamestnancov
                if (parent.Type == NodeType.Department) 
                {
                    throw new InvalidOperationException("Department node cannot have children");
                }
            }

            var node = new Node(
                Guid.NewGuid(),
                request.Name,
                request.Code,
                request.Type,
                request.ParentId
            );

            await _nodeRepository.AddAsync(node);

            return node.Id;
        }
    }
}
