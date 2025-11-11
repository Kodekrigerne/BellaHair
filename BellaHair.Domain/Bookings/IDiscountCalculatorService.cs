using BellaHair.Domain.Discounts;

namespace BellaHair.Domain.Bookings
{
    //Dennis
    /// <summary>
    /// Domain Service for calculating the best available discount for a given Booking.
    /// </summary>
    public interface IDiscountCalculatorService
    {
        BookingDiscount? GetBestDiscount(Booking booking);
    }
}
