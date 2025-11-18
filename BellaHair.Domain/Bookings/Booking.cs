using BellaHair.Domain.Discounts;
using BellaHair.Domain.Employees;
using BellaHair.Domain.PrivateCustomers;
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

        public bool IsPaid;
        //_total is stored in the database, Total is ignored
        //_total is only set (and saved in the database) after booking is paid, and is therefore nullable
        private decimal? _total;
        public decimal Total => IsPaid ? _total!.Value : CalculateTotal();

#pragma warning disable CS8618
        private Booking() { }
#pragma warning restore CS8618

        private Booking(PrivateCustomer customer, Employee employee, Treatment treatment, DateTime startDateTime, DateTime currentDateTime)
        {
            Customer = customer;
            Employee = employee;
            Treatment = treatment;
            StartDateTime = startDateTime;
            CreatedDateTime = currentDateTime;
            IsPaid = false;
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

        //public void PayBooking()
        //{
        //Null checks (alt skal .Includes)
        //Set Snapshots
        //Set _total
        //Set IsPaid
        //}

        //Denne metode kaldes hvis Total efterspørges på en ikke-betalt booking
        //TODO: Repo og query metode til booking GetAsync skal .Include Treatment, Employee, Customer
        private decimal CalculateTotal()
        {
            return Treatment?.Price.Value ?? throw new BookingException($"Booking must be loaded with all relations included {Id}");
        }
    }

    public class BookingException(string message) : Exception(message);
}
