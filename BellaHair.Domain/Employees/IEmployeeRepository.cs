using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Domain.Employees
{
    public interface IEmployeeRepository
    {
        Task AddAsync(Employee employee);
        void Delete(Employee employee);
        Task<Employee> Get(Guid id);
        Task SaveChangesAsync();
    }
}
