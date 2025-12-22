using System.ComponentModel.DataAnnotations;

namespace CompanyManagement.Application.DTOs.UnassignManagerDTO
{
    public class UnassignManagerFromNodeRequest
    {
        public Guid NodeId { get; init; }
    }
}
