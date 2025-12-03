using BellaHair.Domain;
using BellaHair.Domain.Bookings;
using BellaHair.Domain.Discounts;
using BellaHair.Domain.Employees;
using BellaHair.Domain.PrivateCustomers;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Domain.Treatments;
using BellaHair.Domain.Treatments.ValueObjects;
using BellaHair.Infrastructure;
using Bogus;
using Bogus.Extensions.Denmark;
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

        private readonly IOptions<OpeningTimesSettings> OpeningTimes;

        // --- 1. Treatment Fields ---
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

        public void AddData()
        {
            AddLoyaltyDiscounts();
            AddTreatment();
            AddEmployees();
            AddPrivateCustomersAndBookingsUsingBogus();
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
                new DateTime(2027, 4, 1, 9, 0, 0),
                new DateTime(2027, 5, 31, 18, 0, 0),
                new List<Guid> {
                    _hårOpsætningElegance.Id,
                    _hårOpsætningKompleks.Id,
                    _ordneBrynVipper.Id,
                    _damefrisureBlæs.Id
                }));

            // Farve Fornyelse
            _db.Add(CampaignDiscount.Create("Farve Fornyelse",
                DiscountPercent.FromDecimal(0.25m), 
                new DateTime(2026, 3, 1, 9, 0, 0),
                new DateTime(2026, 4, 15, 17, 0, 0),
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
                new DateTime(2026, 9, 10, 9, 0, 0),
                new DateTime(2026, 9, 30, 18, 0, 0),
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
                new DateTime(2026, 12, 4, 10, 0, 0), 
                new DateTime(2026, 12, 18, 18, 0, 0), 
                new List<Guid> {
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
            _db.Add(LoyaltyDiscount.Create("Stamkunde Nikkel", 1, DiscountPercent.FromDecimal(0.01m)));
            _db.Add(LoyaltyDiscount.Create("Stamkunde Bronze", 5, DiscountPercent.FromDecimal(0.05m)));
            _db.Add(LoyaltyDiscount.Create("Stamkunde Sølv", 10, DiscountPercent.FromDecimal(0.10m)));
            _db.Add(LoyaltyDiscount.Create("Stamkunde Guld", 15, DiscountPercent.FromDecimal(0.15m)));
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
        }

        private void AddPrivateCustomersAndBookingsUsingBogus()
        {

            var now = _currentDateTimeProvider.GetCurrentDateTime();

            var customers = new List<PrivateCustomer>();

            for (int i = 0; i < 250; i++)
            {

            var danishFaker = new Faker<PrivateCustomer>("nb_NO")
                .CustomInstantiator(f => PrivateCustomer.Create(
                                                                Name.FromStrings(f.Name.FirstName(), f.Name.LastName()),
                                                                Address.Create(f.Address.StreetName().Replace(".", ""), f.Address.City(), f.Random.Int(min: 1, max: 200).ToString(), f.Random.Int(min: 1000, max: 9990)),
                                                                PhoneNumber.FromString(f.Phone.PhoneNumber("########")),
                                                                Email.FromString(f.Internet.Email()),
                                                                f.Date.Between(now.AddYears(-18), now.AddYears(-110)),
                                                                _currentDateTimeProvider
                                                                ));
            var customer = danishFaker.Generate();


                customers.Add(customer);
            _db.Add(customer);
            }

            for (int i = 0; i < 20; i++)
            {
                try
                {

                    Random random = new Random();
                    var employee = _employees[random.Next(1, 6)];
                    var treatment = employee.Treatments[random.Next(1, employee.Treatments.Count)];

                    var bookingFaker = new Faker<Booking>("nb_NO")
                        .CustomInstantiator(f => Booking.Create(
                                                        f.PickRandom<PrivateCustomer>(customers),
                                                        employee,
                                                        treatment,
                                                    new DateTime(
                                                            f.Date.Soon(60, DateTime.Now).Date.Year,
                                                            f.Date.Soon(60, DateTime.Now).Date.Month,
                                                            f.Date.Soon(60, DateTime.Now).Date.Day,
                                                            f.Random.Int(OpeningTimes.Value.OpeningTime.Hour, OpeningTimes.Value.ClosingTime.Hour-treatment.DurationMinutes.Value),
                                                            f.PickRandom(new[] { 0, 15, 30, 45 }), // Use PickRandom with an inline array
                                                            0),
                                                    _currentDateTimeProvider));
                    
                    
                    var booking = bookingFaker.Generate();
                }
                catch (Exception)
                {

                }
            }


        }

        // Bruges da Bookings skal have en ICurrentDateTimeProvider som giver deres CreatedDate som skal være i fortiden i forhold til StartTime.
        internal class PastDateTimeProvider : ICurrentDateTimeProvider
        {
            DateTime ICurrentDateTimeProvider.GetCurrentDateTime() => DateTime.Now.AddDays(-60);
        }
    }

}
