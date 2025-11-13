using BellaHair.Domain.Discounts;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Domain.Treatments;
using BellaHair.Domain.Treatments.ValueObjects;
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
            AddTreatment();

            _db.SaveChangesAsync();
        }

        private void AddLoyaltyDiscounts()
        {
            _db.Add(LoyaltyDiscount.Create("Loyalty5", 5, DiscountPercent.FromDecimal(0.05m)));
            _db.Add(LoyaltyDiscount.Create("Loyalty10", 10, DiscountPercent.FromDecimal(0.10m)));
            _db.Add(LoyaltyDiscount.Create("Loyalty15", 15, DiscountPercent.FromDecimal(0.15m)));
        }

        private void AddTreatment()
        {
            _db.Add(Treatment.Create("Herreklip", Price.FromDecimal(450m), DurationMinutes.FromInt(30)));
            _db.Add(Treatment.Create("Dameklip", Price.FromDecimal(600m), DurationMinutes.FromInt(60)));
            _db.Add(Treatment.Create("Dame Hårfarvning", Price.FromDecimal(400m), DurationMinutes.FromInt(90)));
        }
    }
}
