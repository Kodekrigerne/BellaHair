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
        public DateTime CreatedDate { get; private init; }

        //TODO: Vælg en strategi
        // 1a. Hvis alle priser er i value objekter: Total => { udregn }
        // 1b. Hvis nogen er navigations properties: .Include navigations properties i query når du skal bruge Total
        // 2.  GetTotal(IBookingTotalCalculator _) metode
        // 3.  Udregn og set hver gang ordren opdateres
        public decimal Total { get; private set; }

#pragma warning disable CS8618
        private Booking() { }
#pragma warning restore CS8618

        private Booking(PrivateCustomer customer, Employee employee, Treatment treatment, DateTime createdDate)
        {
            Customer = customer;
            CustomerSnapshot = CustomerSnapshot.FromCustomer(customer);
            Employee = employee;
            EmployeeSnapshot = EmployeeSnapshot.FromEmployee(employee);
            Treatment = treatment;
            TreatmentSnapshot = TreatmentSnapshot.FromTreatment(treatment);
            CreatedDate = createdDate; //TODO: Brug currentDateTime til invariant, ikke til createdDate, pass createdDate (invariant -1 dag)

            //TODO: Invariant på at employee har Treatment
        }

        public static Booking Create(PrivateCustomer customer, Employee employee, Treatment treatment, ICurrentDateTimeProvider currentDateTimeProvider)
            => new(customer, employee, treatment, currentDateTimeProvider.GetCurrentDateTime());

        public void FindBestDiscount(IDiscountCalculatorService discountCalculatorService)
        {
            Discount = discountCalculatorService.GetBestDiscount(this);
        }
    }

    public class BookingException(string message) : Exception(message);
}
