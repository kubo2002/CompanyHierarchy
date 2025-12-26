using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Application.DTOs;

namespace CompanyManagement.Application.UseCases
{
    public class UpdateEmployee
    {
        private readonly IEmployeeRepository _employeeRepository;

        public UpdateEmployee(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task ExecuteAsync(UpdateEmployeeRequest request)
        {
            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);

            if (employee == null)
            {
                throw new KeyNotFoundException("Employee not found");
            }

            var changed = false;

            if (!string.IsNullOrWhiteSpace(request.FirstName) && request.FirstName != employee.FirstName)
            {
                employee.UpdateFirstName(request.FirstName);
                changed = true;
            }

            if (!string.IsNullOrWhiteSpace(request.LastName) && request.LastName != employee.LastName)
            {
                employee.UpdateLastName(request.LastName);
                changed = true;
            }

            if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != employee.Email)
            {
                var emailExists = await _employeeRepository.ExistsByEmailAsync(request.Email);

                if (emailExists) 
                {
                    throw new InvalidOperationException("Employee with this email already exists");
                }
             
                employee.UpdateEmail(request.Email);
                changed = true;
            }

            if (request.Phone != employee.Phone)
            {
                employee.UpdatePhone(request.Phone);
                changed = true;
            }

            if (!changed)
            {
                return;
            }
     
            await _employeeRepository.UpdateEmployeeParametersAsync(employee);
        }
    }
}
