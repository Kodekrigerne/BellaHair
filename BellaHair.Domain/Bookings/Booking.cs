using BellaHair.Domain.Discounts;
using BellaHair.Domain.Employees;
using BellaHair.Domain.Invoices;
using BellaHair.Domain.PrivateCustomers;
using BellaHair.Domain.Products;
using BellaHair.Domain.Treatments;

namespace BellaHair.Domain.Bookings
{
    //Dennis
    /// <summary>
    /// Represents a Booking in the system with attached employee, treatment and customer<br/>
    /// If the booking is marked as paid then value objects are used to store historic data for invoicing reasons<br/>
    /// Relations are nullable as navigation properties may later be deleted, in this case snapshots can be used to retrieve latest data<br/>
    /// Total is calculated when accessed for unpaid bookings, and fetched from Db for paid bookings
    /// </summary>
    public class Booking : EntityBase
    {
        public PrivateCustomer? Customer { get; private set; }
        public CustomerSnapshot? CustomerSnapshot { get; private set; }

        public Employee? Employee { get; private set; }
        public EmployeeSnapshot? EmployeeSnapshot { get; private set; }

        public Treatment? Treatment { get; private set; }
        public TreatmentSnapshot? TreatmentSnapshot { get; private set; }

        public BookingDiscount? Discount { get; private set; }
        public DateTime CreatedDateTime { get; private init; }
        public DateTime StartDateTime { get; private set; }
        public DateTime EndDateTime { get; private set; }
        public DateTime? PaidDateTime { get; private set; }
        public Invoice? Invoice { get; private set; }

        private List<ProductLine> _productLines;
        public IReadOnlyList<ProductLine> ProductLines => !IsPaid ? _productLines.AsReadOnly() : throw new InvalidOperationException("Cannot access product lines on a paid booking");

        private List<ProductLineSnapshot> _productLineSnapshots;
        public IReadOnlyList<ProductLineSnapshot> ProductLineSnapshots => IsPaid ? _productLineSnapshots.AsReadOnly() : throw new InvalidOperationException("Cannot access product line snapshots on an unpaid booking");

        public bool IsPaid { get; private set; }
        //_total is stored in the database, Total is ignored
        //_total is only set (and saved in the database) after booking is paid, and is therefore nullable
        private decimal? _totalBase;
        public decimal TotalBase => IsPaid ? _totalBase!.Value : CalculateTotalBase();

        private decimal? _totalWithDiscount;
        public decimal TotalWithDiscount => IsPaid ? _totalWithDiscount!.Value : CalculateTotalWithDiscount();

#pragma warning disable CS8618
        private Booking() { }
#pragma warning restore CS8618

        private Booking(PrivateCustomer customer, Employee employee, Treatment treatment, DateTime startDateTime, DateTime currentDateTime, IEnumerable<ProductLine> productLines)
        {
            Customer = customer;
            Employee = employee;
            Treatment = treatment;
            StartDateTime = startDateTime;
            CreatedDateTime = currentDateTime;
            IsPaid = false;
            _productLines = [.. productLines];
            UpdateEndDateTime();
        }

        public static Booking Create(
            PrivateCustomer customer,
            Employee employee,
            Treatment treatment,
            DateTime startDateTime,
            ICurrentDateTimeProvider currentDateTimeProvider,
             IEnumerable<ProductLineData> productLineDatas)
        {
            var currentDateTime = currentDateTimeProvider.GetCurrentDateTime();

            if (startDateTime < currentDateTime)
                throw new BookingException($"Kan ikke oprette booking med fortidig startdato og tidspunkt {startDateTime}.");

            ValidateEmployeeTreatment(employee, treatment);

            List<ProductLine> productLines = productLineDatas.Select(pld => ProductLine.Create(pld.Quantity, pld.Product)).ToList();

            return new(customer, employee, treatment, startDateTime, currentDateTime, productLines);
        }

        public void PayBooking(ICurrentDateTimeProvider currentDateTimeProvider)
        {
            if (Employee == null || Customer == null || Treatment == null || _productLines == null)
                throw new InvalidOperationException("all booking relations must be populated when calling PayBooking.");

            if (IsPaid == true) throw new BookingException("Kan ikke betale en booking som allerede er betalt.");

            EmployeeSnapshot = EmployeeSnapshot.FromEmployee(Employee);
            CustomerSnapshot = CustomerSnapshot.FromCustomer(Customer);
            TreatmentSnapshot = TreatmentSnapshot.FromTreatment(Treatment);
            _totalBase = CalculateTotalBase();
            _totalWithDiscount = CalculateTotalWithDiscount();
            IsPaid = true;
            PaidDateTime = currentDateTimeProvider.GetCurrentDateTime();

            //_productLineSnapshots = [.. _productLines.Select(pl => ProductLineSnapshot.FromProductLine(pl))];

            //Vi rydder product lines da vi ikke længere skal bruge dem. Da de er owned af Booking bliver de slettet fra databasen.
            //_productLines = [];
        }

        //Denne metode kaldes hvis Total efterspørges på en ikke-betalt booking
        private decimal CalculateTotalBase()
        {
            if (IsPaid) throw new InvalidOperationException("Do not use this method to calculate total after booking has been paid.");
            if (Treatment == null || _productLines == null) throw new InvalidOperationException($"Booking must be loaded with all relations included {Id}");

            var price = 0m;
            price += Treatment.Price.Value;
            foreach (var productLine in _productLines)
            {
                price += productLine.Quantity.Value * productLine.Product.Price.Value;
            }
            return price;
        }

        private decimal CalculateTotalWithDiscount()
        {
            var discountAmount = Discount?.Amount ?? 0;
            return CalculateTotalBase() - discountAmount;
        }

        //Kald altid denne metode når bookingen ændres
        private void UpdateEndDateTime()
        {
            if (IsPaid) throw new InvalidOperationException("Cannot update EndDateTime on a paid booking.");
            if (Treatment == null) throw new InvalidOperationException("Cannot update EndDateTime without Treatments loaded.");

            EndDateTime = StartDateTime.AddMinutes(Treatment.DurationMinutes.Value);
        }

        public void SetDiscount(BookingDiscount discount)
        {
            if (IsPaid) throw new BookingException("Kan ikke opdatere en betalt booking.");

            Discount = discount;
        }

        public void ValidateDelete(ICurrentDateTimeProvider currentDateTimeProvider)
        {
            var now = currentDateTimeProvider.GetCurrentDateTime();

            if (IsPaid) throw new BookingException("Kan ikke slette en betalt ordre.");
            if (StartDateTime < now && StartDateTime.Date == now.Date)
                throw new BookingException("Afviklede bookinger kan først slettes dagen efter hvis ikke betalt.");
        }

        public void Update(DateTime startDateTime, Employee employee, Treatment treatment, IEnumerable<ProductLineData> productLineDatas, ICurrentDateTimeProvider currentDateTimeProvider)
        {
            var now = currentDateTimeProvider.GetCurrentDateTime();

            if (IsPaid) throw new BookingException("Kan ikke opdatere en betalt booking.");
            if (StartDateTime < now) throw new BookingException("Kan ikke opdatere en booking som allerede er startet.");
            if (startDateTime < now) throw new BookingException("Kan ikke opdatere en booking med en starttid i fortiden");

            ValidateEmployeeTreatment(employee, treatment);

            StartDateTime = startDateTime;
            Employee = employee;
            Treatment = treatment;

            List<ProductLine> productLines = productLineDatas.Select(pld => ProductLine.Create(pld.Quantity, pld.Product)).ToList();

            _productLines = productLines;

            UpdateEndDateTime();
        }

        private static void ValidateEmployeeTreatment(Employee employee, Treatment treatment)
        {
            if (employee.Treatments == null)
                throw new InvalidOperationException("Employees for Booking creation must have Treatments loaded eagerly.");

            if (!employee.Treatments.Any(t => t.Id == treatment.Id))
                throw new BookingException($"Medarbejder {employee.Name.FullName} udbyder ikke behandling {treatment.Name}.");
        }
    }

    public record ProductLineData(Quantity Quantity, Product Product);
    public class BookingException(string message) : Exception(message);
}
