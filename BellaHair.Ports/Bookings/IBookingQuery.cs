using BellaHair.Ports.Discounts;
using BellaHair.Ports.Employees;
using BellaHair.Ports.PrivateCustomers;
using BellaHair.Ports.Treatments;

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
        Task<IEnumerable<BookingDTO>> GetAllWithinPeriodOnEmployee(DateTime startDateTime, DateTime endDateTime, Guid employeeId);
        Task<bool> BookingHasOverlap(BookingIsAvailableQuery query);
    }

    public record BookingWithRelationsDTO(
        DateTime StartDateTime,
        bool IsPaid,
        EmployeeNameWithBookingsDTO Employee,
        PrivateCustomerSimpleDTO Customer,
        TreatmentDTO Treatment,
        IEnumerable<ProductLineDTO> Products,
        DiscountDTO? Discount);

    public record ProductLineDTO(Guid ProductId, string Name, string Description, decimal Price, int Quantity);
    public record DiscountDTO(string Name, decimal Amount, DiscountTypeDTO Type);

    public record BookingDTO(
        Guid Id,
        DateTime StartDateTime,
        DateTime EndDateTime,
        bool IsPaid,
        decimal TotalBase,
        decimal TotalWithDiscount,
        string EmployeeFullName,
        string CustomerFullName,
        string CustomerAddress,
        string CustomerPhone,
        string CustomerEmail,
        string TreatmentName,
        int DurationMinutes,
        DiscountDTO? Discount);

    public record GetWithRelationsQuery(Guid Id);

    public record BookingIsAvailableQuery(DateTime StartDateTime, int DurationMinutes, Guid EmployeeId, Guid CustomerId, Guid? bookingId = null);
}
