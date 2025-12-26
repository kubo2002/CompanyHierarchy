using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Application.DTOs;
using CompanyManagement.Domain.Enums;

namespace CompanyManagement.Application.UseCases
{
    public class UpdateNode
    {
        private readonly INodeRepository _nodeRepository;

        public UpdateNode(INodeRepository nodeRepository)
        {
            _nodeRepository = nodeRepository;
        }

        public async Task ExecuteAsync(UpdateNodeDto request)
        {
            var node = await _nodeRepository.GetByIdAsync(request.NodeId)
                ?? throw new KeyNotFoundException("Node not found");

            var changed = false;

            if (!string.IsNullOrWhiteSpace(request.Name) &&
                request.Name != node.Name)
            {
                node.UpdateName(request.Name);
                changed = true;
            }

            if (!string.IsNullOrWhiteSpace(request.Code) &&
                request.Code != node.Code)
            {
                node.UpdateCode(request.Code);
                changed = true;
            }

            if (request.ParentId.HasValue &&
                request.ParentId != node.ParentId)
            {
                var parent = await _nodeRepository.GetByIdAsync(request.ParentId.Value)
                    ?? throw new InvalidOperationException("Parent node not found");

                if ((int)parent.Type != (int)node.Type - 1)
                {
                    throw new InvalidOperationException(
                        $"Invalid parent type. Parent must be of type {(NodeType)((int)node.Type - 1)}");
                }

                node.ChangeParent(parent.Id);
                changed = true;
            }

            if (!changed)
                return;

            await _nodeRepository.UpdateAsync(node);
        }
    }
}