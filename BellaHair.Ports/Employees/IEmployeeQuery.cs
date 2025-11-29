using BellaHair.Ports.Treatments;

namespace BellaHair.Ports.Employees
{

    /// <summary>
    /// Defines methods for querying employee data with varying levels of detail.
    /// </summary>

    // Linnea
    public interface IEmployeeQuery
    {
        Task<List<EmployeeDTOFull>> GetAllEmployeesAsync();
        Task<EmployeeNameWithBookingsDTO> GetWithFutureBookingsAsync(GetWithFutureBookingsQuery query);
        Task<List<EmployeeNameWithBookingsDTO>> GetHasTreatmentAndWithFutureBookingsAsync(Guid treatmentId);
        Task<EmployeeDTOFull> GetEmployeeAsync(GetEmployeeByIdQuery query);
        Task<List<EmployeeNameDTO>> GetEmployeesByTreatmentIdAsync(GetEmployeesByTreatmentIdQuery query);
        Task<bool> EmployeeHasFutureBookings(Guid id);
    }

    /// <summary>
    /// Represents a simplified data transfer object for an employee, including basic contact information and a list of
    /// associated treatment names.
    /// </summary>
    public record EmployeeDTO(Guid Id, string Name, string PhoneNumber, string Email, string Address, List<string> TreatmentNames);

    /// <summary>
    /// Represents a data transfer object containing all information about an employee, including personal
    /// details, contact information, address, and associated treatments.
    /// </summary>
    public record EmployeeDTOFull(Guid Id, string FirstName, string MiddleName, string LastName, string FullName, string Email, string PhoneNumber, string StreetName, string City, string StreetNumber, int ZipCode, string FullAddress, List<TreatmentDTO> Treatments, int? Floor = null);

    public record GetEmployeeByIdQuery(Guid Id);

    public record GetWithFutureBookingsQuery(Guid Id);

    public record EmployeeNameDTO(string FullName);

    public record GetEmployeesByTreatmentIdQuery(Guid TreatmentId);

    public record EmployeeNameWithBookingsDTO(Guid Id, string Name, List<BookingTimesOnlyDTO> Bookings);

    public record BookingTimesOnlyDTO(Guid Id, DateTime StartDateTime, DateTime EndDateTime, int DurationMinutes);
}
