using BellaHair.Domain;
using BellaHair.Domain.Discounts;
using BellaHair.Domain.Employees;
using BellaHair.Domain.PrivateCustomers;
using BellaHair.Domain.Products;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Domain.Treatments;
using BellaHair.Domain.Treatments.ValueObjects;
using BellaHair.Infrastructure;
using BellaHair.Ports.Bookings;
using Bogus;
using FixtureBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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
        private readonly IServiceProvider _serviceProvider;

#pragma warning disable CS8618
        public DataProvider(BellaHairContext db, IServiceProvider serviceProvider)
        {
            _db = db;
            _serviceProvider = serviceProvider;
        }
#pragma warning restore CS8618

        public async Task ReinstateData()
        {
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();
            _db.Database.ExecuteSqlRaw("PRAGMA journal_mode=DELETE;");
            await AddData();
        }

        private readonly IOptions<OpeningTimesSettings> OpeningTimes;

        // Treatment Fields 
        private Treatment _herreklipUdenVaskFøn;
        private Treatment _herreklipMedVaskFøn;
        private Treatment _damefrisureInklVaskFøn;
        private Treatment _damefrisureBlæs;
        private Treatment _lilleTilretning;
        private Treatment _storKlipning;
        private Treatment _luksusKur;
        private Treatment _retFarveBryn;
        private Treatment _ordneBrynVipper;
        private Treatment _herreKlipPermanent;
        private Treatment _helfarveHalvkortHårUdenKlip;
        private Treatment _helfarveHalvKortHårMedKlip;
        private Treatment _helfarveLangtHårUdenKlip;
        private Treatment _helfarveLangtHårMedKlip;
        private Treatment _hætteStriberIHalvkortHårMedKlip;
        private Treatment _hætteStriberILangtHårMedKlip;
        private Treatment _permanentHalvkortHårMedKlip;
        private Treatment _permanentHalvkortHårUdenKlip;
        private Treatment _permanentLangtHårUdenKlip;
        private Treatment _permanentLangtHårMedKlip;
        private Treatment _staniolStriberIHalvkortHårUdenKlip;
        private Treatment _staniolStriberIHalvkortHårMedKlip;
        private Treatment _staniolStriberILangtHårUdenKlip;
        private Treatment _staniolStriberILangtHårMedKlip;
        private Treatment _balayageUdenKlip;
        private Treatment _balayageMedKlip;
        private Treatment _hårOpsætningStruktur;
        private Treatment _hårOpsætningElegance;
        private Treatment _hårOpsætningKompleks;

        List<Treatment> _treatments;

        // Employee fields
        private Employee _idaChristensen;
        private Employee _jonasHansen;
        private Employee _sofieNielsen;
        private Employee _larsMikkelsen;
        private Employee _signeJørgensen;
        private Employee _madsKnudsen;
        private Employee _annaPetersen;

        List<Employee> _employees = [];
        List<PrivateCustomer> _customers = [];

        // Product fields
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

        List<Product> _products = [];

        public async Task AddData()
        {
            AddProducts();
            AddPrivateCustomersUsingBogus();
            AddLoyaltyDiscounts();
            AddTreatment();
            AddEmployees();
            AddCampaignDiscounts();
            AddBirthdayDiscounts();
            AddBookingsUsingBogusAndHandler();
            AddPastBookingsUsingBogusAndHandler();
            await PayAndInvoicePastBookings();

            _db.SaveChanges();
        }

        private async Task PayAndInvoicePastBookings()
        {
            var queryHandler = _serviceProvider.GetRequiredService<IBookingQuery>();
            var commandHandler = _serviceProvider.GetRequiredService<IBookingCommand>();
            var pastBookings = await queryHandler.GetAllOldAsync();

            foreach (var booking in pastBookings)
            {
                if (booking.Discount != null)
                {
                    var discountData = new DiscountData(booking.Discount.Name, booking.Discount.Amount, booking.Discount.Type);
                    var command = new PayAndInvoiceBookingCommand(booking.Id, discountData);
                    await commandHandler.PayAndInvoiceBooking(command);
                }
                else
                {
                    var command = new PayAndInvoiceBookingCommand(booking.Id, null);
                    await commandHandler.PayAndInvoiceBooking(command);
                }
            }
        }

        private void AddBookingsUsingBogusAndHandler()
        {

            var now = _currentDateTimeProvider.GetCurrentDateTime();

            for (int i = 0; i < 50; i++)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();

                    Random random = new Random();
                    var employee = _employees[random.Next(0, 6)];
                    var treatment = employee.Treatments[random.Next(1, employee.Treatments.Count)];
                    List<CreateProductLine> productLines = [];
                    for (int p = 0; p < random.Next(0, 2); p++)
                    {
                        var productToList = _products[random.Next(0, _products.Count - 1)];
                        var productId = productToList.Id;
                        var quantity = random.Next(1, 2);
                        productLines.Add(new CreateProductLine(quantity, productId));
                    }
                    var product = _products[random.Next(0, _products.Count)];
                    var customer = _customers[random.Next(0, _customers.Count)];

                    var bookingFaker = new Faker<CreateBookingCommand>("nb_NO")
                        .CustomInstantiator(f =>
                        {
                            var bookingDate = f.Date.Between(now.AddDays(1), now.AddDays(30));
                            var bookingHour = f.Random.Int(10, 17 - treatment.DurationMinutes.Value / 60);
                            var bookingMinutes = f.Random.Int(0, 60 - treatment.DurationMinutes.Value);
                            bookingMinutes -= (bookingMinutes % 15);

                            return new CreateBookingCommand(new DateTime(bookingDate.Year, bookingDate.Month, bookingDate.Day, bookingHour, bookingMinutes, 0), employee.Id, customer.Id, treatment.Id, productLines);
                        });

                    var booking = bookingFaker.Generate();
                    if (booking.StartDateTime.DayOfWeek == DayOfWeek.Saturday || booking.StartDateTime.DayOfWeek == DayOfWeek.Sunday) throw new Exception();

                    var bookingCommandHandler = scope.ServiceProvider.GetRequiredService<IBookingCommand>();
                    bookingCommandHandler.CreateBooking(booking).Wait();

                    scope.Dispose();

                }
                catch (Exception)
                {

                }
            }

        }


        private void AddPastBookingsUsingBogusAndHandler()
        {

            var now = _mockPastDateTimeProvider.GetCurrentDateTime();

            for (int i = 0; i < 50; i++)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();

                    Random random = new Random();
                    var employee = _employees[random.Next(0, 6)];
                    var treatment = employee.Treatments[random.Next(1, employee.Treatments.Count)];
                    List<CreateProductLine> productLines = [];
                    for (int p = 0; p < random.Next(0, 2); p++)
                    {
                        var productToList = _products[random.Next(0, _products.Count - 1)];
                        var productId = productToList.Id;
                        var quantity = random.Next(1, 2);
                        productLines.Add(new CreateProductLine(quantity, productId));
                    }
                    var product = _products[random.Next(0, _products.Count)];
                    var customer = _customers[random.Next(0, _customers.Count)];

                    var bookingFaker = new Faker<CreateBookingCommand>("nb_NO")
                        .CustomInstantiator(f =>
                        {
                            var bookingDate = f.Date.Between(now.AddDays(1), now.AddDays(30));
                            var bookingHour = f.Random.Int(10, 17 - treatment.DurationMinutes.Value / 60);
                            var bookingMinutes = f.Random.Int(0, 60 - treatment.DurationMinutes.Value);
                            bookingMinutes -= (bookingMinutes % 15);

                            return new CreateBookingCommand(new DateTime(bookingDate.Year, bookingDate.Month, bookingDate.Day, bookingHour, bookingMinutes, 0), employee.Id, customer.Id, treatment.Id, productLines);
                        });

                    var booking = bookingFaker.Generate();
                    if (booking.StartDateTime.DayOfWeek == DayOfWeek.Saturday || booking.StartDateTime.DayOfWeek == DayOfWeek.Sunday) throw new Exception();

                    var bookingCommandHandler = scope.ServiceProvider.GetRequiredService<IBookingCommand>();
                    Fixture.New(bookingCommandHandler).WithField("_currentDateTimeProvider", _mockPastDateTimeProvider).Build();

                    bookingCommandHandler.CreateBooking(booking).Wait();

                    scope.Dispose();

                }
                catch (Exception)
                {

                }
            }

        }

        private void AddPrivateCustomersUsingBogus()
        {
            _customers = [];

            var now = _currentDateTimeProvider.GetCurrentDateTime();

            for (int i = 0; i < 30; i++)
            {
                try
                {
                    var customerFaker = new Faker<PrivateCustomer>("nb_NO")
                        .CustomInstantiator(f =>
                        {
                            var name = Name.FromStrings(f.Name.FirstName(), f.Name.LastName());

                            return PrivateCustomer.Create(name,
                                                    Address.Create(f.Address.StreetName().Replace(".", ""), f.Address.City(), f.Random.Int(min: 1, max: 200).ToString(), f.Random.Int(min: 1000, max: 9990)),
                                                    PhoneNumber.FromString(f.Phone.PhoneNumber("########")),
                                                    Email.FromString(f.Internet.Email(name.FirstName, name.LastName)),
                                                    f.Date.Between(now.AddYears(-18), now.AddYears(-80)),
                                                    _mockPastDateTimeProvider
                                                    );
                        });

                    var customer = customerFaker.Generate();


                    _customers.Add(customer);
                    _db.Add(customer);
                }
                catch (Exception) { }
            }
        }

        private void AddProducts()
        {
            _shampoo = Product.Create("Shampoo", "Mild shampoo for alle hårtyper", Price.FromDecimal(120m));
            _db.Add(_shampoo);
            _products.Add(_shampoo);

            _balsam = Product.Create("Balsam", "Nærende balsam for glat og skinnende hår", Price.FromDecimal(150m));
            _db.Add(_balsam);
            _products.Add(_balsam);

            _hårvoks = Product.Create("Hårvoks", "Fleksibel hårvoks for tekstur og hold", Price.FromDecimal(200m));
            _db.Add(_hårvoks);
            _products.Add(_hårvoks);

            _hårspray = Product.Create("Hårspray", "Langtidsholdbar hårspray med let finish", Price.FromDecimal(180m));
            _db.Add(_hårspray);
            _products.Add(_hårspray);

            _hårkur = Product.Create("Hårkur", "Intensiv hårkur til genopbygning af skadet hår", Price.FromDecimal(250m));
            _db.Add(_hårkur);
            _products.Add(_hårkur);

            _leaveinConditioner = Product.Create("Leave-in Conditioner", "Let leave-in conditioner for nem styling", Price.FromDecimal(130m));
            _db.Add(_leaveinConditioner);
            _products.Add(_leaveinConditioner);

            _ansigtsrens = Product.Create("Ansigtsrens", "Skånsom rens til daglig brug, fjerner urenheder og makeup", Price.FromDecimal(160m));
            _db.Add(_ansigtsrens);
            _products.Add(_ansigtsrens);

            _dagcreme = Product.Create("Dagcreme", "Fugtgivende dagcreme med SPF 30, beskytter mod solen", Price.FromDecimal(280m));
            _db.Add(_dagcreme);
            _products.Add(_dagcreme);

            _natcreme = Product.Create("Natcreme", "Rig natcreme, genopbygger huden mens du sover", Price.FromDecimal(300m));
            _db.Add(_natcreme);
            _products.Add(_natcreme);

            _serum = Product.Create("Serum", "Anti-age serum med hyaluronsyre for dyb hydrering", Price.FromDecimal(350m));
            _db.Add(_serum);
            _products.Add(_serum);

            _øjenceme = Product.Create("Øjencreme", "Let øjencreme reducerer poser og mørke rande", Price.FromDecimal(220m));
            _db.Add(_øjenceme);
            _products.Add(_øjenceme);

            _bodylotion = Product.Create("Bodylotion", "Blødgørende bodylotion med naturlige olier", Price.FromDecimal(110m));
            _db.Add(_bodylotion);
            _products.Add(_bodylotion);

            _håndcreme = Product.Create("Håndcreme", "Reparerende håndcreme, ideel til tørre hænder", Price.FromDecimal(85m));
            _db.Add(_håndcreme);
            _products.Add(_håndcreme);

            _bodyWash = Product.Create("Body Wash", "Opfriskende kropsvask med citrusduft", Price.FromDecimal(95m));
            _db.Add(_bodyWash);
            _products.Add(_bodyWash);

            _eksfoliering = Product.Create("Eksfoliering", "Kropsskrub med fine korn, fjerner døde hudceller", Price.FromDecimal(145m));
            _db.Add(_eksfoliering);
            _products.Add(_eksfoliering);

            _solcreme = Product.Create("Solcreme", "Vandfast solcreme SPF 50 til hele kroppen", Price.FromDecimal(190m));
            _db.Add(_solcreme);
            _products.Add(_solcreme);

            _aftershave = Product.Create("Aftershave", "Beroligende aftershave balm uden alkohol", Price.FromDecimal(175m));
            _db.Add(_aftershave);
            _products.Add(_aftershave);

            _skægolie = Product.Create("Skægolie", "Blødgørende skægolie med cedertræ og patchouli", Price.FromDecimal(195m));
            _db.Add(_skægolie);
            _products.Add(_skægolie);

            _stylingmousse = Product.Create("Stylingmousse", "Volumengivende mousse for let og luftig frisure", Price.FromDecimal(140m));
            _db.Add(_stylingmousse);
            _products.Add(_stylingmousse);

            _tørshampoo = Product.Create("Tørshampoo", "Øjeblikkelig tørshampoo, giver volumen og friskhed", Price.FromDecimal(125m));
            _db.Add(_tørshampoo);
            _products.Add(_tørshampoo);

            _læbepomade = Product.Create("Læbepomade", "Fugtgivende læbepomade med bivoks og mint", Price.FromDecimal(55m));
            _db.Add(_læbepomade);
            _products.Add(_læbepomade);

            _fodcreme = Product.Create("Fodcreme", "Intensiv fodcreme mod tør og sprukken hud", Price.FromDecimal(105m));
            _db.Add(_fodcreme);
            _products.Add(_fodcreme);

            _negleolie = Product.Create("Negleolie", "Plejende olie til negle og neglebånd", Price.FromDecimal(75m));
            _db.Add(_negleolie);
            _products.Add(_negleolie);

            _hårfarve = Product.Create("Hårfarve", "Permanent hårfarve i medium brun", Price.FromDecimal(199m));
            _db.Add(_hårfarve);
            _products.Add(_hårfarve);

            _makeupfjerner = Product.Create("Makeupfjerner", "Effektiv makeupfjerner, også til vandfast makeup", Price.FromDecimal(115m));
            _db.Add(_makeupfjerner);
            _products.Add(_makeupfjerner);

            _barberskum = Product.Create("Barberskum", "Klassisk barberskum for tæt og glat barbering", Price.FromDecimal(80m));
            _db.Add(_barberskum);
            _products.Add(_barberskum);
        }


        private void AddBirthdayDiscounts()
        {
            _db.Add(BirthdayDiscount.Create("Fødselsdagsrabat", DiscountPercent.FromDecimal(0.50m)));
        }

        private void AddCampaignDiscounts()
        {
            _db.Add(CampaignDiscount.Create("Firesale",
                DiscountPercent.FromDecimal(0.20m), // 20% rabat
                new DateTime(2025, 06, 1, 12, 0, 0),
                new DateTime(2025, 12, 30, 12, 0, 0),
                new List<Guid> {
                    _herreklipUdenVaskFøn.Id,
                    _herreklipMedVaskFøn.Id,
                    _damefrisureInklVaskFøn.Id,
                    _damefrisureBlæs.Id,
                    _lilleTilretning.Id,
                    _storKlipning.Id,
                    _luksusKur.Id,
                    _retFarveBryn.Id,
                    _ordneBrynVipper.Id,
                    _herreKlipPermanent.Id,
                    _helfarveHalvkortHårUdenKlip.Id,
                    _helfarveHalvKortHårMedKlip.Id,
                    _helfarveLangtHårUdenKlip.Id,
                    _helfarveLangtHårMedKlip.Id,
                    _hætteStriberIHalvkortHårMedKlip.Id,
                    _hætteStriberILangtHårMedKlip.Id,
                    _permanentHalvkortHårMedKlip.Id,
                    _permanentHalvkortHårUdenKlip.Id,
                    _permanentLangtHårUdenKlip.Id,
                    _permanentLangtHårMedKlip.Id,
                    _staniolStriberIHalvkortHårUdenKlip.Id,
                    _staniolStriberIHalvkortHårMedKlip.Id,
                    _staniolStriberILangtHårUdenKlip.Id,
                    _staniolStriberILangtHårMedKlip.Id,
                    _balayageUdenKlip.Id,
                    _balayageMedKlip.Id,
                    _hårOpsætningStruktur.Id,
                    _hårOpsætningElegance.Id,
                    _hårOpsætningKompleks.Id
                }));


            // Sommerklip udsalg
            _db.Add(CampaignDiscount.Create("Sommerklip",
                DiscountPercent.FromDecimal(0.20m), // 20% rabat
                new DateTime(2026, 6, 1, 12, 0, 0),
                new DateTime(2026, 8, 1, 12, 0, 0),
                new List<Guid> {
                    _damefrisureInklVaskFøn.Id,
                    _damefrisureBlæs.Id,
                    _storKlipning.Id,
                    _herreklipMedVaskFøn.Id,
                    _herreklipUdenVaskFøn.Id
                }));

            // Vinter Permanent Udsalg (afsluttet)
            _db.Add(CampaignDiscount.Create("Vinter Permanent Udsalg",
                DiscountPercent.FromDecimal(0.30m),
                new DateTime(2025, 1, 15, 9, 0, 0),
                new DateTime(2025, 2, 28, 18, 0, 0),
                new List<Guid> {
                    _permanentHalvkortHårMedKlip.Id,
                    _permanentHalvkortHårUdenKlip.Id,
                    _permanentLangtHårUdenKlip.Id,
                    _permanentLangtHårMedKlip.Id,
                    _herreKlipPermanent.Id
                }));

            // Galla & Fest Klar
            _db.Add(CampaignDiscount.Create("Galla & Fest Klar",
                DiscountPercent.FromDecimal(0.10m),
                _currentDateTimeProvider.GetCurrentDateTime().AddMonths(2),
                _currentDateTimeProvider.GetCurrentDateTime().AddMonths(3),
                new List<Guid> {
                    _hårOpsætningElegance.Id,
                    _hårOpsætningKompleks.Id,
                    _ordneBrynVipper.Id,
                    _damefrisureBlæs.Id
                }));

            // Farve Fornyelse
            _db.Add(CampaignDiscount.Create("Farve Fornyelse",
                DiscountPercent.FromDecimal(0.25m),
                _currentDateTimeProvider.GetCurrentDateTime().AddDays(-12),
                _currentDateTimeProvider.GetCurrentDateTime().AddDays(18),
                new List<Guid> {
                    _helfarveHalvKortHårMedKlip.Id,
                    _helfarveLangtHårMedKlip.Id,
                    _balayageUdenKlip.Id,
                    _staniolStriberIHalvkortHårMedKlip.Id,
                    _staniolStriberILangtHårMedKlip.Id,
                    _hætteStriberIHalvkortHårMedKlip.Id,

                    _hætteStriberILangtHårMedKlip.Id
                }));

            // Balayage Mesterværk
            _db.Add(CampaignDiscount.Create("Balayage Mesterværk",
                DiscountPercent.FromDecimal(0.20m),
                _currentDateTimeProvider.GetCurrentDateTime().AddDays(-3),
                _currentDateTimeProvider.GetCurrentDateTime().AddDays(11),
                new List<Guid> {
                    _balayageUdenKlip.Id,
                    _balayageMedKlip.Id,
                    _staniolStriberIHalvkortHårMedKlip.Id,
                    _staniolStriberIHalvkortHårUdenKlip.Id,
                    _staniolStriberILangtHårMedKlip.Id,
                    _staniolStriberILangtHårUdenKlip.Id
                }));

            // Hurtig Tilretning Tirsdag
            _db.Add(CampaignDiscount.Create("Hurtig Tilretning Tirsdag",
                DiscountPercent.FromDecimal(0.25m),
                new DateTime(2027, 2, 2, 10, 0, 0),
                new DateTime(2027, 2, 2, 16, 0, 0),
                new List<Guid> {
        _lilleTilretning.Id
                }));

            // Forkælelsestider 
            _db.Add(CampaignDiscount.Create("Forkælelsestider",
                DiscountPercent.FromDecimal(0.15m),
                _currentDateTimeProvider.GetCurrentDateTime().AddDays(-20),
                _currentDateTimeProvider.GetCurrentDateTime().AddDays(2),
                new List<Guid>
                {
                    _luksusKur.Id,
                    _retFarveBryn.Id,
                }));

            // Langt Hår Helfarve Tilbud
            _db.Add(CampaignDiscount.Create("Langt Hår Helfarve Tilbud",
                DiscountPercent.FromDecimal(0.20m), // 20% rabat
                new DateTime(2027, 1, 20, 9, 0, 0),
                new DateTime(2027, 2, 15, 18, 0, 0),
                new List<Guid> {
                    _helfarveLangtHårMedKlip.Id,
                    _helfarveLangtHårUdenKlip.Id,
                    _helfarveHalvKortHårMedKlip.Id
                }));
        }

        private void AddLoyaltyDiscounts()
        {
            _db.Add(LoyaltyDiscount.Create("Stamkunde Bronze", 5, DiscountPercent.FromDecimal(0.05m)));
            _db.Add(LoyaltyDiscount.CreateWithProductDiscount("Stamkunde Sølv", 10, DiscountPercent.FromDecimal(0.10m), DiscountPercent.FromDecimal(0.5m)));
            _db.Add(LoyaltyDiscount.CreateWithProductDiscount("Stamkunde Guld", 15, DiscountPercent.FromDecimal(0.15m), DiscountPercent.FromDecimal(0.10m)));
        }

        private void AddEmployees()
        {

            _idaChristensen = Employee.Create(
                Name.FromStrings("Ida", "Christensen", "Marie"),
                Email.FromString("ida@bellahairfrisor.dk"),
                PhoneNumber.FromString("70801234"),
                Address.Create("Nørregade", "Vejle", "45", 7100, 9),
                new List<Treatment>
                {
                    _damefrisureInklVaskFøn,
                    _damefrisureBlæs,
                    _storKlipning,
                    _lilleTilretning,
                    _helfarveHalvkortHårUdenKlip,
                    _helfarveHalvKortHårMedKlip,
                    _helfarveLangtHårUdenKlip,
                    _helfarveLangtHårMedKlip,
                    _staniolStriberIHalvkortHårUdenKlip,
                    _staniolStriberIHalvkortHårMedKlip,
                    _staniolStriberILangtHårUdenKlip,
                    _staniolStriberILangtHårMedKlip,
                    _hårOpsætningStruktur,
                    _hårOpsætningElegance,
                    _luksusKur
                }
            );


            _db.Employees.Add(_idaChristensen);
            _employees.Add(_idaChristensen);


            // --- Jonas Hansen: Herreklip & Kort Hår Opsætning ---
            _jonasHansen = Employee.Create(
                Name.FromStrings("Jonas", "Hansen"),
                Email.FromString("jonas@bellahairfrisor.dk"),
                PhoneNumber.FromString("70805678"),
                Address.Create("Fredericiavej", "Vejle", "112", 7100, 2),
                new List<Treatment>
                {
                    _herreklipMedVaskFøn,
                    _herreklipUdenVaskFøn,
                    _herreKlipPermanent,
                    _lilleTilretning,
                    _storKlipning,
                    _hårOpsætningStruktur
                }
            );

            _db.Employees.Add(_jonasHansen);
            _employees.Add(_jonasHansen);

            // --- Sofie Nielsen: Komplekse Opsætninger & Balayage Specialist ---
            _sofieNielsen = Employee.Create(
                Name.FromStrings("Sofie", "Nielsen", "Hansen"),
                Email.FromString("sofie@bellahairfrisor.dk"),
                PhoneNumber.FromString("70809012"),
                Address.Create("Søndertorv", "Vejle", "7", 7100),
                new List<Treatment>
                {
                    _damefrisureInklVaskFøn,
                    _storKlipning,
                    _balayageUdenKlip,
                    _balayageMedKlip,
                    _staniolStriberILangtHårMedKlip,
                    _staniolStriberILangtHårUdenKlip,
                    _staniolStriberIHalvkortHårMedKlip,
                    _staniolStriberIHalvkortHårUdenKlip,
                    _hårOpsætningElegance,
                    _hårOpsætningKompleks,
                    _luksusKur
                }
            );

            _db.Employees.Add(_sofieNielsen);
            _employees.Add(_sofieNielsen);


            // --- Lars Mikkelsen: Permanent & Detaljeklip ---
            _larsMikkelsen = Employee.Create(
                Name.FromStrings("Lars", "Mikkelsen"),
                Email.FromString("lars@bellahairfrisor.dk"),
                PhoneNumber.FromString("70803456"),
                Address.Create("Vestergade", "Vejle", "9", 7100, 1),
                new List<Treatment>
                {
                    _luksusKur,
                    _damefrisureInklVaskFøn,
                    _storKlipning,
                    _lilleTilretning,
                    _permanentHalvkortHårMedKlip,
                    _permanentHalvkortHårUdenKlip,
                    _permanentLangtHårUdenKlip,
                    _permanentLangtHårMedKlip,
                    _hætteStriberIHalvkortHårMedKlip,
                    _hætteStriberILangtHårMedKlip,
                    _herreklipMedVaskFøn
                }
            );

            _db.Employees.Add(_larsMikkelsen);
            _employees.Add(_larsMikkelsen);


            // --- Signe Jørgensen: Bryn/Vipper & Dameklip ---
            _signeJørgensen = Employee.Create(
                Name.FromStrings("Signe", "Jørgensen", "Aia"),
                Email.FromString("signe@bellahairfrisor.dk"),
                PhoneNumber.FromString("70807890"),
                Address.Create("Gl Landevej", "Børkop", "22", 7080, 2),
                new List<Treatment>
                {
                    // Signe's ekspertise: Tillægsbehandlinger og grundlæggende styling
                    _damefrisureInklVaskFøn,
                    _damefrisureBlæs,
                    _lilleTilretning,
                    _retFarveBryn,
                    _ordneBrynVipper,
                    _helfarveHalvKortHårMedKlip, // Fokuserer på helfarve i forbindelse med klip
                    _helfarveLangtHårMedKlip,
                    _hårOpsætningElegance
                }
            );

            _db.Employees.Add(_signeJørgensen);
            _employees.Add(_signeJørgensen);


            // --- Mads Knudsen: Permanent, Farve & Herreklip (All-rounder) ---
            _madsKnudsen = Employee.Create(
                Name.FromStrings("Mads", "Knudsen"),
                Email.FromString("mads@bellahairfrisor.dk"),
                PhoneNumber.FromString("70802345"),
                Address.Create("Skolegade", "Egtved", "5", 6040, 6),
                new List<Treatment>
                {
                    _luksusKur,
                    _herreklipMedVaskFøn,
                    _herreklipUdenVaskFøn,
                    _damefrisureInklVaskFøn,
                    _storKlipning,
                    _herreKlipPermanent,
                    _permanentHalvkortHårMedKlip,
                    _permanentLangtHårMedKlip,
                    _herreKlipPermanent,
                    _helfarveHalvkortHårUdenKlip,
                    _helfarveHalvKortHårMedKlip,
                    _helfarveLangtHårMedKlip,
                    _helfarveLangtHårUdenKlip,
                    _hætteStriberIHalvkortHårMedKlip,
                    _hætteStriberILangtHårMedKlip,
                    _hårOpsætningStruktur
                }
            );

            _db.Employees.Add(_madsKnudsen);
            _employees.Add(_madsKnudsen);


            // --- Anna Petersen: Balayage & Langt Hår Specialisten ---
            _annaPetersen = Employee.Create(
                Name.FromStrings("Anna", "Petersen"),
                Email.FromString("anna@bellahairfrisor.dk"),
                PhoneNumber.FromString("70806789"),
                Address.Create("Fjordgade", "Brejning", "88", 7080),
                new List<Treatment>
                {
                    // Anna's ekspertise: Langt hår, store farveprojekter og Kompleks Opsætning
                    _damefrisureInklVaskFøn,
                    _storKlipning,
                    _balayageUdenKlip,
                    _balayageMedKlip,
                    _staniolStriberIHalvkortHårMedKlip,
                    _staniolStriberILangtHårMedKlip,
                    _helfarveHalvKortHårMedKlip,
                    _hårOpsætningElegance,
                    _hårOpsætningKompleks
                }
            );
            _db.Employees.Add(_annaPetersen);
            _employees.Add(_annaPetersen);
            _db.SaveChanges();
        }

        private void AddTreatment()
        {
            _herreklipMedVaskFøn = Treatment.Create("Herreklip inkl vask og føn", Price.FromDecimal(350m), DurationMinutes.FromInt(30));
            _herreklipUdenVaskFøn = Treatment.Create("Herreklip", Price.FromDecimal(300m), DurationMinutes.FromInt(25));
            _damefrisureInklVaskFøn = Treatment.Create("Damefrisure inkl vask og føn", Price.FromDecimal(445m), DurationMinutes.FromInt(40));
            _damefrisureBlæs = Treatment.Create("Damefrisure og blæs", Price.FromDecimal(400m), DurationMinutes.FromInt(45));
            _lilleTilretning = Treatment.Create("Lille tilretning ved øre og nakke", Price.FromDecimal(200m), DurationMinutes.FromInt(15));
            _storKlipning = Treatment.Create("Stor klipning", Price.FromDecimal(495m), DurationMinutes.FromInt(60));
            _retFarveBryn = Treatment.Create("Ret og farve bryn", Price.FromDecimal(110m), DurationMinutes.FromInt(15));
            _ordneBrynVipper = Treatment.Create("Ordnet bryn og vipper", Price.FromDecimal(330m), DurationMinutes.FromInt(35));
            _herreKlipPermanent = Treatment.Create("Herreklip med permanent", Price.FromDecimal(875m), DurationMinutes.FromInt(90));
            _helfarveHalvkortHårUdenKlip = Treatment.Create("Helfarve halvkort hår uden klip", Price.FromDecimal(575m), DurationMinutes.FromInt(120));
            _helfarveHalvKortHårMedKlip = Treatment.Create("Helfarve halvkort hår med klip", Price.FromDecimal(1020m), DurationMinutes.FromInt(150));
            _helfarveLangtHårMedKlip = Treatment.Create("Helfarve langt hår med klip", Price.FromDecimal(1300m), DurationMinutes.FromInt(170));
            _helfarveLangtHårUdenKlip = Treatment.Create("Helfarve langt hår uden klip", Price.FromDecimal(925m), DurationMinutes.FromInt(145));
            _hætteStriberIHalvkortHårMedKlip = Treatment.Create("Hætte striber i halvkort hår med klip", Price.FromDecimal(505m), DurationMinutes.FromInt(120));
            _hætteStriberILangtHårMedKlip = Treatment.Create("Hætte striber i langth hår med klip", Price.FromDecimal(725m), DurationMinutes.FromInt(160));
            _permanentHalvkortHårMedKlip = Treatment.Create("Permanent i halvkort hår med klip", Price.FromDecimal(1045m), DurationMinutes.FromInt(175));
            _permanentHalvkortHårUdenKlip = Treatment.Create("Permanent i halvkort hår uden klip", Price.FromDecimal(600m), DurationMinutes.FromInt(120));
            _permanentLangtHårMedKlip = Treatment.Create("Permanent i langt hår med klip", Price.FromDecimal(1400m), DurationMinutes.FromInt(230));
            _permanentLangtHårUdenKlip = Treatment.Create("Permanent i langt hår uden klip", Price.FromDecimal(1150m), DurationMinutes.FromInt(200));
            _staniolStriberIHalvkortHårUdenKlip = Treatment.Create("Staniol striber i halvkort hår uden klip", Price.FromDecimal(590m), DurationMinutes.FromInt(160));
            _staniolStriberIHalvkortHårMedKlip = Treatment.Create("Staniol striber i halvkort hår med klip", Price.FromDecimal(1035m), DurationMinutes.FromInt(190));
            _staniolStriberILangtHårUdenKlip = Treatment.Create("Staniol striber i langt hår uden klip", Price.FromDecimal(725m), DurationMinutes.FromInt(190));
            _staniolStriberILangtHårMedKlip = Treatment.Create("Staniol striber i langt hår med klip", Price.FromDecimal(500m), DurationMinutes.FromInt(150));
            _balayageUdenKlip = Treatment.Create("Balayage uden klip", Price.FromDecimal(950m), DurationMinutes.FromInt(190));
            _balayageMedKlip = Treatment.Create("Balyage med klip", Price.FromDecimal(1300m), DurationMinutes.FromInt(180));
            _hårOpsætningStruktur = Treatment.Create("Håropsætning Struktur", Price.FromDecimal(250m), DurationMinutes.FromInt(30));
            _hårOpsætningElegance = Treatment.Create("Håropsætning Elegance", Price.FromDecimal(450m), DurationMinutes.FromInt(60));
            _hårOpsætningKompleks = Treatment.Create("Håropsætning Kompleks", Price.FromDecimal(800m), DurationMinutes.FromInt(120));
            _luksusKur = Treatment.Create("Luksuskur til hår", Price.FromDecimal(1100m), DurationMinutes.FromInt(120));

            _db.Add(_herreklipMedVaskFøn);
            _db.Add(_herreklipUdenVaskFøn);
            _db.Add(_damefrisureInklVaskFøn);
            _db.Add(_damefrisureBlæs);
            _db.Add(_lilleTilretning);
            _db.Add(_storKlipning);
            _db.Add(_retFarveBryn);
            _db.Add(_ordneBrynVipper);
            _db.Add(_herreKlipPermanent);
            _db.Add(_helfarveHalvkortHårUdenKlip);
            _db.Add(_helfarveHalvKortHårMedKlip);
            _db.Add(_hætteStriberIHalvkortHårMedKlip);
            _db.Add(_permanentHalvkortHårMedKlip);
            _db.Add(_permanentHalvkortHårUdenKlip);
            _db.Add(_staniolStriberIHalvkortHårUdenKlip);
            _db.Add(_staniolStriberIHalvkortHårMedKlip);
            _db.Add(_balayageUdenKlip);
            _db.Add(_balayageMedKlip);
            _db.Add(_hårOpsætningStruktur); // Håropsætning Kort
            _db.Add(_hårOpsætningElegance); // Håropsætning Mellem
            _db.Add(_hårOpsætningKompleks); // Håropsætning Lang

            _treatments = new List<Treatment>
                {
                    _herreklipUdenVaskFøn,
                    _herreklipMedVaskFøn,
                    _damefrisureInklVaskFøn,
                    _damefrisureBlæs,
                    _lilleTilretning,
                    _storKlipning,
                    _luksusKur,
                    _retFarveBryn,
                    _ordneBrynVipper,
                    _herreKlipPermanent,
                    _permanentHalvkortHårMedKlip,
                    _permanentHalvkortHårUdenKlip,
                    _permanentLangtHårUdenKlip,
                    _permanentLangtHårMedKlip,
                    _helfarveHalvkortHårUdenKlip,
                    _helfarveHalvKortHårMedKlip,
                    _helfarveLangtHårUdenKlip,
                    _helfarveLangtHårMedKlip,
                    _hætteStriberIHalvkortHårMedKlip,
                    _hætteStriberILangtHårMedKlip,
                    _staniolStriberIHalvkortHårUdenKlip,
                    _staniolStriberIHalvkortHårMedKlip,
                    _staniolStriberILangtHårUdenKlip,
                    _staniolStriberILangtHårMedKlip,
                    _balayageUdenKlip,
                    _balayageMedKlip,
                    _hårOpsætningStruktur,
                    _hårOpsætningElegance,
                    _hårOpsætningKompleks
                };
            _db.SaveChanges();
        }

        // Bruges da Bookings skal have en ICurrentDateTimeProvider som giver deres CreatedDate som skal være i fortiden i forhold til StartTime.
        internal class PastDateTimeProvider : ICurrentDateTimeProvider
        {
            DateTime ICurrentDateTimeProvider.GetCurrentDateTime() => DateTime.Now.AddDays(-31);
        }
    }

}
