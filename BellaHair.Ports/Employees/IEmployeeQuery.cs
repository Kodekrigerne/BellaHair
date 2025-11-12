using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Ports.Employees
{
    public interface IEmployeeQuery
    {
        Task<List<EmployeeDTOSimple>> GetAllEmployeesSimple();
        Task<EmployeeDTOFull> GetEmployee(GetEmployeeByIdQuery query);
    }
}

public record EmployeeDTOSimple(string Name, string PhoneNumber, string Email);
public record EmployeeDTOFull(string FirstName, string MiddleName, string LastName, string Email, string PhoneNumber, string StreetName, string City, string StreetNumber, int ZipCode, int? Floor = null);
public record GetEmployeeByIdQuery(Guid Id);