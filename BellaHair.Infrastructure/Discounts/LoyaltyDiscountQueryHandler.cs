using BellaHair.Domain.Discounts;
using BellaHair.Ports.Discounts;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.Discounts
{
    //Dennis
    /// <inheritdoc cref="ILoyaltyDiscountQuery"/>
    public class LoyaltyDiscountQueryHandler : ILoyaltyDiscountQuery
    {
        private readonly BellaHairContext _db;

        public LoyaltyDiscountQueryHandler(BellaHairContext db) => _db = db;

        async Task<List<LoyaltyDiscountDTO>> ILoyaltyDiscountQuery.GetLoyaltyDiscounts()
        {
            return await _db.Discounts
                .AsNoTracking()
                .OfType<LoyaltyDiscount>()
                .Select(x => new LoyaltyDiscountDTO(x.Id, x.Name, x.MinimumVisits, x.DiscountPercent.Value))
                .ToListAsync();
        }
    }
}
