namespace BellaHair.Presentation.WebUI.Components.Pages.Bookings
{
    public record BookingSimpleViewModel(
        DateTime StartDateTime,
        DateOnly Date,
        TimeOnly StartTime,
        TimeOnly EndTime,
        decimal Total,
        string EmployeeFullName,
        string CustomerFullName,
        string TreatmentName,
        int DurationMinutes,
        string? DiscountName,
        decimal? DiscountAmount);

    public record BookingTimesOnlyViewModel(DateTime StartDateTime, DateTime EndDateTime);

    public record BookingDiscountViewModel(string Name, decimal Amount);
}
