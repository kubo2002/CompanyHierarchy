
using System.ComponentModel.DataAnnotations;

namespace CompanyManagement.Application.DTOs
{
    public class UpdateNodeDto
    {
        [Required(ErrorMessage = "Node ID is required.")]
        public Guid NodeId { get; init; }
        public string? Name { get; init; }
        public string? Code { get; init; }
        public Guid? ParentId { get; init; }
    }
}
