using BellaHair.Domain.Discounts;
using BellaHair.Domain.Employees;
using BellaHair.Domain.PrivateCustomers;
using BellaHair.Domain.Treatments;

namespace BellaHair.Domain.Bookings
{
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

        //TODO: Set IsPaid and _total in Pay method
        public bool IsPaid;
        private decimal? _total;
        public decimal Total => IsPaid ? _total!.Value : CalculateTotal();

#pragma warning disable CS8618
        private Booking() { }
#pragma warning restore CS8618

        private Booking(PrivateCustomer customer, Employee employee, Treatment treatment, DateTime startDateTime, DateTime currentDateTime)
        {
            Customer = customer;
            CustomerSnapshot = CustomerSnapshot.FromCustomer(customer);
            Employee = employee;
            EmployeeSnapshot = EmployeeSnapshot.FromEmployee(employee);
            Treatment = treatment;
            TreatmentSnapshot = TreatmentSnapshot.FromTreatment(treatment);
            StartDateTime = startDateTime;
            CreatedDateTime = currentDateTime;
            IsPaid = false;

            _total = CalculateTotal();
        }

        public static Booking Create(
            PrivateCustomer customer,
            Employee employee,
            Treatment treatment,
            DateTime startDateTime,
            ICurrentDateTimeProvider currentDateTimeProvider)
        {
            var currentDateTime = currentDateTimeProvider.GetCurrentDateTime();

            if (startDateTime < currentDateTime)
                throw new BookingException($"Cannot create past bookings {startDateTime}.");

            //TODO: Fjern kommentar når treatments er implementeret på medarbejdere
            //if (employee.Treatments == null || employee.Treatments.Count == 0)
            //    throw new InvalidOperationException("Employees for Booking creation must have Treatments loaded eagerly.");

            //if (!employee.Treatments.Any(t => t.Id == treatment.Id))
            //    throw new BookingException($"Employee {employee.Name.FullName} does not offer treatment {treatment.Name}.");

            return new(customer, employee, treatment, startDateTime, currentDateTime);
        }

        //Denne metode kaldes hvis Total efterspørges på en ikke-betalt booking
        //Dette betyder at Total ikke nødvendig altid er up to date i database hvis ordren ikke er betalt,
        // Men den vil blive opdateret så snart Total efterspørges fra Bookingen.
        //TODO: Repo og query metode til booking GetAsync skal .Include Treatment, Employee, Customer
        private decimal CalculateTotal()
        {
            return Treatment?.Price.Value ?? throw new BookingException("");
        }
    }

    public class BookingException(string message) : Exception(message);
}
