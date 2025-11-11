using BellaHair.Domain.Bookings;

namespace BellaHair.Domain.Discounts
{
    //Dennis
    /// <summary>
    /// Base class for different types of discounts.
    /// </summary>
    /// <remarks>
    /// <inheritdoc/>
    /// </remarks>
    public abstract class DiscountBase : EntityBase
    {
        /// <summary>
        /// Calculates the discount for a given booking, based on the specific discount instance and booking.
        /// </summary>
        /// <returns>A BookingDiscount.</returns>
        public abstract BookingDiscount CalculateBookingDiscount(Booking booking);
    }
}
