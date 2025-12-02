namespace BellaHair.Ports.PrivateCustomers
{
    // Mikkel Dahlmann

    /// <summary>
    /// Defines a query interface for retrieving private customer records asynchronously.
    /// </summary>

    public interface IPrivateCustomerQuery
    {
        Task<List<PrivateCustomerDTO>> GetPrivateCustomersAsync();
        Task<PrivateCustomerDTO> GetPrivateCustomerAsync(GetPrivateCustomerQuery query);
        Task<bool> PCFutureBookingsCheck(Guid id);
    }

    public record GetPrivateCustomerQuery(Guid Id);

    public record PrivateCustomerDTO(
        Guid Id,
        string FirstName,
        string? MiddleName,
        string LastName,
        string FullName,
        string StreetName,
        string City,
        string StreetNumber,
        int ZipCode,
        int? Floor,
        string FullAddress,
        string PhoneNumber,
        string Email,
        DateTime Birthday,
        int Visits);

    public record PrivateCustomerSimpleDTO(
        Guid Id,
        string FullName,
        DateTime Birthday,
        string Email,
        string PhoneNumber,
        string FullAddress,
        int Visits);
}
