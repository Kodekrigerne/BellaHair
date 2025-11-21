using BellaHair.Domain;
using BellaHair.Domain.Bookings;
using BellaHair.Domain.Discounts;
using BellaHair.Domain.Employees;
using BellaHair.Domain.PrivateCustomers;
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
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider = new CurrentDateTimeProvider();
        private readonly ICurrentDateTimeProvider _mockPastDateTimeProvider = new PastDateTimeProvider();

#pragma warning disable CS8618
        public DataProvider(BellaHairContext db) => _db = db;
#pragma warning restore CS8618

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
        private Treatment _barbering;
        private Treatment _børneklip;
        private Treatment _permanent;


        // --- 2. Employee Fields ---
        private Employee _henny;
        private Employee _peter;
        private Employee _maria;
        private Employee _sorenM;
        private Employee _sorenJ;

        // --- 3. Private Customer Fields ---
        private PrivateCustomer _peterse;
        private PrivateCustomer _lismk;
        private PrivateCustomer _larsc;
        private PrivateCustomer _oskarit;
        private PrivateCustomer _simonehs;

        public void AddData()
        {
            AddLoyaltyDiscounts();
            AddTreatment();
            AddPrivateCustomers();
            AddEmployees();
            AddBookings();

            _db.SaveChanges();
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

            // Dennis: No bookings
            var dennis = Employee.Create(
                Name.FromStrings("Dennis", "Hansen"),
                Email.FromString("dennis@frisor.dk"),
                PhoneNumber.FromString("38238289"),
                Address.Create("Vesterbro", "Byby", "11A", 1100),
                new List<Treatment> { _dameklip}
            );
            _db.Add(dennis);
        }

        private void AddTreatment()
        {
            _herreklip = Treatment.Create("Herreklip", Price.FromDecimal(450m), DurationMinutes.FromInt(30));
            _dameklip = Treatment.Create("Dameklip", Price.FromDecimal(600m), DurationMinutes.FromInt(60));
            _farvning = Treatment.Create("Dame Hårfarvning", Price.FromDecimal(400m), DurationMinutes.FromInt(90));
            _barbering = Treatment.Create("Barbering", Price.FromDecimal(150m), DurationMinutes.FromInt(20));
            _børneklip = Treatment.Create("Børneklip", Price.FromDecimal(250m), DurationMinutes.FromInt(30));
            _permanent = Treatment.Create("Permanent", Price.FromDecimal(770m), DurationMinutes.FromInt(120));

            _db.Add(_herreklip);
            _db.Add(_dameklip);
            _db.Add(_farvning);
            _db.Add(_barbering);
            _db.Add(_børneklip);
            _db.Add(_permanent);
        }

        private void AddPrivateCustomers()
        {
            _peterse = PrivateCustomer.Create(Name.FromStrings("Peter", "Svendsen", "Emil"), Address.Create("Søndergade", "Vejle", "15A", 7100, 3), PhoneNumber.FromString("12345678"), Email.FromString("peteres@gmail.com"), _currentDateTimeProvider.GetCurrentDateTime().AddYears(-42), _currentDateTimeProvider);
            _lismk = PrivateCustomer.Create(Name.FromStrings("Lis", "Mortensen", "Karin"), Address.Create("Vestergade", "Vejle", "2", 7100), PhoneNumber.FromString("87654321"), Email.FromString("lis@gmail.com"), _currentDateTimeProvider.GetCurrentDateTime().AddYears(-68), _currentDateTimeProvider);
            _larsc = PrivateCustomer.Create(Name.FromStrings("Lars", "Christiansen"), Address.Create("Østergade", "Vejle", "342", 7100, 9), PhoneNumber.FromString("43215678"), Email.FromString("Lars@hotmail.com"), _currentDateTimeProvider.GetCurrentDateTime().AddYears(-38), _currentDateTimeProvider);
            _oskarit = PrivateCustomer.Create(Name.FromStrings("Oskar", "Issaksen", "Theodor"), Address.Create("Nygade", "Vejle", "6", 7100), PhoneNumber.FromString("56784321"), Email.FromString("oskartheshit@hotmail.com"), _currentDateTimeProvider.GetCurrentDateTime().AddYears(-20), _currentDateTimeProvider);
            _simonehs = PrivateCustomer.Create(Name.FromStrings("Simone", "Sørensen", "Henriette"), Address.Create("Allégade", "Vejle", "55", 7100), PhoneNumber.FromString("67891234"), Email.FromString("henry@live.com"), _currentDateTimeProvider.GetCurrentDateTime().AddYears(-32), _currentDateTimeProvider);

            _db.Add(_peterse);
            _db.Add(_lismk);
            _db.Add(_larsc);
            _db.Add(_oskarit);
            _db.Add(_simonehs);
        }

        private void AddBookings()
        {
            var now = _currentDateTimeProvider.GetCurrentDateTime();

            // Past bookings (completed appointments)

            // Peter Svendsen bookings
            var b1 = Booking.Create(_peterse, _henny, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 10, 0, 0).AddDays(-30), _mockPastDateTimeProvider);
            b1.PayBooking(_currentDateTimeProvider);
            _db.Add(b1);

            var b2 = Booking.Create(_peterse, _maria, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 14, 30, 0).AddDays(-15), _mockPastDateTimeProvider);
            b2.PayBooking(_currentDateTimeProvider);
            _db.Add(b2);

            var b3 = Booking.Create(_peterse, _peter, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 9, 0, 0).AddDays(-5), _mockPastDateTimeProvider);
            b3.PayBooking(_currentDateTimeProvider);
            _db.Add(b3);

            // Lis Mortensen bookings
            var b4 = Booking.Create(_lismk, _sorenJ, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 11, 0, 0).AddDays(-25), _mockPastDateTimeProvider);
            b4.PayBooking(_currentDateTimeProvider);
            _db.Add(b4);

            var b5 = Booking.Create(_lismk, _maria, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 13, 0, 0).AddDays(-20), _mockPastDateTimeProvider);
            b5.PayBooking(_currentDateTimeProvider);
            _db.Add(b5);

            var b6 = Booking.Create(_lismk, _henny, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 10, 30, 0).AddDays(-8), _mockPastDateTimeProvider);
            b6.PayBooking(_currentDateTimeProvider);
            _db.Add(b6);

            // Lars Christiansen bookings
            var b7 = Booking.Create(_larsc, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 15, 0, 0).AddDays(-28), _mockPastDateTimeProvider);
            b7.PayBooking(_currentDateTimeProvider);
            _db.Add(b7);

            var b8 = Booking.Create(_larsc, _peter, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 16, 0, 0).AddDays(-12), _mockPastDateTimeProvider);
            b8.PayBooking(_currentDateTimeProvider);
            _db.Add(b8);

            var b9 = Booking.Create(_larsc, _maria, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 11, 30, 0).AddDays(-3), _mockPastDateTimeProvider);
            b9.PayBooking(_currentDateTimeProvider);
            _db.Add(b9);

            // Oskar Issaksen bookings
            var b10 = Booking.Create(_oskarit, _henny, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 13, 30, 0).AddDays(-22), _mockPastDateTimeProvider);
            b10.PayBooking(_currentDateTimeProvider);
            _db.Add(b10);

            var b11 = Booking.Create(_oskarit, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 12, 0, 0).AddDays(-10), _mockPastDateTimeProvider);
            b11.PayBooking(_currentDateTimeProvider);
            _db.Add(b11);

            // Future bookings (upcoming appointments)

            // Peter Svendsen bookings
            _db.Add(Booking.Create(_peterse, _henny, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 10, 0, 0).AddDays(3), _mockPastDateTimeProvider));
            _db.Add(Booking.Create(_peterse, _maria, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 14, 0, 0).AddDays(10), _mockPastDateTimeProvider));

            // Lis Mortensen bookings
            _db.Add(Booking.Create(_lismk, _sorenJ, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 9, 30, 0).AddDays(5), _mockPastDateTimeProvider));
            _db.Add(Booking.Create(_lismk, _maria, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 11, 0, 0).AddDays(14), _mockPastDateTimeProvider));
            _db.Add(Booking.Create(_lismk, _henny, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 15, 30, 0).AddDays(21), _mockPastDateTimeProvider));

            // Lars Christiansen bookings
            _db.Add(Booking.Create(_larsc, _peter, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 13, 0, 0).AddDays(7), _mockPastDateTimeProvider));
            _db.Add(Booking.Create(_larsc, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 16, 30, 0).AddDays(18), _mockPastDateTimeProvider));

            // Oskar Issaksen bookings
            _db.Add(Booking.Create(_oskarit, _maria, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 12, 30, 0).AddDays(2), _mockPastDateTimeProvider));
            _db.Add(Booking.Create(_oskarit, _henny, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 10, 30, 0).AddDays(12), _mockPastDateTimeProvider));
            _db.Add(Booking.Create(_oskarit, _peter, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 14, 30, 0).AddDays(25), _mockPastDateTimeProvider));

            // Additional bookings for variety - mixing treatments and employees

            // More Henny bookings (past)
            _db.Add(Booking.Create(_peterse, _henny, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 11, 0, 0).AddDays(-18), _mockPastDateTimeProvider));

            // More Peter bookings (future)
            _db.Add(Booking.Create(_lismk, _peter, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 10, 0, 0).AddDays(8), _mockPastDateTimeProvider));

            // More Maria bookings (past and future)
            _db.Add(Booking.Create(_larsc, _maria, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 9, 0, 0).AddDays(-7), _mockPastDateTimeProvider));
            _db.Add(Booking.Create(_oskarit, _maria, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 15, 0, 0).AddDays(15), _mockPastDateTimeProvider));

            // More Søren M bookings
            _db.Add(Booking.Create(_peterse, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 13, 30, 0).AddDays(-14), _mockPastDateTimeProvider));
            _db.Add(Booking.Create(_larsc, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 11, 0, 0).AddDays(20), _mockPastDateTimeProvider));

            // More Søren J bookings
            _db.Add(Booking.Create(_lismk, _sorenJ, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 14, 0, 0).AddDays(-6), _mockPastDateTimeProvider));
            _db.Add(Booking.Create(_oskarit, _sorenJ, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 16, 0, 0).AddDays(28), _mockPastDateTimeProvider));
        }

        // Bruges da Bookings skal have en ICurrentDateTimeProvider som giver deres CreatedDate som skal være i fortiden i forhold til StartTime.
        internal class PastDateTimeProvider : ICurrentDateTimeProvider
        {
            DateTime ICurrentDateTimeProvider.GetCurrentDateTime() => DateTime.Now.AddDays(-60);
        }
    }

}
