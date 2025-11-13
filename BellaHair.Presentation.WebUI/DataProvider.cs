using BellaHair.Domain.Discounts;
using BellaHair.Domain.Employees;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Infrastructure;
using BellaHair.Domain;

namespace BellaHair.Presentation.WebUI
{
    public class DataProvider
    {
        private readonly BellaHairContext _db;

        public DataProvider(BellaHairContext db) => _db = db;

        public void AddData()
        {
            AddLoyaltyDiscounts();
            AddEmployees();

            _db.SaveChangesAsync();
        }

        private void AddLoyaltyDiscounts()
        {
            _db.Add(LoyaltyDiscount.Create("Loyalty5", 5, DiscountPercent.FromDecimal(0.05m)));
            _db.Add(LoyaltyDiscount.Create("Loyalty10", 10, DiscountPercent.FromDecimal(0.10m)));
            _db.Add(LoyaltyDiscount.Create("Loyalty15", 15, DiscountPercent.FromDecimal(0.15m)));
        }

        private void AddEmployees()
        {
            _db.Add(Employee.Create(Name.FromStrings("Henny", "Hansen"), Email.FromString("hennyh@frisor.dk"), PhoneNumber.FromString("42501113"), Address.Create("Nørrebro", "København H", "47", 2000)));
            _db.Add(Employee.Create(Name.FromStrings("Peter", "Pedersen"), Email.FromString("peterp@snedker.dk"), PhoneNumber.FromString("20456789"), Address.Create("Vestergade", "Aarhus C", "10", 8000)));
            _db.Add(Employee.Create(Name.FromStrings("Maria", "Jensen"), Email.FromString("mariaj@bageri.dk"), PhoneNumber.FromString("55123456"), Address.Create("Østerbrogade", "København Ø", "15B", 2100)));
            _db.Add(Employee.Create(Name.FromStrings("Søren", "Mikkelsen"), Email.FromString("sorenm@mekaniker.dk"), PhoneNumber.FromString("77889900"), Address.Create("Industrivej", "Odense M", "5", 5260)));
        }
    }
}
