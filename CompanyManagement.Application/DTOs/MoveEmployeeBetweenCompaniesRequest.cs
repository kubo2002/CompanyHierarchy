using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyManagement.Application.DTOs
{
    public class MoveEmployeeBetweenCompaniesRequest
    {
        /// <summary>
        /// ID of the employee to be moved.
        /// </summary>
        public Guid EmployeeId { get; init; }

        /// <summary>
        /// Id of the target company node where the employee will be moved.
        /// </summary>
        public Guid TargetCompanyNodeId { get; init; }
    }
}
