using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Ports.Treatments;

namespace BellaHair.Ports.Employees
{

    /// <summary>
    /// Defines methods for querying employee data with varying levels of detail.
    /// </summary>
    
    // Linnea
    public interface IEmployeeQuery
    {
        Task<List<EmployeeDTOSimple>> GetAllEmployeesSimpleAsync();
        Task<EmployeeDTOFull> GetEmployeeAsync(GetEmployeeByIdQuery query);
        Task<List<EmployeeNameDTO>> GetEmployeesByTreatmentIdAsync(GetEmployeesByTreatmentIdQuery query);
    }
}

/// <summary>
/// Represents a simplified data transfer object for an employee, including basic contact information and a list of
/// associated treatment names.
/// </summary>
public record EmployeeDTOSimple(Guid Id, string Name, string PhoneNumber, string Email, List<string> TreatmentNames);

/// <summary>
/// Represents a data transfer object containing all information about an employee, including personal
/// details, contact information, address, and associated treatments.
/// </summary>
public record EmployeeDTOFull(Guid Id, string FirstName, string MiddleName, string LastName, string Email, string PhoneNumber, string StreetName, string City, string StreetNumber, int ZipCode, List<TreatmentDTO> Treatments, int? Floor = null);
public record GetEmployeeByIdQuery(Guid Id);
public record EmployeeNameDTO(string FullName);
public record GetEmployeesByTreatmentIdQuery(Guid TreatmentId);