namespace BellaHair.Ports.Bookings
{
    //Dennis
    /// <summary>
    /// Exposes queries for the Booking entity.
    /// </summary>
    public interface IBookingQueryHandler
    {
        Task<IEnumerable<BookingSimpleDTO>> GetAllAsync();
    }

    public record DiscountDTO(string Name, decimal DiscountAmount);

    public record BookingSimpleDTO(
        DateTime StartDateTime,
        decimal Total,
        string EmployeeFullName,
        string CustomerFullName,
        string TreatmentName,
        int DurationMinutes,
        DiscountDTO? Discount);
}
