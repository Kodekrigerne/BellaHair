using BellaHair.Domain.Employees;

namespace BellaHair.Domain.Bookings
{
    //Dennis
    /// <summary>
    /// Represents a Customer as a Snapshot in time for invoicing and historic data reasons<br/>
    /// Contains only the relevant information for these purposes, and so omits relational data
    /// </summary>
    public record EmployeeSnapshot
    {
        public Guid EmployeeId { get; private init; }
        public string FullName { get; private init; }
        public string Email { get; private init; }
        public string PhoneNumber { get; private init; }
        public string FullAddress { get; private init; }

#pragma warning disable CS8618
        private EmployeeSnapshot() { }
#pragma warning restore CS8618

        private EmployeeSnapshot(Employee employee)
        {
            EmployeeId = employee.Id;
            FullName = employee.Name.FullName;
            Email = employee.Email.Value;
            PhoneNumber = employee.PhoneNumber.Value;
            FullAddress = employee.Address.FullAddress;
        }

        public static EmployeeSnapshot FromEmployee(Employee employee) => new(employee);
    }
}
