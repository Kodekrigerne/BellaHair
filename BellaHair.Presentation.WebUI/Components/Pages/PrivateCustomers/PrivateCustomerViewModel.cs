namespace BellaHair.Presentation.WebUI.Components.Pages.PrivateCustomers
{
    public record PrivateCustomerViewModel(Guid Id, string FullName, DateTime Birthday, string Email, string PhoneNumber, string FullAddress, int Visits);
}