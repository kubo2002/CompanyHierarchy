using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyManagement.Application.DTOs
{
    public class RemoveEmployeeFromNodeRequest
    {
        public Guid NodeId { get; init; }
        public Guid EmployeeId { get; init; }
    }
}
