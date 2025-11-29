using BellaHair.Ports.Discounts;

namespace BellaHair.Ports.Bookings
{
    //Dennis
    /// <summary>
    /// Exposes methods related to Bookings for the frontend to use
    /// </summary>
    public interface IBookingCommand
    {
        Task CreateBooking(CreateBookingCommand command);
        Task PayAndInvoiceBooking(PayAndInvoiceBookingCommand command);
        Task UpdateBooking(UpdateBookingCommand command);
        Task DeleteBooking(DeleteBookingCommand command);
    }

    public record DiscountData(string Name, decimal Amount, DiscountTypeDTO Type);
    public record PayAndInvoiceBookingCommand(Guid Id, DiscountData? Discount);

    public record UpdateBookingCommand(Guid Id, DateTime StartDateTime, Guid EmployeeId, Guid TreatmentId);

    public record DeleteBookingCommand(Guid Id);

    public record CreateBookingCommand(DateTime StartDateTime, Guid EmployeeId, Guid CustomerId, Guid TreatmentId);
}
