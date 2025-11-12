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
    public class EmployeeQueryHandler : IEmployeeQuery
    {
        private readonly BellaHairContext _db;

        public EmployeeQueryHandler(BellaHairContext db) => _db = db;

        // TODO : Add treatments

        Task<List<EmployeeDTOSimple>> IEmployeeQuery.GetAllEmployeesSimple()
        {
            return  _db.Employees
                .AsNoTracking()
                .OfType<Employee>()
                .Select(x => new EmployeeDTOSimple(x.Name.FullName, x.PhoneNumber.Value, x.Email.Value))
                .ToListAsync();
        }

        async Task<EmployeeDTOFull> IEmployeeQuery.GetEmployee(GetEmployeeByIdQuery query)
        {
            var employee = await _db.Employees.FindAsync(query.Id) ?? throw new KeyNotFoundException($"Employee with ID {query.Id} not found");

            return new EmployeeDTOFull(employee.Name.FirstName, employee.Name.MiddleName ?? "", employee.Name.LastName, employee.Email.Value, employee.PhoneNumber.Value, employee.Address.StreetName, employee.Address.City, employee.Address.StreetNumber, employee.Address.ZipCode, employee.Address.Floor);
        }
    }
}
