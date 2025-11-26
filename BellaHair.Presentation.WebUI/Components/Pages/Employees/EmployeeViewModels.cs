using BellaHair.Presentation.WebUI.Components.Pages.Bookings;

namespace BellaHair.Presentation.WebUI.Components.Pages.Employees
{
    // TODO: Add full viewmodel
    public record EmployeeSimpleViewModel(Guid Id, string Name, string Email, string PhoneNumber, List<string> TreatmentNames);

    public record EmployeeNameWithBookingsViewModel(Guid Id, string Name, List<BookingTimesOnlyViewModel> Bookings);

    public record EmployeeNameViewModel(Guid Id, string FullName);
}
