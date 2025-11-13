using BellaHair.Domain.Discounts;
using BellaHair.Domain.Employees;
using BellaHair.Domain.SharedValueObjects;
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
            _db.Add(LoyaltyDiscount.Create("Loyalty5", 5, DiscountPercent.FromDecimal(0.05m)));
            _db.Add(LoyaltyDiscount.Create("Loyalty10", 10, DiscountPercent.FromDecimal(0.10m)));
            _db.Add(LoyaltyDiscount.Create("Loyalty15", 15, DiscountPercent.FromDecimal(0.15m)));
        }

        private void AddEmployees()
        {
            _db.Add(Employee.Create(Name.FromStrings("Henny", "Hansen")));
        }
    }
}
