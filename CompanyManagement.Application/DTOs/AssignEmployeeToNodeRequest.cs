using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyManagement.Application.DTOs
{
    public class AssignEmployeeToNodeRequest
    {
        public Guid NodeId { get; init; }
        public Guid EmployeeId { get; init; }
    }
}
