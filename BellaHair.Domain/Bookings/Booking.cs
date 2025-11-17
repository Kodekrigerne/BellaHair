using BellaHair.Domain.Discounts;
using BellaHair.Domain.Employees;
using BellaHair.Domain.PrivateCustomers;
using BellaHair.Domain.Treatments;

namespace BellaHair.Domain.Bookings
{
    public class Booking : EntityBase
    {
        public PrivateCustomer? Customer { get; private set; } //Vi kan overveje om vi vil have navigationspropertien når vi har snapshottet
        public CustomerSnapshot CustomerSnapshot { get; private set; }

        public Employee? Employee { get; private set; } //Vi kan overveje om vi vil have navigationspropertien når vi har snapshottet
        public EmployeeSnapshot EmployeeSnapshot { get; private set; }

        public Treatment? Treatment { get; private set; } //Vi kan overveje om vi vil have navigationspropertien når vi har snapshottet
        public TreatmentSnapshot TreatmentSnapshot { get; private set; }

        public BookingDiscount? Discount { get; private set; }
        public DateTime CreatedDateTime { get; private init; }
        public DateTime StartDateTime { get; private set; }

        //TODO: Vælg en strategi
        // 1a. Hvis alle priser er i value objekter: Total => { udregn }
        // 1b. Hvis nogen er navigations properties: .Include navigations properties i query når du skal bruge Total
        // 2.  GetTotal(IBookingTotalCalculator _) metode
        // 3.  Udregn og set hver gang ordren opdateres
        public decimal Total { get; private set; }

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
        }

        public static Booking Create(
            PrivateCustomer customer,
            Employee employee,
            Treatment treatment,
            DateTime startDateTime,
            ICurrentDateTimeProvider currentDateTimeProvider,
            IBookingOverlapChecker bookingOverlapChecker)
        {
            var currentDateTime = currentDateTimeProvider.GetCurrentDateTime();

            if (startDateTime < currentDateTime)
                throw new BookingException($"Cannot create past bookings {startDateTime}.");

            //TODO: Fjern kommentar når treatments er implementeret på medarbejdere
            //TODO: Flyt til BookingCommandHandler.CreateBooking
            //if (!employee.Treatments.Any(t => t.Id == treatment.Id))
            //    throw new BookingException($"Employee {employee.Name.FullName} does not offer treatment {treatment.Name}.");

            return new(customer, employee, treatment, startDateTime, currentDateTime);
        }

        public void FindBestDiscount(IDiscountCalculatorService discountCalculatorService)
        {
            Discount = discountCalculatorService.GetBestDiscount(this);
        }
    }

    public class BookingException(string message) : Exception(message);
}
