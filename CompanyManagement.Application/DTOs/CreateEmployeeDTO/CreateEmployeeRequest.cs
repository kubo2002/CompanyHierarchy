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
        public string Email { get; init; } = string.Empty;
        [Required(ErrorMessage = "Phone number is required.")]
        public string Phone { get; init; } = string.Empty;
    }
}
