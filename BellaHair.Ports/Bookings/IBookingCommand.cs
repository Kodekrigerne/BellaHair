namespace BellaHair.Ports.Bookings
{
    public interface IBookingCommand
    {
        Task CreateBooking(CreateBookingCommand command);
    }

    public record CreateBookingCommand(DateTime StartDateTime, Guid EmployeeId, Guid CustomerId, Guid TreatmentId);
}
