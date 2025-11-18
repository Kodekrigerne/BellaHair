using BellaHair.Domain;
using BellaHair.Domain.Discounts;
using BellaHair.Domain.Employees;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Domain.Treatments;
using BellaHair.Domain.Treatments.ValueObjects;
using BellaHair.Infrastructure;
using Microsoft.EntityFrameworkCore;

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

        public void ReinstateData()
        {
            Console.WriteLine("test");
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();
            _db.Database.ExecuteSqlRaw("PRAGMA journal_mode=DELETE;");
            AddData();
        }

        public void AddData()
        {
            AddLoyaltyDiscounts();
            AddTreatment();
            AddEmployees();

            _db.SaveChangesAsync();
        }

        private void AddLoyaltyDiscounts()
        {
            _db.Add(LoyaltyDiscount.Create("Stamkunde Bronze", 5, DiscountPercent.FromDecimal(0.05m)));
            _db.Add(LoyaltyDiscount.Create("Stamkunde Sølv", 10, DiscountPercent.FromDecimal(0.10m)));
            _db.Add(LoyaltyDiscount.Create("Stamkunde Guld", 15, DiscountPercent.FromDecimal(0.15m)));
        }

        private void AddEmployees()
        {
            _db.Add(Employee.Create(Name.FromStrings("Henny", "Hansen"), Email.FromString("hennyh@frisor.dk"), PhoneNumber.FromString("42501113"), Address.Create("Nørrebro", "København H", "47", 2000)));
            _db.Add(Employee.Create(Name.FromStrings("Peter", "Pedersen"), Email.FromString("peterp@frisor.dk"), PhoneNumber.FromString("20456789"), Address.Create("Vestergade", "Aarhus C", "10", 8000)));
            _db.Add(Employee.Create(Name.FromStrings("Maria", "Jensen"), Email.FromString("mariaj@frisor.dk"), PhoneNumber.FromString("55123456"), Address.Create("Østerbrogade", "København Ø", "15B", 2100)));
            _db.Add(Employee.Create(Name.FromStrings("Søren", "Mikkelsen"), Email.FromString("sorenm@frisor.dk"), PhoneNumber.FromString("77889900"), Address.Create("Industrivej", "Odense M", "5", 5260)));
            _db.Add(Employee.Create(Name.FromStrings("Søren", "Jensen"), Email.FromString("sorenj@frisor.dk"), PhoneNumber.FromString("23132322"), Address.Create("Industrigade", "Vejle", "12", 7100)));
        }

        private void AddTreatment()
        {
            _db.Add(Treatment.Create("Herreklip", Price.FromDecimal(450m), DurationMinutes.FromInt(30)));
            _db.Add(Treatment.Create("Dameklip", Price.FromDecimal(600m), DurationMinutes.FromInt(60)));
            _db.Add(Treatment.Create("Dame Hårfarvning", Price.FromDecimal(400m), DurationMinutes.FromInt(90)));
        }
    }
}
