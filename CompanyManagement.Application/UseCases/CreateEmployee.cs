using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Application.DTOs.CreateEmployeeDTO;
using CompanyManagement.Domain.Entities;
using System.ComponentModel.DataAnnotations;

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
        /// Employee with this email already exists
        /// </exception>
        /// <remarks>
        /// It does not check for email uniqueness or assign the employee to any organizational node.
        /// </remarks>
        public async Task<Guid> ExecuteAsync(CreateEmployeeRequest request)
        {
            
            if (await _employeeRepository.ExistsByEmailAsync(request.Email)) 
            {
                throw new ValidationException("Employee with this email already exists");
            }

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
