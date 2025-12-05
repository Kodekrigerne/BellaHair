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
using BellaHair.Ports.Bookings;
using Bogus;
using FixtureBuilder;

namespace BellaHair.Presentation.WebUI
{
    // Dennis, Linnea, Mikkel Dahlmann
    /// <summary>
    /// Provides a method for adding hardcoded example data to BellaHairContext.
    /// </summary>
    public class DataProvider
    {
        private readonly BellaHairContext _db;
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider = new CurrentDateTimeProvider();
        private readonly ICurrentDateTimeProvider _mockPastDateTimeProvider = new PastDateTimeProvider();
        private readonly IServiceProvider _serviceProvider;

        public DataProvider(BellaHairContext db, IServiceProvider serviceProvider)
        {
            _db = db;
            _serviceProvider = serviceProvider;
        }

        // Settings
        private const int NoOfPastBookings = 100;
        private const int NoOfFutureBookings = 100;
        private const int NoOfCustomers = 50;

        // Lists
        private readonly List<Employee> _employees = [];
        private readonly List<PrivateCustomer> _customers = [];
        private readonly List<Product> _products = [];

        // Treatment Fields 
        private Treatment? _herreklipUdenVaskFøn;
        private Treatment? _herreklipMedVaskFøn;
        private Treatment? _damefrisureInklVaskFøn;
        private Treatment? _damefrisureBlæs;
        private Treatment? _lilleTilretning;
        private Treatment? _storKlipning;
        private Treatment? _luksusKur;
        private Treatment? _retFarveBryn;
        private Treatment? _ordneBrynVipper;
        private Treatment? _herreKlipPermanent;
        private Treatment? _helfarveHalvkortHårUdenKlip;
        private Treatment? _helfarveHalvKortHårMedKlip;
        private Treatment? _helfarveLangtHårUdenKlip;
        private Treatment? _helfarveLangtHårMedKlip;
        private Treatment? _hætteStriberIHalvkortHårMedKlip;
        private Treatment? _hætteStriberILangtHårMedKlip;
        private Treatment? _permanentHalvkortHårMedKlip;
        private Treatment? _permanentHalvkortHårUdenKlip;
        private Treatment? _permanentLangtHårUdenKlip;
        private Treatment? _permanentLangtHårMedKlip;
        private Treatment? _staniolStriberIHalvkortHårUdenKlip;
        private Treatment? _staniolStriberIHalvkortHårMedKlip;
        private Treatment? _staniolStriberILangtHårUdenKlip;
        private Treatment? _staniolStriberILangtHårMedKlip;
        private Treatment? _balayageUdenKlip;
        private Treatment? _balayageMedKlip;
        private Treatment? _hårOpsætningStruktur;
        private Treatment? _hårOpsætningElegance;
        private Treatment? _hårOpsætningKompleks;

        // Employee fields
        private Employee? _idaChristensen;
        private Employee? _jonasHansen;
        private Employee? _sofieNielsen;
        private Employee? _larsMikkelsen;
        private Employee? _signeJørgensen;
        private Employee? _madsKnudsen;
        private Employee? _annaPetersen;

        public async Task AddData()
        {
            AddProducts();
            AddPrivateCustomersUsingBogus();
            AddTreatment();
            AddEmployees();
            AddLoyaltyDiscounts();
            AddCampaignDiscounts();
            AddPastBookingsUsingBogusAndHandler();
            await PayAndInvoicePastBookings();
            AddBookingsUsingBogusAndHandler();
            AddBirthdayDiscounts();

            _db.SaveChanges();
        }

        private async Task PayAndInvoicePastBookings()
        {
            var queryHandler = _serviceProvider.GetRequiredService<IBookingQuery>();

            var commandHandler = _serviceProvider.GetRequiredService<IBookingCommand>();

            var pastBookings = await queryHandler.GetAllOldAsync();
            pastBookings = pastBookings.OrderBy(b => b.StartDateTime);

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
            _db.SaveChanges();
        }

        private void AddBookingsUsingBogusAndHandler()
        {
            var now = _currentDateTimeProvider.GetCurrentDateTime();

            for (int i = 0; i < NoOfFutureBookings; i++)
            {
                try
                {
                    var random = new Random();

                    using var scope = _serviceProvider.CreateScope();

                    var employee = _employees[random.Next(0, 7)];
                    var treatment = employee.Treatments[random.Next(0, employee.Treatments.Count)];

                    List<CreateProductLine> productLines = [];

                    for (int p = 0; p < random.Next(0, 2); p++)
                    {
                        var productToList = _products[random.Next(0, _products.Count)];
                        var productId = productToList.Id;
                        var quantity = random.Next(1, 1);
                        productLines.Add(new CreateProductLine(quantity, productId));
                    }

                    var product = _products[random.Next(0, _products.Count)];
                    var customer = _customers[random.Next(0, _customers.Count)];

                    var bookingFaker = new Faker<CreateBookingCommand>("nb_NO")
                        .CustomInstantiator(f =>
                        {
                            var bookingDate = f.Date.Between(now.AddDays(1), now.AddDays(30));
                            while (bookingDate.DayOfWeek == DayOfWeek.Saturday || bookingDate.DayOfWeek == DayOfWeek.Sunday)
                            {
                                bookingDate = f.Date.Between(now.AddDays(1), now.AddDays(30));
                            }
                            var bookingHour = f.Random.Int(10, 17 - treatment.DurationMinutes.Value / 60);
                            var bookingMinutes = f.Random.Int(0, Math.Max(0, 60 - treatment.DurationMinutes.Value));
                            bookingMinutes -= (bookingMinutes % 15);

                            return new CreateBookingCommand(new DateTime(bookingDate.Year, bookingDate.Month, bookingDate.Day, bookingHour, bookingMinutes, 0), employee.Id, customer.Id, treatment.Id, productLines);
                        });

                    var booking = bookingFaker.Generate();

                    var bookingCommandHandler = scope.ServiceProvider.GetRequiredService<IBookingCommand>();
                    bookingCommandHandler.CreateBooking(booking).Wait();

                    scope.Dispose();
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
        }

        private void AddPastBookingsUsingBogusAndHandler()
        {
            var now = _mockPastDateTimeProvider.GetCurrentDateTime();

            List<CreateBookingCommand> createBookingCommands = [];

            for (int i = 0; i < NoOfPastBookings; i++)
            {
                try
                {
                    var random = new Random();
                    var employee = _employees[random.Next(0, 7)];
                    var treatment = employee.Treatments[random.Next(0, employee.Treatments.Count)];
                    List<CreateProductLine> productLines = [];
                    for (int p = 0; p < random.Next(0, 2); p++)
                    {
                        var productToList = _products[random.Next(0, _products.Count)];
                        var productId = productToList.Id;
                        var quantity = random.Next(1, 1);
                        productLines.Add(new CreateProductLine(quantity, productId));
                    }
                    var product = _products[random.Next(0, _products.Count)];
                    var customer = _customers[random.Next(0, _customers.Count)];

                    var bookingFaker = new Faker<CreateBookingCommand>("nb_NO")
                        .CustomInstantiator(f =>
                        {
                            var bookingDate = f.Date.Between(now.AddDays(1), now.AddDays(30));
                            while (bookingDate.DayOfWeek == DayOfWeek.Saturday || bookingDate.DayOfWeek == DayOfWeek.Sunday)
                            {
                                bookingDate = f.Date.Between(now.AddDays(1), now.AddDays(30));
                            }
                            var bookingHour = f.Random.Int(10, 17 - treatment.DurationMinutes.Value / 60);
                            var bookingMinutes = f.Random.Int(0, Math.Max(0, 60 - treatment.DurationMinutes.Value));
                            bookingMinutes -= (bookingMinutes % 15);

                            return new CreateBookingCommand(new DateTime(bookingDate.Year, bookingDate.Month, bookingDate.Day, bookingHour, bookingMinutes, 0), employee.Id, customer.Id, treatment.Id, productLines);
                        });

                    createBookingCommands.Add(bookingFaker.Generate());
                }

                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }

            createBookingCommands = createBookingCommands.OrderBy(c => c.StartDateTime).ToList();

            foreach (var command in createBookingCommands)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();

                    var bookingCommandHandler = scope.ServiceProvider.GetRequiredService<IBookingCommand>();
                    Fixture.New(bookingCommandHandler).WithField("_currentDateTimeProvider", _mockPastDateTimeProvider).Build();

                    var overlapChecker = scope.ServiceProvider.GetRequiredService<IBookingOverlapChecker>();
                    Fixture.New(overlapChecker).WithField("_currentDateTimeProvider", _mockPastDateTimeProvider).Build();

                    bookingCommandHandler.CreateBooking(command).Wait();

                    scope.Dispose();
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
        }

        private void AddPrivateCustomersUsingBogus()
        {
            var now = _currentDateTimeProvider.GetCurrentDateTime();

            for (int i = 0; i < NoOfCustomers; i++)
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
            var productData = new[]
            {
                ("Shampoo", "Mild shampoo for alle hårtyper", 120m),
                ("Balsam", "Nærende balsam for glat og skinnende hår", 150m),
                ("Hårvoks", "Fleksibel hårvoks for tekstur og hold", 200m),
                ("Hårspray", "Langtidsholdbar hårspray med let finish", 180m),
                ("Hårkur", "Intensiv hårkur til genopbygning af skadet hår", 250m),
                ("Leave-in Conditioner", "Let leave-in conditioner for nem styling", 130m),
                ("Ansigtsrens", "Skånsom rens til daglig brug, fjerner urenheder og makeup", 160m),
                ("Dagcreme", "Fugtgivende dagcreme med SPF 30, beskytter mod solen", 280m),
                ("Natcreme", "Rig natcreme, genopbygger huden mens du sover", 300m),
                ("Serum", "Anti-age serum med hyaluronsyre for dyb hydrering", 350m),
                ("Ã˜jencreme", "Let øjencreme reducerer poser og mørke rande", 220m),
                ("Bodylotion", "Blødgørende bodylotion med naturlige olier", 110m),
                ("Håndcreme", "Reparerende håndcreme, ideel til tørre hænder", 85m),
                ("Body Wash", "Opfriskende kropsvask med citrusduft", 95m),
                ("Eksfoliering", "Kropsskrub med fine korn, fjerner døde hudceller", 145m),
                ("Solcreme", "Vandfast solcreme SPF 50 til hele kroppen", 190m),
                ("Aftershave", "Beroligende aftershave balm uden alkohol", 175m),
                ("Skægolie", "Blødgørende skægolie med cedertræ og patchouli", 195m),
                ("Stylingmousse", "Volumengivende mousse for let og luftig frisure", 140m),
                ("Tørshampoo", "Ã˜jeblikkelig tørshampoo, giver volumen og friskhed", 125m),
                ("Læbepomade", "Fugtgivende læbepomade med bivoks og mint", 55m),
                ("Fodcreme", "Intensiv fodcreme mod tør og sprukken hud", 105m),
                ("Negleolie", "Plejende olie til negle og neglebånd", 75m),
                ("Hårfarve", "Permanent hårfarve i medium brun", 199m),
                ("Makeupfjerner", "Effektiv makeupfjerner, også til vandfast makeup", 115m),
                ("Barberskum", "Klassisk barberskum for tæt og glat barbering", 80m)
            };

            foreach (var (name, description, priceValue) in productData)
            {
                var price = Price.FromDecimal(priceValue);
                var product = Product.Create(name, description, price);

                _db.Add(product);
                _products.Add(product);
            }

            _db.SaveChanges();
        }

        private void AddLoyaltyDiscounts()
        {
            _db.Add(LoyaltyDiscount.Create("Loyalty test", 1, DiscountPercent.FromDecimal(0.50m)));
            _db.Add(LoyaltyDiscount.Create("Stamkunde Bronze", 5, DiscountPercent.FromDecimal(0.05m)));
            _db.Add(LoyaltyDiscount.CreateWithProductDiscount("Stamkunde Sølv", 10, DiscountPercent.FromDecimal(0.10m), DiscountPercent.FromDecimal(0.5m)));
            _db.Add(LoyaltyDiscount.CreateWithProductDiscount("Stamkunde Guld", 20, DiscountPercent.FromDecimal(0.15m), DiscountPercent.FromDecimal(0.10m)));

            _db.SaveChanges();
        }

        private void AddBirthdayDiscounts()
        {
            _db.Add(BirthdayDiscount.Create("Fødselsdagsrabat", DiscountPercent.FromDecimal(0.50m)));

            _db.SaveChanges();
        }

        private void AddCampaignDiscounts()
        {
            // Test Campaign
            _db.Add(CampaignDiscount.Create("Firesale",
                DiscountPercent.FromDecimal(0.20m),
                new DateTime(2025, 06, 1, 12, 0, 0),
                new DateTime(2025, 12, 30, 12, 0, 0),
                new List<Guid> {
                    _herreklipUdenVaskFøn!.Id,
                    _herreklipMedVaskFøn!.Id,
                    _damefrisureInklVaskFøn!.Id,
                    _damefrisureBlæs!.Id,
                    _lilleTilretning!.Id,
                    _storKlipning!.Id,
                    _luksusKur!.Id,
                    _retFarveBryn!.Id,
                    _ordneBrynVipper!.Id,
                    _herreKlipPermanent!.Id,
                    _helfarveHalvkortHårUdenKlip!.Id,
                    _helfarveHalvKortHårMedKlip!.Id,
                    _helfarveLangtHårUdenKlip!.Id,
                    _helfarveLangtHårMedKlip!.Id,
                    _hætteStriberIHalvkortHårMedKlip!.Id,
                    _hætteStriberILangtHårMedKlip!.Id,
                    _permanentHalvkortHårMedKlip!.Id,
                    _permanentHalvkortHårUdenKlip!.Id,
                    _permanentLangtHårUdenKlip!.Id,
                    _permanentLangtHårMedKlip!.Id,
                    _staniolStriberIHalvkortHårUdenKlip!.Id,
                    _staniolStriberIHalvkortHårMedKlip!.Id,
                    _staniolStriberILangtHårUdenKlip!.Id,
                    _staniolStriberILangtHårMedKlip!.Id,
                    _balayageUdenKlip!.Id,
                    _balayageMedKlip!.Id,
                    _hårOpsætningStruktur!.Id,
                    _hårOpsætningElegance!.Id,
                    _hårOpsætningKompleks!.Id
                }));


            // Sommerklip udsalg
            _db.Add(CampaignDiscount.Create("Sommerklip",
                DiscountPercent.FromDecimal(0.20m),
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
                DiscountPercent.FromDecimal(0.20m),
                new DateTime(2027, 1, 20, 9, 0, 0),
                new DateTime(2027, 2, 15, 18, 0, 0),
                new List<Guid> {
                    _helfarveLangtHårMedKlip.Id,
                    _helfarveLangtHårUdenKlip.Id,
                    _helfarveHalvKortHårMedKlip.Id
                }));

            _db.SaveChanges();
        }

        private void AddEmployees()
        {
            // Ida Christensen
            _idaChristensen = Employee.Create(
                Name.FromStrings("Ida", "Christensen", "Marie"),
                Email.FromString("ida@bellahairfrisor.dk"),
                PhoneNumber.FromString("70801234"),
                Address.Create("Nørregade", "Vejle", "45", 7100, 9),
                new List<Treatment>
                {
                    _damefrisureInklVaskFøn!,
                    _damefrisureBlæs!,
                    _storKlipning!,
                    _lilleTilretning!,
                    _helfarveHalvkortHårUdenKlip!,
                    _helfarveHalvKortHårMedKlip!,
                    _helfarveLangtHårUdenKlip!,
                    _helfarveLangtHårMedKlip!,
                    _staniolStriberIHalvkortHårUdenKlip!,
                    _staniolStriberIHalvkortHårMedKlip!,
                    _staniolStriberILangtHårUdenKlip!,
                    _staniolStriberILangtHårMedKlip!,
                    _hårOpsætningStruktur!,
                    _hårOpsætningElegance!,
                    _luksusKur!
                }
            );

            _db.Employees.Add(_idaChristensen);
            _employees.Add(_idaChristensen);

            // Jonas Hansen
            _jonasHansen = Employee.Create(
                Name.FromStrings("Jonas", "Hansen"),
                Email.FromString("jonas@bellahairfrisor.dk"),
                PhoneNumber.FromString("70805678"),
                Address.Create("Fredericiavej", "Vejle", "112", 7100, 2),
                new List<Treatment>
                {
                    _herreklipMedVaskFøn!,
                    _herreklipUdenVaskFøn!,
                    _herreKlipPermanent!,
                    _lilleTilretning!,
                    _storKlipning!,
                    _hårOpsætningStruktur!
                }
            );

            _db.Employees.Add(_jonasHansen);
            _employees.Add(_jonasHansen);

            // Sofie Nielsen
            _sofieNielsen = Employee.Create(
                Name.FromStrings("Sofie", "Nielsen", "Hansen"),
                Email.FromString("sofie@bellahairfrisor.dk"),
                PhoneNumber.FromString("70809012"),
                Address.Create("Søndertorv", "Vejle", "7", 7100),
                new List<Treatment>
                {
                    _damefrisureInklVaskFøn!,
                    _storKlipning!,
                    _balayageUdenKlip!,
                    _balayageMedKlip!,
                    _staniolStriberILangtHårMedKlip!,
                    _staniolStriberILangtHårUdenKlip!,
                    _staniolStriberIHalvkortHårMedKlip!,
                    _staniolStriberIHalvkortHårUdenKlip!,
                    _hårOpsætningElegance!,
                    _hårOpsætningKompleks!,
                    _luksusKur!
                }
            );

            _db.Employees.Add(_sofieNielsen);
            _employees.Add(_sofieNielsen);

            // Lars Mikkelsen
            _larsMikkelsen = Employee.Create(
                Name.FromStrings("Lars", "Mikkelsen"),
                Email.FromString("lars@bellahairfrisor.dk"),
                PhoneNumber.FromString("70803456"),
                Address.Create("Vestergade", "Vejle", "9", 7100, 1),
                new List<Treatment>
                {
                    _luksusKur!,
                    _damefrisureInklVaskFøn!,
                    _storKlipning!,
                    _lilleTilretning!,
                    _permanentHalvkortHårMedKlip!,
                    _permanentHalvkortHårUdenKlip!,
                    _permanentLangtHårUdenKlip!,
                    _permanentLangtHårMedKlip!,
                    _hætteStriberIHalvkortHårMedKlip!,
                    _hætteStriberILangtHårMedKlip!,
                    _herreklipMedVaskFøn!
                }
            );

            _db.Employees.Add(_larsMikkelsen);
            _employees.Add(_larsMikkelsen);

            // Signe Jørgensen
            _signeJørgensen = Employee.Create(
                Name.FromStrings("Signe", "Jørgensen", "Aia"),
                Email.FromString("signe@bellahairfrisor.dk"),
                PhoneNumber.FromString("70807890"),
                Address.Create("Gl Landevej", "Børkop", "22", 7080, 2),
                new List<Treatment>
                {
                    _damefrisureInklVaskFøn!,
                    _damefrisureBlæs!,
                    _lilleTilretning!,
                    _retFarveBryn!,
                    _ordneBrynVipper!,
                    _helfarveHalvKortHårMedKlip!,
                    _helfarveLangtHårMedKlip!,
                    _hårOpsætningElegance!
                }
            );

            _db.Employees.Add(_signeJørgensen);
            _employees.Add(_signeJørgensen);

            // Mads Knudsen
            _madsKnudsen = Employee.Create(
                Name.FromStrings("Mads", "Knudsen"),
                Email.FromString("mads@bellahairfrisor.dk"),
                PhoneNumber.FromString("70802345"),
                Address.Create("Skolegade", "Egtved", "5", 6040, 6),
                new List<Treatment>
                {
                    _luksusKur!,
                    _herreklipMedVaskFøn!,
                    _herreklipUdenVaskFøn!,
                    _damefrisureInklVaskFøn!,
                    _storKlipning!,
                    _herreKlipPermanent!,
                    _permanentHalvkortHårMedKlip!,
                    _permanentLangtHårMedKlip!,
                    _herreKlipPermanent!,
                    _helfarveHalvkortHårUdenKlip!,
                    _helfarveHalvKortHårMedKlip!,
                    _helfarveLangtHårMedKlip!,
                    _helfarveLangtHårUdenKlip!,
                    _hætteStriberIHalvkortHårMedKlip!,
                    _hætteStriberILangtHårMedKlip!,
                    _hårOpsætningStruktur!
                }
            );

            _db.Employees.Add(_madsKnudsen);
            _employees.Add(_madsKnudsen);

            // Anna Petersen
            _annaPetersen = Employee.Create(
                Name.FromStrings("Anna", "Petersen"),
                Email.FromString("anna@bellahairfrisor.dk"),
                PhoneNumber.FromString("70806789"),
                Address.Create("Fjordgade", "Brejning", "88", 7080),
                new List<Treatment>
                {
                    _damefrisureInklVaskFøn!,
                    _storKlipning!,
                    _balayageUdenKlip!,
                    _balayageMedKlip!,
                    _staniolStriberIHalvkortHårMedKlip!,
                    _staniolStriberILangtHårMedKlip!,
                    _helfarveHalvKortHårMedKlip!,
                    _hårOpsætningElegance!,
                    _hårOpsætningKompleks!
                }
            );
            _db.Employees.Add(_annaPetersen);
            _employees.Add(_annaPetersen);

            _db.SaveChanges();
        }

        private void AddTreatment()
        {
            _herreklipMedVaskFøn = CreateAndAddTreatment("Herreklip inkl vask og føn", 350m, 30);
            _herreklipUdenVaskFøn = CreateAndAddTreatment("Herreklip", 300m, 30);
            _damefrisureInklVaskFøn = CreateAndAddTreatment("Damefrisure inkl vask og føn", 445m, 45);
            _damefrisureBlæs = CreateAndAddTreatment("Damefrisure og blæs", 400m, 45);
            _lilleTilretning = CreateAndAddTreatment("Lille tilretning ved øre og nakke", 200m, 30);
            _storKlipning = CreateAndAddTreatment("Stor klipning", 495m, 60);
            _retFarveBryn = CreateAndAddTreatment("Ret og farve bryn", 110m, 30);
            _ordneBrynVipper = CreateAndAddTreatment("Ordnet bryn og vipper", 330m, 45);
            _herreKlipPermanent = CreateAndAddTreatment("Herreklip med permanent", 875m, 90);
            _helfarveHalvkortHårUdenKlip = CreateAndAddTreatment("Helfarve halvkort hår uden klip", 575m, 120);
            _helfarveHalvKortHårMedKlip = CreateAndAddTreatment("Helfarve halvkort hår med klip", 1020m, 150);
            _helfarveLangtHårMedKlip = CreateAndAddTreatment("Helfarve langt hår med klip", 1300m, 165);
            _helfarveLangtHårUdenKlip = CreateAndAddTreatment("Helfarve langt hår uden klip", 925m, 135);
            _hætteStriberIHalvkortHårMedKlip = CreateAndAddTreatment("Hætte striber i halvkort hår med klip", 505m, 120);
            _hætteStriberILangtHårMedKlip = CreateAndAddTreatment("Hætte striber i langth hår med klip", 725m, 165);
            _permanentHalvkortHårMedKlip = CreateAndAddTreatment("Permanent i halvkort hår med klip", 1045m, 180);
            _permanentHalvkortHårUdenKlip = CreateAndAddTreatment("Permanent i halvkort hår uden klip", 600m, 120);
            _permanentLangtHårMedKlip = CreateAndAddTreatment("Permanent i langt hår med klip", 1400m, 225);
            _permanentLangtHårUdenKlip = CreateAndAddTreatment("Permanent i langt hår uden klip", 1150m, 210);
            _staniolStriberIHalvkortHårUdenKlip = CreateAndAddTreatment("Staniol striber i halvkort hår uden klip", 590m, 165);
            _staniolStriberIHalvkortHårMedKlip = CreateAndAddTreatment("Staniol striber i halvkort hår med klip", 1035m, 195);
            _staniolStriberILangtHårUdenKlip = CreateAndAddTreatment("Staniol striber i langt hår uden klip", 725m, 195);
            _staniolStriberILangtHårMedKlip = CreateAndAddTreatment("Staniol striber i langt hår med klip", 500m, 150);
            _balayageUdenKlip = CreateAndAddTreatment("Balayage uden klip", 950m, 195);
            _balayageMedKlip = CreateAndAddTreatment("Balyage med klip", 1300m, 180);
            _hårOpsætningStruktur = CreateAndAddTreatment("Håropsætning Struktur", 250m, 30);
            _hårOpsætningElegance = CreateAndAddTreatment("Håropsætning Elegance", 450m, 60);
            _hårOpsætningKompleks = CreateAndAddTreatment("Håropsætning Kompleks", 800m, 120);
            _luksusKur = CreateAndAddTreatment("Luksuskur til hår", 1100m, 120);

            _db.SaveChanges();
        }

        private Treatment CreateAndAddTreatment(string name, decimal priceValue, int durationMinutes)
        {
            var price = Price.FromDecimal(priceValue);
            var duration = DurationMinutes.FromInt(durationMinutes);
            var treatment = Treatment.Create(name, price, duration);

            _db.Add(treatment);

            return treatment;
        }

        // Gives til BookingCommandHandler i metoden der opretter tidligere bookinger,
        // så commandhandleren tror, at den befinder sig 31 dage tilbage i tiden.
        internal class PastDateTimeProvider : ICurrentDateTimeProvider
        {
            DateTime ICurrentDateTimeProvider.GetCurrentDateTime() => DateTime.Now.AddDays(-31);
        }
    }
}
