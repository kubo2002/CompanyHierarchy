using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CompanyManagement.Application.DTOs.AssignEmployeeToNodeDTO
{
    public class AssignEmployeeToNodeRequest
    {
        [Required(ErrorMessage = "Id of department is required.")]
        public Guid NodeId { get; init; }
        [Required(ErrorMessage = "Id of employee is required.")]
        public Guid EmployeeId { get; init; }
    }
}
