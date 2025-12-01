namespace BellaHair.Domain.PrivateCustomers
{
    public interface ICustomerOverlapChecker
    {
        Task OverlapsWithCustomer(string phoneNumber, string email);
    }
}
