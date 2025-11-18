using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Ports.Employees
{
    /// <summary>
    /// Defines operations for creating and deleting employee records asynchronously.
    /// </summary>

    // Linnea
    public interface IEmployeeCommand
    {
        Task CreateEmployeeCommand(CreateEmployeeCommand command);
        Task DeleteEmployeeCommand(DeleteEmployeeCommand command);
    }

    public record CreateEmployeeCommand(string FirstName, string? MiddleName, string LastName, string
        Email, string PhoneNumber, string StreetName, string City, string StreetNumber, int ZipCode, IEnumerable<Guid> TreatmentIds, int? Floor = null);

    public record DeleteEmployeeCommand(Guid Id);
}
