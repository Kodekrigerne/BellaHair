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
            AddCampaignDiscounts();

            _db.SaveChanges();
        }

        private void AddCampaignDiscounts()
        {
            _db.Add(CampaignDiscount.Create("Sommerklip",
                DiscountPercent.FromDecimal(0.20m),
                new DateTime(2026, 6, 1, 12, 0, 0),
                new DateTime(2026, 8, 1, 12, 0, 0),
                new List<Guid> { _herreklip.Id, _dameklip.Id }));

            _db.Add(CampaignDiscount.Create("Februar Farve Flash",
                DiscountPercent.FromDecimal(0.10m),
                new DateTime(2026, 2, 1, 12, 0, 0),
                new DateTime(2026, 3, 1, 0, 0, 0),
                new List<Guid> { _farvning.Id }));

            _db.Add(CampaignDiscount.Create("Back-to-School Børneklip",
                DiscountPercent.FromDecimal(0.10m),
                new DateTime(2026, 8, 1, 12, 0, 0),
                new DateTime(2026, 9, 1, 0, 0, 0),
                new List<Guid> { _børneklip.Id }));

            _db.Add(CampaignDiscount.Create("Juletilbud",
                DiscountPercent.FromDecimal(0.12m),
                new DateTime(2025, 12, 1),
                new DateTime(2025, 12, 24),
                new List<Guid>
                    { _herreklip.Id, _dameklip.Id, _barbering.Id, _farvning.Id, _børneklip.Id, _permanent.Id }));

            _db.Add(CampaignDiscount.Create("Movember shave",
                DiscountPercent.FromDecimal(0.30m),
                new DateTime(2025, 11, 1),
                new DateTime(2025, 12, 1),
                new List<Guid>
                    { _barbering.Id }));
        }

        private void AddLoyaltyDiscounts()
        {
            _db.Add(LoyaltyDiscount.Create("Stamkunde Nikkel", 1, DiscountPercent.FromDecimal(0.01m)));
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
            var kennet = Employee.Create(
                Name.FromStrings("Kennet", "Hansen"),
                Email.FromString("kennet@frisor.dk"),
                PhoneNumber.FromString("38238289"),
                Address.Create("Vesterbro", "Byby", "11A", 1100),
                new List<Treatment> { _dameklip }
            );
            _db.Add(kennet);
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

            var b12 = Booking.Create(_larsc, _peter, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 8, 30, 0).AddDays(1), _mockPastDateTimeProvider);
            _db.Add(b12);

            var b13 = Booking.Create(_lismk, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 8, 30, 0).AddDays(1), _mockPastDateTimeProvider);
            _db.Add(b13);

            // 09:00 - Morning Rush
            var b14 = Booking.Create(_simonehs, _henny, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 9, 0, 0).AddDays(1), _mockPastDateTimeProvider);
            _db.Add(b14);

            var b15 = Booking.Create(_peterse, _maria, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 9, 0, 0).AddDays(1), _mockPastDateTimeProvider);
            _db.Add(b15);

            var b16 = Booking.Create(_oskarit, _sorenJ, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 9, 30, 0).AddDays(1), _mockPastDateTimeProvider);
            _db.Add(b16);

            // 10:00
            var b17 = Booking.Create(_lismk, _peter, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 10, 0, 0).AddDays(1), _mockPastDateTimeProvider);
            _db.Add(b17);

            var b18 = Booking.Create(_larsc, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 10, 0, 0).AddDays(1), _mockPastDateTimeProvider);
            _db.Add(b18);

            // 11:00
            var b19 = Booking.Create(_simonehs, _henny, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 11, 0, 0).AddDays(1), _mockPastDateTimeProvider);
            _db.Add(b19);

            var b20 = Booking.Create(_oskarit, _maria, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 11, 30, 0).AddDays(1), _mockPastDateTimeProvider);
            _db.Add(b20);

            // 12:00 - Lunch shift
            var b21 = Booking.Create(_peterse, _sorenJ, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 12, 0, 0).AddDays(1), _mockPastDateTimeProvider);
            _db.Add(b21);

            var b22 = Booking.Create(_larsc, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 12, 30, 0).AddDays(1), _mockPastDateTimeProvider);
            _db.Add(b22);

            // 13:00 - Afternoon
            var b23 = Booking.Create(_lismk, _peter, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 13, 0, 0).AddDays(1), _mockPastDateTimeProvider);
            _db.Add(b23);

            var b24 = Booking.Create(_simonehs, _maria, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 13, 30, 0).AddDays(1), _mockPastDateTimeProvider);
            _db.Add(b24);

            var b25 = Booking.Create(_oskarit, _henny, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 14, 0, 0).AddDays(1), _mockPastDateTimeProvider);
            _db.Add(b25);

            // 15:00
            var b26 = Booking.Create(_peterse, _sorenJ, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 15, 0, 0).AddDays(1), _mockPastDateTimeProvider);
            _db.Add(b26);

            var b27 = Booking.Create(_larsc, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 15, 30, 0).AddDays(1), _mockPastDateTimeProvider);
            _db.Add(b27);


            // --- DAY +2 (Heavy Load) ---

            var b28 = Booking.Create(_lismk, _henny, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 8, 30, 0).AddDays(2), _mockPastDateTimeProvider);
            _db.Add(b28);

            var b29 = Booking.Create(_peterse, _peter, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 9, 0, 0).AddDays(2), _mockPastDateTimeProvider);
            _db.Add(b29);

            var b30 = Booking.Create(_simonehs, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 9, 30, 0).AddDays(2), _mockPastDateTimeProvider);
            _db.Add(b30);

            var b31 = Booking.Create(_oskarit, _maria, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 10, 0, 0).AddDays(2), _mockPastDateTimeProvider);
            _db.Add(b31);

            var b32 = Booking.Create(_larsc, _sorenJ, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 10, 30, 0).AddDays(2), _mockPastDateTimeProvider);
            _db.Add(b32);

            var b33 = Booking.Create(_lismk, _henny, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 11, 0, 0).AddDays(2), _mockPastDateTimeProvider);
            _db.Add(b33);

            var b34 = Booking.Create(_peterse, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 11, 30, 0).AddDays(2), _mockPastDateTimeProvider);
            _db.Add(b34);

            var b35 = Booking.Create(_simonehs, _peter, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 12, 30, 0).AddDays(2), _mockPastDateTimeProvider);
            _db.Add(b35);

            var b36 = Booking.Create(_oskarit, _maria, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 13, 0, 0).AddDays(2), _mockPastDateTimeProvider);
            _db.Add(b36);

            var b37 = Booking.Create(_larsc, _sorenJ, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 13, 30, 0).AddDays(2), _mockPastDateTimeProvider);
            _db.Add(b37);

            var b38 = Booking.Create(_lismk, _henny, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 14, 0, 0).AddDays(2), _mockPastDateTimeProvider);
            _db.Add(b38);

            var b39 = Booking.Create(_peterse, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 14, 30, 0).AddDays(2), _mockPastDateTimeProvider);
            _db.Add(b39);

            var b40 = Booking.Create(_simonehs, _peter, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 15, 0, 0).AddDays(2), _mockPastDateTimeProvider);
            _db.Add(b40);


            // --- DAY +3 ---

            var b41 = Booking.Create(_oskarit, _sorenJ, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 9, 0, 0).AddDays(3), _mockPastDateTimeProvider);
            _db.Add(b41);

            var b42 = Booking.Create(_larsc, _maria, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 9, 30, 0).AddDays(3), _mockPastDateTimeProvider);
            _db.Add(b42);

            var b43 = Booking.Create(_lismk, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 10, 0, 0).AddDays(3), _mockPastDateTimeProvider);
            _db.Add(b43);

            var b44 = Booking.Create(_peterse, _henny, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 10, 30, 0).AddDays(3), _mockPastDateTimeProvider);
            _db.Add(b44);

            var b45 = Booking.Create(_simonehs, _peter, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 11, 0, 0).AddDays(3), _mockPastDateTimeProvider);
            _db.Add(b45);

            var b46 = Booking.Create(_oskarit, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 12, 30, 0).AddDays(3), _mockPastDateTimeProvider);
            _db.Add(b46);

            var b47 = Booking.Create(_larsc, _maria, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 13, 0, 0).AddDays(3), _mockPastDateTimeProvider);
            _db.Add(b47);

            var b48 = Booking.Create(_lismk, _sorenJ, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 14, 0, 0).AddDays(3), _mockPastDateTimeProvider);
            _db.Add(b48);

            var b49 = Booking.Create(_peterse, _henny, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 15, 0, 0).AddDays(3), _mockPastDateTimeProvider);
            _db.Add(b49);


            // --- DAY +4 ---

            var b50 = Booking.Create(_simonehs, _peter, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 8, 30, 0).AddDays(4), _mockPastDateTimeProvider);
            _db.Add(b50);

            var b51 = Booking.Create(_oskarit, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 9, 0, 0).AddDays(4), _mockPastDateTimeProvider);
            _db.Add(b51);

            var b52 = Booking.Create(_larsc, _maria, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 10, 0, 0).AddDays(4), _mockPastDateTimeProvider);
            _db.Add(b52);

            var b53 = Booking.Create(_lismk, _sorenJ, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 11, 0, 0).AddDays(4), _mockPastDateTimeProvider);
            _db.Add(b53);

            var b54 = Booking.Create(_peterse, _henny, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 13, 0, 0).AddDays(4), _mockPastDateTimeProvider);
            _db.Add(b54);

            var b55 = Booking.Create(_simonehs, _peter, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 13, 30, 0).AddDays(4), _mockPastDateTimeProvider);
            _db.Add(b55);

            var b56 = Booking.Create(_oskarit, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 14, 0, 0).AddDays(4), _mockPastDateTimeProvider);
            _db.Add(b56);

            var b57 = Booking.Create(_larsc, _maria, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 15, 0, 0).AddDays(4), _mockPastDateTimeProvider);
            _db.Add(b57);


            // --- DAY +5 ---

            var b58 = Booking.Create(_lismk, _sorenJ, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 9, 0, 0).AddDays(5), _mockPastDateTimeProvider);
            _db.Add(b58);

            var b59 = Booking.Create(_peterse, _henny, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 9, 30, 0).AddDays(5), _mockPastDateTimeProvider);
            _db.Add(b59);

            var b60 = Booking.Create(_simonehs, _peter, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 10, 0, 0).AddDays(5), _mockPastDateTimeProvider);
            _db.Add(b60);

            var b61 = Booking.Create(_oskarit, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 11, 0, 0).AddDays(5), _mockPastDateTimeProvider);
            _db.Add(b61);

            var b62 = Booking.Create(_larsc, _maria, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 12, 0, 0).AddDays(5), _mockPastDateTimeProvider);
            _db.Add(b62);

            var b63 = Booking.Create(_lismk, _sorenJ, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 13, 0, 0).AddDays(5), _mockPastDateTimeProvider);
            _db.Add(b63);

            var b64 = Booking.Create(_peterse, _henny, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 14, 0, 0).AddDays(5), _mockPastDateTimeProvider);
            _db.Add(b64);


            // --- DAY +6 to +10 (Scattered) ---

            var b65 = Booking.Create(_simonehs, _peter, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 9, 0, 0).AddDays(6), _mockPastDateTimeProvider);
            _db.Add(b65);

            var b66 = Booking.Create(_oskarit, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 10, 30, 0).AddDays(6), _mockPastDateTimeProvider);
            _db.Add(b66);

            var b67 = Booking.Create(_larsc, _maria, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 14, 0, 0).AddDays(6), _mockPastDateTimeProvider);
            _db.Add(b67);

            var b68 = Booking.Create(_lismk, _sorenJ, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 9, 30, 0).AddDays(7), _mockPastDateTimeProvider);
            _db.Add(b68);

            var b69 = Booking.Create(_peterse, _henny, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 11, 0, 0).AddDays(7), _mockPastDateTimeProvider);
            _db.Add(b69);

            var b70 = Booking.Create(_simonehs, _peter, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 15, 0, 0).AddDays(7), _mockPastDateTimeProvider);
            _db.Add(b70);

            var b71 = Booking.Create(_oskarit, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 8, 30, 0).AddDays(8), _mockPastDateTimeProvider);
            _db.Add(b71);

            var b72 = Booking.Create(_larsc, _maria, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 10, 0, 0).AddDays(8), _mockPastDateTimeProvider);
            _db.Add(b72);

            var b73 = Booking.Create(_lismk, _sorenJ, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 13, 0, 0).AddDays(8), _mockPastDateTimeProvider);
            _db.Add(b73);

            var b74 = Booking.Create(_peterse, _henny, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 15, 30, 0).AddDays(8), _mockPastDateTimeProvider);
            _db.Add(b74);

            var b75 = Booking.Create(_simonehs, _peter, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 9, 0, 0).AddDays(9), _mockPastDateTimeProvider);
            _db.Add(b75);

            var b76 = Booking.Create(_oskarit, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 11, 0, 0).AddDays(9), _mockPastDateTimeProvider);
            _db.Add(b76);

            var b77 = Booking.Create(_larsc, _maria, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 14, 0, 0).AddDays(9), _mockPastDateTimeProvider);
            _db.Add(b77);

            var b78 = Booking.Create(_lismk, _sorenJ, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 10, 0, 0).AddDays(10), _mockPastDateTimeProvider);
            _db.Add(b78); // Assuming SorenJ can do herreklip, if not, change to Dame

            var b79 = Booking.Create(_peterse, _henny, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 13, 0, 0).AddDays(10), _mockPastDateTimeProvider);
            _db.Add(b79);


            // --- DAYS +11 to +14 ---

            var b80 = Booking.Create(_simonehs, _peter, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 9, 30, 0).AddDays(11), _mockPastDateTimeProvider);
            _db.Add(b80);

            var b81 = Booking.Create(_oskarit, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 11, 0, 0).AddDays(11), _mockPastDateTimeProvider);
            _db.Add(b81);

            var b82 = Booking.Create(_larsc, _maria, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 14, 0, 0).AddDays(12), _mockPastDateTimeProvider);
            _db.Add(b82);

            var b83 = Booking.Create(_lismk, _sorenJ, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 10, 30, 0).AddDays(13), _mockPastDateTimeProvider);
            _db.Add(b83);

            var b84 = Booking.Create(_peterse, _henny, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 15, 0, 0).AddDays(13), _mockPastDateTimeProvider);
            _db.Add(b84);

            var b85 = Booking.Create(_simonehs, _peter, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 9, 0, 0).AddDays(14), _mockPastDateTimeProvider);
            _db.Add(b85);
        }

        // Bruges da Bookings skal have en ICurrentDateTimeProvider som giver deres CreatedDate som skal være i fortiden i forhold til StartTime.
        internal class PastDateTimeProvider : ICurrentDateTimeProvider
        {
            DateTime ICurrentDateTimeProvider.GetCurrentDateTime() => DateTime.Now.AddDays(-60);
        }
    }

}
