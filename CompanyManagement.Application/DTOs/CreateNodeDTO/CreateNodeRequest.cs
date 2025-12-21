using CompanyManagement.Domain.Enums;

namespace CompanyManagement.Application.DTOs.CreateNodeDTO
{
    public class CreateNodeRequest
    {
        /// <summary>
        /// Human-readable name of the node.
        /// </summary>
        public string Name { get; init; } = string.Empty;

        /// <summary>
        /// Unique code identifying the node within the organization.
        /// </summary>
        public string Code { get; init; } = string.Empty;

        /// <summary>
        /// Type of the organizational node (e.g. Company, Division, Project).
        /// </summary>
        public NodeType Type { get; init; }

        /// <summary>
        /// Identifier of the parent node; null if the node is a root.
        /// </summary>
        public Guid? ParentId { get; init; }
    }
}
