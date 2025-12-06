using BellaHair.Domain;
using BellaHair.Ports.Employees;
using BellaHair.Ports.Treatments;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.Employees
{
    // Linnea

    /// <summary>
    /// Handles queries for retrieving employee information from the data store.
    /// </summary>
    /// <remarks>This class provides implementations for the IEmployeeQuery interface, enabling retrieval of
    /// employee data in both simplified and detailed forms. It is intended for use in scenarios where employee
    /// information needs to be fetched for display or processing purposes. Instances of this class are typically
    /// created with a BellaHairContext to access the underlying database.</remarks>

    public class EmployeeQueryHandler : IEmployeeQuery
    {
        private readonly BellaHairContext _db;
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;

        public EmployeeQueryHandler(BellaHairContext db, ICurrentDateTimeProvider currentDateTimeProvider)
        {
            _db = db;
            _currentDateTimeProvider = currentDateTimeProvider;
        }
        public async Task<List<EmployeeNameDTO>> GetEmployeesByTreatmentIdAsync(GetEmployeesByTreatmentIdQuery query)
        {
            return await _db.Employees
                .AsNoTracking()
                .Where(e => e.Treatments
                                        .Any(t => t.Id == query.TreatmentId))
                .Select(e => new EmployeeNameDTO(e.Name.FullName))
                .ToListAsync();
        }

        // Henter alle medarbejdere med færre detaljer til kalenderen
        async Task<List<EmployeeDTOSimple>> IEmployeeQuery.GetAllEmployeesSimpleAsync()
        {
            var emp = await _db.Employees
                .AsNoTracking()
                .Select(x => new EmployeeDTOSimple(x.Id, x.Name.FullName))
                .ToListAsync();

            return emp;
        }

        // Henter alle medarbejdere med alle detaljer til overblikket
        async Task<List<EmployeeDTOFull>> IEmployeeQuery.GetAllEmployeesAsync()
        {
            var emp = await _db.Employees
                .AsNoTracking()
                .Select(employee => new EmployeeDTOFull(
                    employee.Id,
                    employee.Name.FirstName,
                    employee.Name.MiddleName ?? "",
                    employee.Name.LastName,
                    employee.Name.FullName,
                    employee.Email.Value,
                    employee.PhoneNumber.Value,
                    employee.Address.StreetName,
                    employee.Address.City,
                    employee.Address.StreetNumber,
                    employee.Address.ZipCode,
                    employee.Address.FullAddress,
                    employee.Treatments.Select(t => new TreatmentDTO(
                        t.Id,
                        t.Name,
                        t.Price.Value,
                        t.DurationMinutes.Value,
                        t.Employees.Count)).ToList(),
                    employee.Address.Floor))
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

            List<TreatmentDTO> treatments = employee.Treatments.Select(e => new TreatmentDTO(
                    e.Id,
                    e.Name,
                    e.Price.Value,
                    e.DurationMinutes.Value,
                    e.Employees.Count))
                .ToList();

            return new EmployeeDTOFull(employee.Id,
                employee.Name.FirstName,
                employee.Name.MiddleName ?? "",
                employee.Name.LastName,
                employee.Name.FullName,
                employee.Email.Value,
                employee.PhoneNumber.Value,
                employee.Address.StreetName,
                employee.Address.City,
                employee.Address.StreetNumber,
                employee.Address.ZipCode,
                employee.Address.FullAddress,
                treatments,
                employee.Address.Floor);
        }

        async Task<EmployeeNameWithBookingsDTO> IEmployeeQuery.GetWithFutureBookingsAsync(GetWithFutureBookingsQuery query)
        {
            var now = _currentDateTimeProvider.GetCurrentDateTime();

            return await _db.Employees
                .AsNoTracking()
                .Where(e => e.Id == query.Id)
                .Select(e => new EmployeeNameWithBookingsDTO(
                    e.Id,
                    e.Name.FullName,
                    e.Bookings
                        .Where(b => b.EndDateTime > now)
                        .Select(b => new BookingTimesOnlyDTO(
                            b.Id,
                            b.StartDateTime,
                            b.EndDateTime))
                .ToList()))
                .FirstOrDefaultAsync() ?? throw new KeyNotFoundException($"Employee {query.Id} not found.");
        }

        async Task<List<EmployeeNameWithBookingsDTO>> IEmployeeQuery.GetHasTreatmentAndWithFutureBookingsAsync(Guid treatmentId)
        {
            var now = _currentDateTimeProvider.GetCurrentDateTime();

            return await _db.Employees
                .AsNoTracking()
                .Where(e => e.Treatments.Any(t => t.Id == treatmentId))
                .Select(e => new EmployeeNameWithBookingsDTO(
                    e.Id,
                    e.Name.FullName,
                    e.Bookings
                        .Where(b => b.EndDateTime > now)
                        .Select(b => new BookingTimesOnlyDTO(
                            b.Id,
                            b.StartDateTime,
                            b.EndDateTime))
                        .ToList()))
                .ToListAsync();
        }

        /// <summary>
        /// Determines whether the specified employee has any bookings scheduled for a future date and time.
        /// </summary>
        /// <param name="id">The unique identifier of the employee to check for future bookings.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if the
        /// employee has at least one future booking; otherwise, <see langword="false"/>.</returns>
        async Task<bool> IEmployeeQuery.EmployeeHasFutureBookings(Guid id)
        {
            return (await _db.Employees.Include(e => e.Bookings.Where(b => b.EndDateTime > _currentDateTimeProvider.GetCurrentDateTime()))
                            .FirstAsync(p => p.Id == id))
                            .Bookings.Any();
        }
    }
}
