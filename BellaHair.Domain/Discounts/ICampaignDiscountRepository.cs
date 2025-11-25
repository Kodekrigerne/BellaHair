namespace BellaHair.Domain.Discounts
{
    // Mikkel Klitgaard
    /// <summary>
    /// Defines methods for managing campaign discount entities in a data store.
    /// </summary>
    /// <remarks>Implementations of this interface provide asynchronous and synchronous operations for adding,
    /// deleting, retrieving, and persisting changes to campaign discounts. Methods may interact with a database or
    /// other persistent storage. Thread safety and transaction management depend on the specific
    /// implementation.</remarks>
    public interface ICampaignDiscountRepository
    {
        Task AddAsync(CampaignDiscount campaignDiscount);
        void Delete(CampaignDiscount campaignDiscount);
        Task<CampaignDiscount> GetAsync(Guid id);
        Task SaveChangesAsync();

    }
}
