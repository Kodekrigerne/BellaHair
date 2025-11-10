namespace BellaHair.Domain.Discounts
{
    //Dennis
    public interface ILoyaltyDiscountRepository
    {
        Task AddAsync(LoyaltyDiscount loyaltyDiscount);
        void Delete(LoyaltyDiscount loyaltyDiscount);
        Task<LoyaltyDiscount> Get(Guid id);
        Task<List<LoyaltyDiscount>> GetAll();
        Task SaveChangesAsync();
    }
}
