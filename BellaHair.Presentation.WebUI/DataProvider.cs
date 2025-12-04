using BellaHair.Domain;
using BellaHair.Domain.Bookings;
using BellaHair.Domain.Discounts;
using BellaHair.Domain.Employees;
using BellaHair.Domain.PrivateCustomers;
using BellaHair.Domain.Products;
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

        // --- 4. Product Fields ---
        private Product _shampoo;
        private Product _balsam;
        private Product _hårvoks;
        private Product _hårspray;
        private Product _hårkur;
        private Product _leaveinConditioner;
        private Product _ansigtsrens;
        private Product _dagcreme;
        private Product _natcreme;
        private Product _serum;
        private Product _øjenceme;
        private Product _bodylotion;
        private Product _håndcreme;
        private Product _bodyWash;
        private Product _eksfoliering;
        private Product _solcreme;
        private Product _aftershave;
        private Product _skægolie;
        private Product _stylingmousse;
        private Product _tørshampoo;
        private Product _læbepomade;
        private Product _fodcreme;
        private Product _negleolie;
        private Product _hårfarve;
        private Product _makeupfjerner;
        private Product _barberskum;

        public void AddData()
        {
            AddProducts();
            AddLoyaltyDiscounts();
            AddTreatment();
            AddPrivateCustomers();
            AddEmployees();
            AddBookings();
            AddCampaignDiscounts();
            AddBirthdayDiscounts();

            _db.SaveChanges();
        }

        private void AddBirthdayDiscounts()
        {
            _db.Add(BirthdayDiscount.Create("Fødselsdagsrabat", DiscountPercent.FromDecimal(0.50m)));
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
            _db.Add(LoyaltyDiscount.CreateWithProductDiscount("Stamkunde Sølv", 10, DiscountPercent.FromDecimal(0.10m), DiscountPercent.FromDecimal(0.5m)));
            _db.Add(LoyaltyDiscount.CreateWithProductDiscount("Stamkunde Guld", 15, DiscountPercent.FromDecimal(0.15m), DiscountPercent.FromDecimal(0.10m)));
        }

        private void AddProducts()
        {
            _shampoo = Product.Create("Shampoo", "Mild shampoo for alle hårtyper", Price.FromDecimal(120m));
            _db.Add(_shampoo);

            _balsam = Product.Create("Balsam", "Nærende balsam for glat og skinnende hår", Price.FromDecimal(150m));
            _db.Add(_balsam);

            _hårvoks = Product.Create("Hårvoks", "Fleksibel hårvoks for tekstur og hold", Price.FromDecimal(200m));
            _db.Add(_hårvoks);

            _hårspray = Product.Create("Hårspray", "Langtidsholdbar hårspray med let finish", Price.FromDecimal(180m));
            _db.Add(_hårspray);

            _hårkur = Product.Create("Hårkur", "Intensiv hårkur til genopbygning af skadet hår", Price.FromDecimal(250m));
            _db.Add(_hårkur);

            _leaveinConditioner = Product.Create("Leave-in Conditioner", "Let leave-in conditioner for nem styling", Price.FromDecimal(130m));
            _db.Add(_leaveinConditioner);

            _ansigtsrens = Product.Create("Ansigtsrens", "Skånsom rens til daglig brug, fjerner urenheder og makeup", Price.FromDecimal(160m));
            _db.Add(_ansigtsrens);

            _dagcreme = Product.Create("Dagcreme", "Fugtgivende dagcreme med SPF 30, beskytter mod solen", Price.FromDecimal(280m));
            _db.Add(_dagcreme);

            _natcreme = Product.Create("Natcreme", "Rig natcreme, genopbygger huden mens du sover", Price.FromDecimal(300m));
            _db.Add(_natcreme);

            _serum = Product.Create("Serum", "Anti-age serum med hyaluronsyre for dyb hydrering", Price.FromDecimal(350m));
            _db.Add(_serum);

            _øjenceme = Product.Create("Øjencreme", "Let øjencreme reducerer poser og mørke rande", Price.FromDecimal(220m));
            _db.Add(_øjenceme);

            _bodylotion = Product.Create("Bodylotion", "Blødgørende bodylotion med naturlige olier", Price.FromDecimal(110m));
            _db.Add(_bodylotion);

            _håndcreme = Product.Create("Håndcreme", "Reparerende håndcreme, ideel til tørre hænder", Price.FromDecimal(85m));
            _db.Add(_håndcreme);

            _bodyWash = Product.Create("Body Wash", "Opfriskende kropsvask med citrusduft", Price.FromDecimal(95m));
            _db.Add(_bodyWash);

            _eksfoliering = Product.Create("Eksfoliering", "Kropsskrub med fine korn, fjerner døde hudceller", Price.FromDecimal(145m));
            _db.Add(_eksfoliering);

            _solcreme = Product.Create("Solcreme", "Vandfast solcreme SPF 50 til hele kroppen", Price.FromDecimal(190m));
            _db.Add(_solcreme);

            _aftershave = Product.Create("Aftershave", "Beroligende aftershave balm uden alkohol", Price.FromDecimal(175m));
            _db.Add(_aftershave);

            _skægolie = Product.Create("Skægolie", "Blødgørende skægolie med cedertræ og patchouli", Price.FromDecimal(195m));
            _db.Add(_skægolie);

            _stylingmousse = Product.Create("Stylingmousse", "Volumengivende mousse for let og luftig frisure", Price.FromDecimal(140m));
            _db.Add(_stylingmousse);

            _tørshampoo = Product.Create("Tørshampoo", "Øjeblikkelig tørshampoo, giver volumen og friskhed", Price.FromDecimal(125m));
            _db.Add(_tørshampoo);

            _læbepomade = Product.Create("Læbepomade", "Fugtgivende læbepomade med bivoks og mint", Price.FromDecimal(55m));
            _db.Add(_læbepomade);

            _fodcreme = Product.Create("Fodcreme", "Intensiv fodcreme mod tør og sprukken hud", Price.FromDecimal(105m));
            _db.Add(_fodcreme);

            _negleolie = Product.Create("Negleolie", "Plejende olie til negle og neglebånd", Price.FromDecimal(75m));
            _db.Add(_negleolie);

            _hårfarve = Product.Create("Hårfarve", "Permanent hårfarve i medium brun", Price.FromDecimal(199m));
            _db.Add(_hårfarve);

            _makeupfjerner = Product.Create("Makeupfjerner", "Effektiv makeupfjerner, også til vandfast makeup", Price.FromDecimal(115m));
            _db.Add(_makeupfjerner);

            _barberskum = Product.Create("Barberskum", "Klassisk barberskum for tæt og glat barbering", Price.FromDecimal(80m));
            _db.Add(_barberskum);
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
            var quantity1 = Quantity.FromInt(1);
            var quantity2 = Quantity.FromInt(2);
            var quantity3 = Quantity.FromInt(3);
            var productLine1 = new ProductLineData(quantity1, _fodcreme);
            var productLine2 = new ProductLineData(quantity2, _hårspray);
            var productLine3 = new ProductLineData(quantity3, _bodyWash);

            var now = _currentDateTimeProvider.GetCurrentDateTime();

            // HENNY HANSEN (_henny)

            // Kunde: Peter Svendsen
            var b1 = Booking.Create(_peterse, _henny, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 10, 0, 0).AddDays(-30), _mockPastDateTimeProvider, []);
            b1.PayBooking(_currentDateTimeProvider);
            _db.Add(b1);

            var b2 = Booking.Create(_peterse, _maria, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 14, 30, 0).AddDays(-15), _mockPastDateTimeProvider, []);
            b2.PayBooking(_currentDateTimeProvider);
            _db.Add(b2);

            var b3 = Booking.Create(_peterse, _peter, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 9, 0, 0).AddDays(-5), _mockPastDateTimeProvider, []);
            b3.PayBooking(_currentDateTimeProvider);
            _db.Add(b3);

            // Lis Mortensen bookings
            var b4 = Booking.Create(_lismk, _sorenJ, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 11, 0, 0).AddDays(-25), _mockPastDateTimeProvider, []);
            b4.PayBooking(_currentDateTimeProvider);
            _db.Add(b4);

            var b5 = Booking.Create(_lismk, _maria, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 13, 0, 0).AddDays(-20), _mockPastDateTimeProvider, []);
            b5.PayBooking(_currentDateTimeProvider);
            _db.Add(b5);

            var b64 = Booking.Create(_peterse, _henny, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 14, 0, 0).AddDays(5), _mockPastDateTimeProvider, [productLine1, productLine2, productLine3]);
            _db.Add(b64);

            var b69 = Booking.Create(_peterse, _henny, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 11, 0, 0).AddDays(7), _mockPastDateTimeProvider, []);
            _db.Add(b69);

            var b74 = Booking.Create(_peterse, _henny, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 15, 30, 0).AddDays(8), _mockPastDateTimeProvider, []);
            _db.Add(b74);

            var b79 = Booking.Create(_peterse, _henny, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 13, 0, 0).AddDays(10), _mockPastDateTimeProvider, []);
            _db.Add(b79);

            var b84 = Booking.Create(_peterse, _henny, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 15, 0, 0).AddDays(13), _mockPastDateTimeProvider, []);
            _db.Add(b84);

            // Kunde: Lis Mortensen
            var b6 = Booking.Create(_lismk, _henny, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 10, 30, 0).AddDays(-8), _mockPastDateTimeProvider, []);
            b6.PayBooking(_currentDateTimeProvider);
            _db.Add(b6);

            // Lars Christiansen bookings
            var b7 = Booking.Create(_larsc, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 15, 0, 0).AddDays(-28), _mockPastDateTimeProvider, []);
            b7.PayBooking(_currentDateTimeProvider);
            _db.Add(b7);

            var b25 = Booking.Create(_oskarit, _henny, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 14, 0, 0).AddDays(1), _mockPastDateTimeProvider, []);
            _db.Add(b25);

            // Kunde: Simone Sørensen
            var b14 = Booking.Create(_simonehs, _henny, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 9, 0, 0).AddDays(1), _mockPastDateTimeProvider, []);
            _db.Add(b14);

            var b19 = Booking.Create(_simonehs, _henny, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 11, 0, 0).AddDays(1), _mockPastDateTimeProvider, []);
            _db.Add(b19);


            // PETER PEDERSEN (_peter)

            // Kunde: Peter Svendsen
            var b8 = Booking.Create(_peterse, _peter, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 9, 0, 0).AddDays(-5), _mockPastDateTimeProvider, []);
            b8.PayBooking(_currentDateTimeProvider);
            _db.Add(b8);

            var b29 = Booking.Create(_peterse, _peter, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 9, 0, 0).AddDays(2), _mockPastDateTimeProvider, []);
            _db.Add(b29);

            // Kunde: Lars Christiansen
            var b9 = Booking.Create(_larsc, _peter, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 16, 0, 0).AddDays(-12), _mockPastDateTimeProvider, []);
            b9.PayBooking(_currentDateTimeProvider);
            _db.Add(b9);

            var b12 = Booking.Create(_larsc, _peter, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 8, 30, 0).AddDays(1), _mockPastDateTimeProvider, []);
            _db.Add(b12);

            // Kunde: Lis Mortensen
            var b17 = Booking.Create(_lismk, _peter, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 10, 0, 0).AddDays(1), _mockPastDateTimeProvider, []);
            _db.Add(b17);

            var b23 = Booking.Create(_lismk, _peter, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 13, 0, 0).AddDays(1), _mockPastDateTimeProvider, []);
            _db.Add(b23);

            // Kunde: Simone Sørensen 
            var b35 = Booking.Create(_simonehs, _peter, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 12, 30, 0).AddDays(2), _mockPastDateTimeProvider, []);
            _db.Add(b35);

            var b40 = Booking.Create(_simonehs, _peter, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 15, 0, 0).AddDays(2), _mockPastDateTimeProvider, []);
            _db.Add(b40);

            var b45 = Booking.Create(_simonehs, _peter, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 11, 0, 0).AddDays(3), _mockPastDateTimeProvider, []);
            _db.Add(b45);

            var b50 = Booking.Create(_simonehs, _peter, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 8, 30, 0).AddDays(4), _mockPastDateTimeProvider, []);
            _db.Add(b50);

            var b55 = Booking.Create(_simonehs, _peter, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 13, 30, 0).AddDays(4), _mockPastDateTimeProvider, []);
            _db.Add(b55);

            var b60 = Booking.Create(_simonehs, _peter, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 10, 0, 0).AddDays(5), _mockPastDateTimeProvider, []);
            _db.Add(b60);

            var b65 = Booking.Create(_simonehs, _peter, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 9, 0, 0).AddDays(6), _mockPastDateTimeProvider, []);
            _db.Add(b65);

            var b70 = Booking.Create(_simonehs, _peter, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 15, 0, 0).AddDays(7), _mockPastDateTimeProvider, []);
            _db.Add(b70);

            var b75 = Booking.Create(_simonehs, _peter, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 9, 0, 0).AddDays(9), _mockPastDateTimeProvider, []);
            _db.Add(b75);

            var b80 = Booking.Create(_simonehs, _peter, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 9, 30, 0).AddDays(11), _mockPastDateTimeProvider, []);
            _db.Add(b80);

            var b85 = Booking.Create(_simonehs, _peter, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 9, 0, 0).AddDays(14), _mockPastDateTimeProvider, []);
            _db.Add(b85);


            // MARIA JENSEN (_maria)

            // Kunde: Peter Svendsen
            var b10 = Booking.Create(_peterse, _maria, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 14, 30, 0).AddDays(-15), _mockPastDateTimeProvider, []);
            b10.PayBooking(_currentDateTimeProvider);
            _db.Add(b10);

            var b15 = Booking.Create(_peterse, _maria, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 9, 0, 0).AddDays(1), _mockPastDateTimeProvider, []);
            _db.Add(b15);

            // Kunde: Lis Mortensen
            var b11 = Booking.Create(_lismk, _maria, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 13, 0, 0).AddDays(-20), _mockPastDateTimeProvider, []);
            b11.PayBooking(_currentDateTimeProvider);
            _db.Add(b11);

            // Kunde: Lars Christiansen
            var b13 = Booking.Create(_larsc, _maria, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 11, 30, 0).AddDays(-3), _mockPastDateTimeProvider, []);
            b13.PayBooking(_currentDateTimeProvider);
            _db.Add(b13);

            // Oskar Issaksen bookings
            var b16 = Booking.Create(_oskarit, _henny, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 13, 30, 0).AddDays(-22), _mockPastDateTimeProvider, []);
            b16.PayBooking(_currentDateTimeProvider);
            _db.Add(b16);

            var b82 = Booking.Create(_larsc, _maria, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 14, 0, 0).AddDays(12), _mockPastDateTimeProvider, []);
            _db.Add(b82);

            // Kunde: Oskar Issaksen
            var b20 = Booking.Create(_oskarit, _maria, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 11, 30, 0).AddDays(1), _mockPastDateTimeProvider, []);
            _db.Add(b20);

            var b31 = Booking.Create(_oskarit, _maria, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 10, 0, 0).AddDays(2), _mockPastDateTimeProvider, []);
            _db.Add(b31);

            var b36 = Booking.Create(_oskarit, _maria, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 13, 0, 0).AddDays(2), _mockPastDateTimeProvider, []);
            _db.Add(b36);

            // Kunde: Simone Sørensen
            var b24 = Booking.Create(_simonehs, _maria, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 13, 30, 0).AddDays(1), _mockPastDateTimeProvider, []);
            _db.Add(b24);


            // SØREN MIKKELSEN (_sorenM)

            // Kunde: Lars Christiansen
            var b18 = Booking.Create(_larsc, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 15, 0, 0).AddDays(-28), _mockPastDateTimeProvider, []);
            b18.PayBooking(_currentDateTimeProvider);
            _db.Add(b18);

            var b21 = Booking.Create(_larsc, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 10, 0, 0).AddDays(1), _mockPastDateTimeProvider, []);
            _db.Add(b21);

            var b22 = Booking.Create(_larsc, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 12, 30, 0).AddDays(1), _mockPastDateTimeProvider, []);
            _db.Add(b22);

            var b27 = Booking.Create(_larsc, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 15, 30, 0).AddDays(1), _mockPastDateTimeProvider, []);
            _db.Add(b27);

            // Kunde: Oskar Issaksen
            var b26 = Booking.Create(_oskarit, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 12, 0, 0).AddDays(-10), _mockPastDateTimeProvider, []);
            b26.PayBooking(_currentDateTimeProvider);
            _db.Add(b26);

            // Future bookings (upcoming appointments)

            // Peter Svendsen bookings
            _db.Add(Booking.Create(_peterse, _henny, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 10, 0, 0).AddDays(3), _mockPastDateTimeProvider, []));
            _db.Add(Booking.Create(_peterse, _maria, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 14, 0, 0).AddDays(10), _mockPastDateTimeProvider, []));

            // Lis Mortensen bookings
            _db.Add(Booking.Create(_lismk, _sorenJ, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 9, 30, 0).AddDays(5), _mockPastDateTimeProvider, []));
            _db.Add(Booking.Create(_lismk, _maria, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 11, 0, 0).AddDays(14), _mockPastDateTimeProvider, []));
            _db.Add(Booking.Create(_lismk, _henny, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 15, 30, 0).AddDays(21), _mockPastDateTimeProvider, []));

            // Lars Christiansen bookings
            _db.Add(Booking.Create(_larsc, _peter, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 13, 0, 0).AddDays(7), _mockPastDateTimeProvider, []));
            _db.Add(Booking.Create(_larsc, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 16, 30, 0).AddDays(18), _mockPastDateTimeProvider, []));

            // Oskar Issaksen bookings
            _db.Add(Booking.Create(_oskarit, _maria, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 12, 30, 0).AddDays(2), _mockPastDateTimeProvider, []));
            _db.Add(Booking.Create(_oskarit, _henny, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 10, 30, 0).AddDays(12), _mockPastDateTimeProvider, []));
            _db.Add(Booking.Create(_oskarit, _peter, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 14, 30, 0).AddDays(25), _mockPastDateTimeProvider, []));

            // Additional bookings for variety - mixing treatments and employees

            // More Henny bookings (past)
            _db.Add(Booking.Create(_peterse, _henny, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 11, 0, 0).AddDays(-18), _mockPastDateTimeProvider, []));

            // More Peter bookings (future)
            _db.Add(Booking.Create(_lismk, _peter, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 10, 0, 0).AddDays(8), _mockPastDateTimeProvider, []));

            // More Maria bookings (past and future)
            _db.Add(Booking.Create(_larsc, _maria, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 9, 0, 0).AddDays(-7), _mockPastDateTimeProvider, []));
            _db.Add(Booking.Create(_oskarit, _maria, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 15, 0, 0).AddDays(15), _mockPastDateTimeProvider, []));

            // More Søren M bookings
            _db.Add(Booking.Create(_peterse, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 13, 30, 0).AddDays(-14), _mockPastDateTimeProvider, []));
            _db.Add(Booking.Create(_larsc, _sorenM, _herreklip,
                new DateTime(now.Year, now.Month, now.Day, 11, 0, 0).AddDays(20), _mockPastDateTimeProvider, []));

            // More Søren J bookings
            _db.Add(Booking.Create(_lismk, _sorenJ, _dameklip,
                new DateTime(now.Year, now.Month, now.Day, 14, 0, 0).AddDays(-6), _mockPastDateTimeProvider, []));
            _db.Add(Booking.Create(_oskarit, _sorenJ, _farvning,
                new DateTime(now.Year, now.Month, now.Day, 16, 0, 0).AddDays(28), _mockPastDateTimeProvider, []));
        }

        // Bruges da Bookings skal have en ICurrentDateTimeProvider som giver deres CreatedDate som skal være i fortiden i forhold til StartTime.
        internal class PastDateTimeProvider : ICurrentDateTimeProvider
        {
            DateTime ICurrentDateTimeProvider.GetCurrentDateTime() => DateTime.Now.AddDays(-60);
        }
    }

}
