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

namespace BellaHair.Infrastructure.Employees
{
    /// <summary>
    /// Handles queries for retrieving employee information.
    /// </summary>
    /// <remarks>Provides implementations of the IEmployeeQuery interface for accessing employee
    /// data. 
    
    // Linnea
    public class EmployeeQueryHandler : IEmployeeQuery
    {
        private readonly BellaHairContext _db;

        public EmployeeQueryHandler(BellaHairContext db) => _db = db;

        // TODO : Add treatments

        // Denne er til at hente alle employees med få informationer til listevisningen af alle
        async Task<List<EmployeeDTOSimple>> IEmployeeQuery.GetAllEmployeesSimpleAsync()
        {
            return  await _db.Employees
                .AsNoTracking()
                .Select(x => new EmployeeDTOSimple(x.Name.FullName, x.PhoneNumber.Value, x.Email.Value))
                .ToListAsync();
        }

        async Task<EmployeeDTOFull> IEmployeeQuery.GetEmployeeAsync(GetEmployeeByIdQuery query)
        {
            var employee = await _db.Employees.FindAsync(query.Id) ?? throw new KeyNotFoundException($"Employee with ID {query.Id} not found");

            return new EmployeeDTOFull(employee.Name.FirstName, employee.Name.MiddleName ?? "", employee.Name.LastName, employee.Email.Value, employee.PhoneNumber.Value, employee.Address.StreetName, employee.Address.City, employee.Address.StreetNumber, employee.Address.ZipCode, employee.Address.Floor);
        }
    }
}
