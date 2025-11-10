using BellaHair.Domain.Discounts;

namespace BellaHair.Infrastructure.Discounts
{
    //Dennis
    /// <summary><inheritdoc cref="ILoyaltyDiscountRepository"/> on the DbContext.</summary>
    public class LoyaltyDiscountRepository : ILoyaltyDiscountRepository
    {
        private readonly BellaHairContext _db;

        public LoyaltyDiscountRepository(BellaHairContext db) => _db = db;

        async Task ILoyaltyDiscountRepository.AddAsync(LoyaltyDiscount loyaltyDiscount)
        {
            await _db.Discounts.AddAsync(loyaltyDiscount);
        }

        void ILoyaltyDiscountRepository.Delete(LoyaltyDiscount loyaltyDiscount)
        {
            _db.Discounts.Remove(loyaltyDiscount);
        }

        async Task<LoyaltyDiscount> ILoyaltyDiscountRepository.Get(Guid id)
        {
            var discount = await _db.Discounts.FindAsync(id)
                ?? throw new KeyNotFoundException($"No loyalty discount exists with ID {id}");

            if (discount is LoyaltyDiscount loyaltyDiscount) return loyaltyDiscount;
            throw new InvalidOperationException($"Discount with ID {id} is not a loyalty discount");
        }

        async Task ILoyaltyDiscountRepository.SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
