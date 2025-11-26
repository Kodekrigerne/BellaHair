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
        public DateTime EndDateTime { get; private set; }
        public DateTime? PaidDateTime { get; private set; }

        public bool IsPaid { get; private set; }
        //_total is stored in the database, Total is ignored
        //_total is only set (and saved in the database) after booking is paid, and is therefore nullable
        private decimal? _total;
        public decimal Total => IsPaid ? _total!.Value : CalculateTotal();

        private Booking() { }

        private Booking(PrivateCustomer customer, Employee employee, Treatment treatment, DateTime startDateTime, DateTime currentDateTime)
        {
            Customer = customer;
            Employee = employee;
            Treatment = treatment;
            StartDateTime = startDateTime;
            CreatedDateTime = currentDateTime;
            IsPaid = false;
            UpdateEndDateTime();
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
                throw new BookingException($"Kan ikke oprette booking med fortidig startdato og tidspunkt {startDateTime}.");

            ValidateEmployeeTreatment(employee, treatment);

            return new(customer, employee, treatment, startDateTime, currentDateTime);
        }

        public void PayBooking(ICurrentDateTimeProvider currentDateTimeProvider)
        {
            if (Employee == null || Customer == null || Treatment == null)
                throw new InvalidOperationException("all booking relations must be populated when calling PayBooking.");

            if (IsPaid == true) throw new BookingException("Kan ikke betale en booking som allerede er betalt.");

            EmployeeSnapshot = EmployeeSnapshot.FromEmployee(Employee);
            CustomerSnapshot = CustomerSnapshot.FromCustomer(Customer);
            TreatmentSnapshot = TreatmentSnapshot.FromTreatment(Treatment);
            _total = CalculateTotal();
            IsPaid = true;
            PaidDateTime = currentDateTimeProvider.GetCurrentDateTime();
        }

        //Denne metode kaldes hvis Total efterspørges på en ikke-betalt booking
        private decimal CalculateTotal()
        {
            if (IsPaid) throw new InvalidOperationException("Do not use this method to calculate total after booking has been paid.");
            if (Treatment == null) throw new InvalidOperationException($"Booking must be loaded with all relations included {Id}");

            return Treatment.Price.Value;
        }

        //Kald altid denne metode når bookingen ændres
        private void UpdateEndDateTime()
        {
            if (IsPaid) throw new InvalidOperationException("Cannot update EndDateTime on a paid booking.");
            if (Treatment == null) throw new InvalidOperationException("Cannot update EndDateTime without Treatments loaded.");

            EndDateTime = StartDateTime.AddMinutes(Treatment.DurationMinutes.Value);
        }

        public void SetDiscount(BookingDiscount discount, ICurrentDateTimeProvider currentDateTimeProvider)
        {
            var now = currentDateTimeProvider.GetCurrentDateTime();

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

        public void Update(DateTime startDateTime, Employee employee, Treatment treatment, ICurrentDateTimeProvider currentDateTimeProvider)
        {
            var now = currentDateTimeProvider.GetCurrentDateTime();

            if (IsPaid) throw new BookingException("Kan ikke opdatere en betalt booking.");
            if (StartDateTime < now) throw new BookingException("Kan ikke opdatere en booking som allerede er startet.");
            if (startDateTime < now) throw new BookingException("Kan ikke opdatere en booking med en starttid i fortiden");

            ValidateEmployeeTreatment(employee, treatment);

            StartDateTime = startDateTime;
            Employee = employee;
            Treatment = treatment;

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

    public class BookingException(string message) : Exception(message);
}
