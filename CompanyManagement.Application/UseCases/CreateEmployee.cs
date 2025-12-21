using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Application.DTOs.CreateEmployeeDTO;
using CompanyManagement.Domain.Entities;

namespace CompanyManagement.Application.UseCases
{
    /// <summary>
    /// Represents the use case for creating a new employee.
    /// </summary>
    public class CreateEmployee
    {
        private readonly IEmployeeRepository _employeeRepository;

        public CreateEmployee(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Creates a new employee and persists it using the employee repository.
        /// </summary>
        /// <param name="request">
        /// Request containing the employee's personal and contact information.
        /// </param>
        /// <returns>
        /// The unique identifier (<see cref="Guid"/>) of the newly created employee.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when first name, last name, or email is null, empty, or contains only whitespace.
        /// </exception>
        /// <remarks>
        /// This use case performs basic input validation only.
        /// It does not check for email uniqueness or assign the employee to any organizational node.
        /// </remarks>
        public async Task<Guid> ExecuteAsync(CreateEmployeeRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FirstName))
                throw new ArgumentException("First name is required");

            if (string.IsNullOrWhiteSpace(request.LastName))
                throw new ArgumentException("Last name is required");

            if (string.IsNullOrWhiteSpace(request.Email))
                throw new ArgumentException("Email is required");

            var employee = new Employee(
                Guid.NewGuid(),
                request.FirstName,
                request.LastName,
                request.Email,
                request.Phone
            );

            await _employeeRepository.AddAsync(employee);

            return employee.Id;
        }
    }
}
