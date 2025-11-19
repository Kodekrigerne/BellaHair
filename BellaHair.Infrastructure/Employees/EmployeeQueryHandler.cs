using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain;
using BellaHair.Domain.Employees;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Ports.Employees;
using Microsoft.EntityFrameworkCore;
using BellaHair.Ports.Treatments;

namespace BellaHair.Infrastructure.Employees
{
    
    /// <summary>
    /// Handles queries for retrieving employee information from the data store.
    /// </summary>
    /// <remarks>This class provides implementations for the IEmployeeQuery interface, enabling retrieval of
    /// employee data in both simplified and detailed forms. It is intended for use in scenarios where employee
    /// information needs to be fetched for display or processing purposes. Instances of this class are typically
    /// created with a BellaHairContext to access the underlying database.</remarks>
    
    // Linnea
    public class EmployeeQueryHandler : IEmployeeQuery
    {
        private readonly BellaHairContext _db;

        public EmployeeQueryHandler(BellaHairContext db) => _db = db;

        public async Task<List<EmployeeNameDTO>> GetEmployeesByTreatmentIdAsync(GetEmployeesByTreatmentIdQuery query)
        {
            return await _db.Employees
                .AsNoTracking()
                .Where(e => e.Treatments
                .Any(t => t.Id == query.TreatmentId))
                .Select(e => new EmployeeNameDTO(e.Name.FullName))
                .ToListAsync();
        }

        // Henter alle medarbejdere med færre detaljer til overblikket
        async Task<List<EmployeeDTOSimple>> IEmployeeQuery.GetAllEmployeesSimpleAsync()
        {
            var emp =  await _db.Employees
                .AsNoTracking()
                .Select(x => new EmployeeDTOSimple(x.Id, x.Name.FullName, x.PhoneNumber.Value, x.Email.Value, x.Treatments.Select(x => x.Name).ToList()))
                .ToListAsync();

            return emp;
        }

        /// <summary>
        /// Asynchronously retrieves detailed information about an employee by their unique identifier.
        /// </summary>
        /// <param name="query">An object containing the identifier of the employee to retrieve. The <c>Id</c> property must specify a valid
        /// employee ID.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="EmployeeDTOFull"/>
        /// with the employee's details, including associated treatments.</returns>
        
        async Task<EmployeeDTOFull> IEmployeeQuery.GetEmployeeAsync(GetEmployeeByIdQuery query)
        {
            var employee = await _db.Employees.Include(e => e.Treatments)
                                               .FirstOrDefaultAsync(e => e.Id == query.Id)
                                               ?? throw new KeyNotFoundException($"Employee with ID {query.Id} not found");

            List<TreatmentDTO> treatments = employee.Treatments.Select(e =>
                                                                           new TreatmentDTO(e.Id,
                                                                                        e.Name,
                                                                                        e.Price.Value,
                                                                                        e.DurationMinutes.Value))
                                                               .ToList();

            return new EmployeeDTOFull(employee.Id,
                                       employee.Name.FirstName,
                                       employee.Name.MiddleName ?? "",
                                       employee.Name.LastName,
                                       employee.Email.Value,
                                       employee.PhoneNumber.Value,
                                       employee.Address.StreetName,
                                       employee.Address.City,
                                       employee.Address.StreetNumber,
                                       employee.Address.ZipCode,
                                       treatments,
                                       employee.Address.Floor);
        }
    }
}   
