namespace BellaHair.Ports.Bookings
{
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
