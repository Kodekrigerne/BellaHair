using BellaHair.Domain;
using BellaHair.Domain.Discounts;
using BellaHair.Domain.Employees;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Domain.Treatments;
using BellaHair.Domain.Treatments.ValueObjects;
using BellaHair.Infrastructure;
using BellaHair.Domain.PrivateCustomers;
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
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();
            _db.Database.ExecuteSqlRaw("PRAGMA journal_mode=DELETE;");
            AddData();
        }

        // --- 1. Treatment Fields ---
        private Treatment _herreklip;
        private Treatment _dameklip;
        private Treatment _farvning;

        // --- 2. Employee Fields ---
        private Employee _henny;
        private Employee _peter;
        private Employee _maria;
        private Employee _sorenM;
        private Employee _sorenJ;

        public async Task AddData()
        {
            AddLoyaltyDiscounts();
            AddTreatment();
            AddEmployees();
            AddPrivateCustomers();

            await _db.SaveChangesAsync();

            AddEmployees();
            await _db.SaveChangesAsync();
        }

        private void AddLoyaltyDiscounts()
        {
            _db.Add(LoyaltyDiscount.Create("Stamkunde Bronze", 5, DiscountPercent.FromDecimal(0.05m)));
            _db.Add(LoyaltyDiscount.Create("Stamkunde Sølv", 10, DiscountPercent.FromDecimal(0.10m)));
            _db.Add(LoyaltyDiscount.Create("Stamkunde Guld", 15, DiscountPercent.FromDecimal(0.15m)));
        }

        private void AddEmployees()
        {
            // --- Employees and Treatments ---

            // Henny Hansen: Herreklip, Dameklip
            _henny = Employee.Create(
                Name.FromStrings("Henny", "Hansen"),
                Email.FromString("hennyh@frisor.dk"),
                PhoneNumber.FromString("42501113"),
                Address.Create("Nørrebro", "København H", "47", 2000),
                new List<Treatment> { _herreklip, _dameklip }
            );
            _db.Employees.Add(_henny);

            // Peter Pedersen: Herreklip, Dame Hårfarvning
            _peter = Employee.Create(
                Name.FromStrings("Peter", "Pedersen"),
                Email.FromString("peterp@frisor.dk"),
                PhoneNumber.FromString("20456789"),
                Address.Create("Vestergade", "Aarhus C", "10", 8000),
                new List<Treatment> { _herreklip, _farvning }
            );
            _db.Add(_peter);

            // Søren Mikkelsen: Herreklip only
            _sorenM = Employee.Create(
                Name.FromStrings("Søren", "Mikkelsen"),
                Email.FromString("sorenm@frisor.dk"),
                PhoneNumber.FromString("77889900"),
                Address.Create("Industrivej", "Odense M", "5", 5260),
                new List<Treatment> { _herreklip }
            );
            _db.Add(_sorenM);

            // Søren Jensen: Dameklip and Dame Hårfarvning
            _sorenJ = Employee.Create(
                Name.FromStrings("Søren", "Jensen"),
                Email.FromString("sorenj@frisor.dk"),
                PhoneNumber.FromString("23132322"),
                Address.Create("Industrigade", "Vejle", "12", 7100),
                new List<Treatment> { _dameklip, _farvning }
            );
            _db.Add(_sorenJ);

            // Maria Jensen: All three treatments
            _maria = Employee.Create(
                Name.FromStrings("Maria", "Jensen"),
                Email.FromString("mariaj@frisor.dk"),
                PhoneNumber.FromString("55123456"),
                Address.Create("Østerbrogade", "København Ø", "15B", 2100),
                new List<Treatment> { _herreklip, _dameklip, _farvning }
            );
            _db.Add(_maria);
        }

        private void AddTreatment()
        {
            _herreklip = Treatment.Create("Herreklip", Price.FromDecimal(450m), DurationMinutes.FromInt(30));
            _dameklip = Treatment.Create("Dameklip", Price.FromDecimal(600m), DurationMinutes.FromInt(60));
            _farvning = Treatment.Create("Dame Hårfarvning", Price.FromDecimal(400m), DurationMinutes.FromInt(90));

            _db.Add(_herreklip);
            _db.Add(_dameklip);
            _db.Add(_farvning);
        }

        private void AddPrivateCustomers()
        {
            _db.Add(PrivateCustomer.Create(Name.FromStrings("Peter", "Svendsen", "Emil"), Address.Create("Søndergade", "Vejle", "15A", 7100, 3), PhoneNumber.FromString("12345678"), Email.FromString("peteres@gmail.com"), DateTime.Now.AddYears(-42)));
            _db.Add(PrivateCustomer.Create(Name.FromStrings("Lis", "Mortensen", "Karin"), Address.Create("Vestergade", "Vejle", "2", 7100), PhoneNumber.FromString("87654321"), Email.FromString("lis@gmail.com"), DateTime.Now.AddYears(-68)));
            _db.Add(PrivateCustomer.Create(Name.FromStrings("Lars", "Christiansen"), Address.Create("Østergade", "Vejle", "342", 7100, 9), PhoneNumber.FromString("43215678"), Email.FromString("Lars@hotmail.com"), DateTime.Now.AddYears(-38)));
            _db.Add(PrivateCustomer.Create(Name.FromStrings("Oskar", "Issaksen", "Theodor"), Address.Create("Nygade", "Vejle", "6", 7100), PhoneNumber.FromString("56784321"), Email.FromString("oskartheshit@hotmail.com"), DateTime.Now.AddYears(-20)));
        }
    }
}
