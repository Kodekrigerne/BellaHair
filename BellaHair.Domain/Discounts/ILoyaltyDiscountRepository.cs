namespace BellaHair.Domain.Discounts
{
    //Dennis
    /// <summary>
    /// Exposes operations for handling LoyaltyDiscounts
    /// </summary>
    public interface ILoyaltyDiscountRepository
    {
        Task AddAsync(LoyaltyDiscount loyaltyDiscount);
        void Delete(LoyaltyDiscount loyaltyDiscount);
        Task<LoyaltyDiscount> GetAsync(Guid id);
        Task SaveChangesAsync();
    }
}
