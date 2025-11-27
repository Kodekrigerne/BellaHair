namespace BellaHair.Domain.Bookings
{
    //TODO: Kald denne i BookingCommandHandler.CreateBooking og UpdateBooking
    //Dennis
    /// <summary>
    /// Exposes methods for checking whether a given booking overlaps with other bookings for the same employee<br/>
    /// Always load the booking eagerly with employee and treatment<br/>
    /// Include a booking ID when you are editing a booking and there may exist an overlap with the bookings original time.
    /// </summary>
    public interface IBookingOverlapChecker
    {
        Task<bool> OverlapsWithBooking(DateTime startDateTime, int durationMinutes, Guid employeeId, Guid customerId, Guid? bookingId = null);
    }
}
