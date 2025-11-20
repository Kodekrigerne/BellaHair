namespace BellaHair.Presentation.WebUI.Components.Pages.PrivateCustomers

// Mikkel Dahlmann

{
    public record PrivateCustomerViewModel(
        Guid Id,
        string FullName,
        DateTime Birthday,
        string Email,
        string PhoneNumber,
        string FullAddress,
        int Visits);
};