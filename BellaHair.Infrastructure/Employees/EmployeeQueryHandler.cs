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
    /// Handles queries for retrieving employee information.
    /// </summary>
    /// <remarks>Provides implementations of the IEmployeeQuery interface for accessing employee
    /// data. 
    
    // Linnea
    public class EmployeeQueryHandler : IEmployeeQuery
    {
        private readonly BellaHairContext _db;

        public EmployeeQueryHandler(BellaHairContext db) => _db = db;

        // Denne er til at hente alle employees med få informationer til listevisningen af alle så vi har et simplificeret overblik
        async Task<List<EmployeeDTOSimple>> IEmployeeQuery.GetAllEmployeesSimpleAsync()
        {
            var emp =  await _db.Employees
                .Include(e => e.Treatments)
                .AsNoTracking()
                .Select(x => new EmployeeDTOSimple(x.Id, x.Name.FullName, x.PhoneNumber.Value, x.Email.Value, x.Treatments.Select(x => x.Name).ToList()))
                .ToListAsync();

            foreach (var employee in emp)
            {
                Console.WriteLine(string.Join(", ", employee.TreatmentNames));
            }
            return emp;
        }

        async Task<EmployeeDTOFull> IEmployeeQuery.GetEmployeeAsync(GetEmployeeByIdQuery query)
        {
            var employee = await _db.Employees.FirstOrDefaultAsync(e => e.Id == query.Id) ?? throw new KeyNotFoundException($"Employee with ID {query.Id} not found");

            List<TreatmentDTO> treatments = [];

            foreach (var treatment in employee.Treatments)
            {
                var tre = await _db.Treatments.FindAsync(treatment.Id);
                treatments.Add(new TreatmentDTO(treatment.Id, treatment.Name, treatment.Price.Value, treatment.DurationMinutes.Value));
            }

            return new EmployeeDTOFull(employee.Id, employee.Name.FirstName, employee.Name.MiddleName ?? "", employee.Name.LastName, employee.Email.Value, employee.PhoneNumber.Value, employee.Address.StreetName, employee.Address.City, employee.Address.StreetNumber, employee.Address.ZipCode, treatments, employee.Address.Floor);
        }
    }
}   
