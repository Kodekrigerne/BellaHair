using BellaHair.Domain.Employees;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.Employees

{
    // Linnea

    /// <summary>
    /// Provides methods for managing employee entities in the data store.
    /// </summary>
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly BellaHairContext _db;

        public EmployeeRepository(BellaHairContext db) => _db = db;

        async Task IEmployeeRepository.AddAsync(Employee employee)
        {
            await _db.Employees.AddAsync(employee);
        }

        void IEmployeeRepository.Delete(Employee employee)
        {
            _db.Employees.Remove(employee);
        }

        async Task<Employee> IEmployeeRepository.GetAsync(Guid id)
        {
            var employee = await _db.Employees.FindAsync(id)
                ?? throw new KeyNotFoundException($"No employee exists with ID {id}");

            return employee;
        }

        async Task<Employee> IEmployeeRepository.GetWithTreatmentsAsync(Guid id)
        {
            var employee = await _db.Employees.Include(e => e.Treatments).SingleOrDefaultAsync(e => e.Id == id)
                ?? throw new KeyNotFoundException($"No employee exists with ID {id}");

            return employee;
        }

        async Task IEmployeeRepository.SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
