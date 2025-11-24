using BellaHair.Presentation.WebUI.Components.Pages.Bookings;

namespace BellaHair.Presentation.WebUI.Components.Pages.Employees
{
    // TODO: Add full viewmodel
    public record EmployeeFullViewModel(Guid Id, string Name, string Email, string PhoneNumber, string Address, List<string> TreatmentNames);

    public record EmployeeNameWithBookingsViewModel(Guid Id, string Name, List<BookingTimesOnlyViewModel> Bookings);
}
