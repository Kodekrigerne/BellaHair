namespace BellaHair.Domain.Bookings
{
    //TODO: Kald denne i BookingCommandHandler.CreateBooking og UpdateBooking
    public interface IBookingOverlapChecker
    {
        Task<bool> CheckOverlap(Guid employeeId, DateTime startDateTime, int durationMinutes);
    }
}
