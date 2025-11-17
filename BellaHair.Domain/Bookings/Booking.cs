using BellaHair.Domain.Discounts;
using BellaHair.Domain.PrivateCustomers;

namespace BellaHair.Domain.Bookings
{
    public class Booking : EntityBase
    {
        public BookingDiscount? Discount { get; private set; }
        public PrivateCustomer Customer { get; private init; }

        //TODO: Vælg en strategi
        // 1a. Hvis alle priser er i value objekter: Total => { udregn }
        // 1b. Hvis nogen er navigations properties: .Include navigations properties i query når du skal bruge Total
        // 2.  GetTotal(IBookingTotalCalculator _) metode
        public decimal Total { get; private set; }

#pragma warning disable CS8618
        private Booking() { }
#pragma warning restore CS8618

        public void FindBestDiscount(IDiscountCalculatorService discountCalculatorService)
        {
            Discount = discountCalculatorService.GetBestDiscount(this);
        }
    }
}
