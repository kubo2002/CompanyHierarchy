using CompanyManagement.Domain.Enums;

namespace CompanyManagement.Application.DTOs
{
    public class ParentNodeDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Code { get; init; } = string.Empty;
        public NodeType Type { get; init; }
    }
}
