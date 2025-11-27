namespace BellaHair.Ports.Bookings
{
    //Dennis
    /// <summary>
    /// Exposes queries for the Booking entity.
    /// </summary>
    public interface IBookingQuery
    {
        Task<BookingWithRelationsDTO> GetWithRelationsAsync(GetWithRelationsQuery query);
        Task<IEnumerable<BookingDTO>> GetAllNewAsync();
        Task<IEnumerable<BookingDTO>> GetAllOldAsync();
        Task<bool> BookingHasOverlap(BookingIsAvailableQuery query);
    }

    public record BookingWithRelationsDTO(
        DateTime StartDateTime,
        bool IsPaid,
        Guid EmployeeId,
        Guid CustomerId,
        Guid TreatmentId,
        DiscountDTO? Discount);

    public record DiscountDTO(string Name, decimal Amount);

    public record BookingDTO(
        Guid Id,
        DateTime StartDateTime,
        DateTime EndDateTime,
        bool IsPaid,
        decimal Total,
        string EmployeeFullName,
        string CustomerFullName,
        string TreatmentName,
        int DurationMinutes,
        DiscountDTO? Discount);

    public record GetWithRelationsQuery(Guid Id);

    public record BookingIsAvailableQuery(DateTime StartDateTime, int DurationMinutes, Guid EmployeeId, Guid CustomerId, Guid? bookingId = null);
}
