using System.ComponentModel.DataAnnotations;

namespace CompanyManagement.Application.DTOs.CreateEmployeeDTO
{
    public class CreateEmployeeRequest
    {
      
        public string FirstName { get; init; } = string.Empty;
        
        public string LastName { get; init; } = string.Empty;
        
        public string Email { get; init; } = string.Empty;
    
        public string Phone { get; init; } = string.Empty;
    }
}
