using CompanyManagement.Domain.Entities;
namespace CompanyManagement.Application.DTOs
{
    public class EmployeeListDto
    {
        public Guid Id { get; init; }
        public string? AcademicTitle { get; init; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;

        public static EmployeeListDto From(Employee employee)
        {
            return new EmployeeListDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                AcademicTitle = employee.AcademicTitle
            };
        }
    }
}
