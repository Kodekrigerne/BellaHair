using BellaHair.Domain.Discounts;

namespace BellaHair.Domain.Bookings
{
    public class Booking : EntityBase
    {
        public BookingDiscount? Discount { get; private set; }
        public PrivateCustomer Customer { get; private init; }
        public decimal Total { get; private set; }

#pragma warning disable CS8618
        private Booking() { }
#pragma warning restore CS8618

        //Kald denne metode hver gang noget på bookingen ændrer sig som påvirker prisen
        private void UpdateTotal()
        {
            //TODO: Calculate total based on treatments and products
        }

        public void FindBestDiscount(IDiscountCalculatorService discountCalculatorService)
        {
            Discount = discountCalculatorService.GetBestDiscount(this);
        }
    }
}
