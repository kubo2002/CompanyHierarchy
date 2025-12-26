using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace CompanyManagement.Application.DTOs
{
    public class UpdateEmployeeRequest
    {
        [Required(ErrorMessage = "Employee ID is required.")]
        public Guid EmployeeId { get; init; }
        public string? FirstName { get; init; } = String.Empty;
        public string? LastName { get; init; } = String.Empty;
        [EmailAddress(ErrorMessage = "Incorrect email format.")]
        public string? Email { get; init; }
        public string? Phone { get; init; } = String.Empty;
        public string? AcademicTitle { get; init; } = String.Empty;
    }
}
