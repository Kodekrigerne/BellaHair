using BellaHair.Domain.Discounts;
using BellaHair.Infrastructure;

namespace BellaHair.Presentation.WebUI
{
    //Dennis
    /// <summary>
    /// Provides a method for adding hardcoded example data to BellaHairContext.
    /// </summary>
    public class DataProvider
    {
        private readonly BellaHairContext _db;

        public DataProvider(BellaHairContext db) => _db = db;

        public void AddData()
        {
            AddLoyaltyDiscounts();

            _db.SaveChangesAsync();
        }

        private void AddLoyaltyDiscounts()
        {
            _db.Add(LoyaltyDiscount.Create("Loyalty5", 5, DiscountPercent.FromDecimal(0.05m)));
            _db.Add(LoyaltyDiscount.Create("Loyalty10", 10, DiscountPercent.FromDecimal(0.10m)));
            _db.Add(LoyaltyDiscount.Create("Loyalty15", 15, DiscountPercent.FromDecimal(0.15m)));
        }
    }
}
