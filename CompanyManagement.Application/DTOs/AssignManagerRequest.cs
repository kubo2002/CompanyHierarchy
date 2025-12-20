namespace CompanyManagement.Application.DTOs
{
    public class AssignManagerRequest
    {
        /// <summary>
        /// Id of the node to which the employee will be assigned as manager
        /// </summary>
        public Guid NodeId { get; init; }

        /// <summary>
        /// Id of the employee to be assigned as manager
        /// </summary>
        public Guid EmployeeId { get; init; }
    }
}
