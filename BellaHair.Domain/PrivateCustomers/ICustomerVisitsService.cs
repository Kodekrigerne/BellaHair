namespace BellaHair.Infrastructure.PrivateCustomers
{
    public interface ICustomerVisitsService
    {
        Task<int> GetCustomerVisitsAsync(Guid customerId);
    }
}
