using BellaHair.Domain.Discounts;

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
        decimal Total,
        string EmployeeFullName,
        string CustomerFullName,
        string TreatmentName,
        int DurationMinutes,
        string? DiscountName,
        decimal? DiscountAmount);

    public record BookingTimesOnlyViewModel(DateTime StartDateTime, DateTime EndDateTime);

    public record BookingDiscountViewModel(string Name, decimal Amount, DiscountType Type);
}
