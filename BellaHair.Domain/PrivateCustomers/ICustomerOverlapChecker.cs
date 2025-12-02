// Mikkel Dahlmann

namespace BellaHair.Domain.PrivateCustomers
{

    /// <summary>
    /// Exposes a method to check for overlapping customer records based on phone number or email address.
    /// </summary>

    public interface ICustomerOverlapChecker
    {
        Task OverlapsWithCustomer(string phoneNumber, string email, Guid? customerId = null);
    }
}
