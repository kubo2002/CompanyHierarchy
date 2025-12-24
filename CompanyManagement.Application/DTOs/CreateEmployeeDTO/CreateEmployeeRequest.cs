using System.ComponentModel.DataAnnotations;

namespace CompanyManagement.Application.DTOs.CreateEmployeeDTO
{
    public class CreateEmployeeRequest
    {

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; init; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; init; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; init; } = string.Empty;

        [Required(ErrorMessage = "Phone is required")]
        public string Phone { get; init; } = string.Empty;
        public string? AcademicTitle { get; init; } = string.Empty;
    }
}
