using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Domain.Employees
{
    /// <summary>
    /// Defines a contract for managing employee entities in a data store.
    /// </summary>
    
    // Linnea
    public interface IEmployeeRepository
    {
        Task AddAsync(Employee employee);
        void Delete(Employee employee);
        Task<Employee> GetAsync(Guid id);
        Task SaveChangesAsync();
    }
}
