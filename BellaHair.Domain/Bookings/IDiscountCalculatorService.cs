using BellaHair.Domain.Discounts;

namespace BellaHair.Domain.Bookings
{
    //Dennis
    /// <summary>
    /// Domain Service for calculating the best available discount for a given Booking.
    /// </summary>
    //TODO: Kald denne i BookingCommandHandler.CreateBooking GetBestDiscount
    public interface IDiscountCalculatorService
    {
        Task<BookingDiscount?> GetBestDiscount(Booking booking, bool includeBirthdayDiscount);
    }
}
