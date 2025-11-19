namespace BellaHair.Ports.Bookings
{
    //Dennis
    /// <summary>
    /// Exposes queries for the Booking entity.
    /// </summary>
    public interface IBookingQuery
    {
        Task<IEnumerable<BookingSimpleDTO>> GetAllNewAsync();
        Task<IEnumerable<BookingSimpleDTO>> GetAllOldAsync();
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
