using BellaHair.Presentation.WebUI.Components.Pages.Bookings;
using BellaHair.Presentation.WebUI.Components.Pages.Treatments;

namespace BellaHair.Presentation.WebUI.Components.Pages.Employees
{
    // TODO: Add full viewmodel
    public record EmployeeFullViewModel(Guid Id, string Name, string Email, string PhoneNumber, string Address, List<string> TreatmentNames);
    public record EmployeeViewModel(Guid Id, string FirstName, string MiddleName, string LastName, string FullName, string Email, string PhoneNumber, string StreetName, string City, string StreetNumber, int ZipCode, string FullAddress, List<EmployeeTreatmentViewModel> Treatments, int? Floor = null);

    public record EmployeeNameWithBookingsViewModel(Guid Id, string Name, List<BookingTimesOnlyViewModel> Bookings);

    public record EmployeeNameViewModel(Guid Id, string FullName);
}
