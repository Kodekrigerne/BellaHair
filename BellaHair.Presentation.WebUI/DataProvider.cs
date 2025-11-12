using BellaHair.Domain.Discounts;
using BellaHair.Infrastructure;

namespace BellaHair.Presentation.WebUI
{
    public class DataProvider
    {
        private readonly BellaHairContext _db;

        public DataProvider(BellaHairContext db) => _db = db;

        public void AddData()
        {
            AddLoyaltyDiscounts();
        }

        private void AddLoyaltyDiscounts()
        {
            _db.Add(LoyaltyDiscount.Create("Loyalty5", 5, DiscountPercent.FromDecimal(5)));
            _db.Add(LoyaltyDiscount.Create("Loyalty5", 10, DiscountPercent.FromDecimal(10)));
            _db.Add(LoyaltyDiscount.Create("Loyalty5", 15, DiscountPercent.FromDecimal(15)));
        }
    }
}
