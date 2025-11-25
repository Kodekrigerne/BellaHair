namespace BellaHair.Infrastructure.PrivateCustomers
{
    //Dennis
    /// <summary>
    /// Exposes a method to get a customers amount of paid past visits to the salon
    /// </summary>
    public interface ICustomerVisitsService
    {
        Task<int> GetCustomerVisitsAsync(Guid customerId);
    }
}
