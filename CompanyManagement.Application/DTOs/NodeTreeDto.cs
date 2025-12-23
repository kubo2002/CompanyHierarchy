using CompanyManagement.Domain.Enums;


namespace CompanyManagement.Application.DTOs.NodeTreeDTO
{
    public class NodeTreeDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Code { get; init; } = string.Empty;
        public NodeType Type { get; init; }
        public ManagerDto? Manager { get; init; }
        public List<NodeTreeDto> Children { get; init; } = new();
    }
}
