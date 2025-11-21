using BellaHair.Presentation.WebUI.Components.Pages.Bookings;

namespace BellaHair.Presentation.WebUI.Components.Pages.Employees
{
    public record EmployeeSimpleViewModel(Guid Id, string name, string email, string phoneNumber, List<string> treatmentNames);

    public record EmployeeNameWithBookingsViewModel(Guid Id, string Name, List<BookingTimesOnlyViewModel> Bookings);
}
