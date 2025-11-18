using BellaHair.Domain.PrivateCustomers;

namespace BellaHair.Domain.Bookings
{
    //Dennis
    /// <summary>
    /// Represents a Customer as a Snapshot in time for invoicing and historic data reasons<br/>
    /// Contains only the relevant information for these purposes, and so omits relational data
    /// </summary>
    public record CustomerSnapshot
    {
        public Guid CustomerId { get; private init; }
        public string FullName { get; private init; }
        public string Email { get; private init; }
        public string PhoneNumber { get; private init; }
        public string FullAddress { get; private init; }
        public DateTime Birthday { get; private init; }

#pragma warning disable CS8618
        private CustomerSnapshot() { }
#pragma warning restore CS8618

        private CustomerSnapshot(PrivateCustomer customer)
        {
            CustomerId = customer.Id;
            FullName = customer.Name.FullName;
            Email = customer.Email.Value;
            PhoneNumber = customer.PhoneNumber.Value;
            FullAddress = customer.Address.FullAddress;
            Birthday = customer.Birthday;
        }

        public static CustomerSnapshot FromCustomer(PrivateCustomer customer) => new(customer);
    }
}
