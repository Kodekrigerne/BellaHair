using BellaHair.Ports.Discounts;

namespace BellaHair.Presentation.WebUI.Components.Pages.Bookings
{
    public record BookingViewModel(
        Guid Id,
        DateTime StartDateTime,
        DateTime EndDateTime,
        DateOnly Date,
        TimeOnly StartTime,
        TimeOnly EndTime,
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
        string? DiscountName,
        decimal? DiscountAmount);

    public record BookingTimesOnlyViewModel(Guid Id, DateTime StartDateTime, DateTime EndDateTime);

    public record BookingDiscountViewModel(string Name, decimal Amount, DiscountTypeDTO Type);
}
