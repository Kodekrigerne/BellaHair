using BellaHair.Domain;
using BellaHair.Domain.Employees;
using BellaHair.Domain.Treatments;
using Microsoft.EntityFrameworkCore;

// Linnea

namespace BellaHair.Infrastructure.Employees
{

    /// <summary>
    /// Provides functionality to determine whether an employee has any bookings scheduled for a future date and time.
    /// </summary>

    public class EmployeeFutureBookingsChecker : IEmployeeFutureBookingsChecker
    {
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;
        private readonly BellaHairContext _db;
        public EmployeeFutureBookingsChecker(ICurrentDateTimeProvider currentDateTimeProvider, BellaHairContext db)
        {
            _currentDateTimeProvider = currentDateTimeProvider;
            _db = db;
        }

        /// <summary>
        /// Determines whether the specified employee has any future bookings with an associated treatment.
        /// </summary>
        /// <remarks>A future booking is defined as a booking whose end time, based on the treatment
        /// duration, is after the current date and time. Only bookings with a non-null treatment are
        /// considered.</remarks>
        /// <param name="id">The unique identifier of the employee to check for future bookings.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if the
        /// employee has at least one future booking with a treatment; otherwise, <see langword="false"/>.</returns>
        
        async Task<bool> IEmployeeFutureBookingsChecker.EmployeeHasFutureBookings(Guid id)
        {
            return (await _db.Employees.AsNoTracking().Include(e => e.Bookings.Where(b => b.EndDateTime > _currentDateTimeProvider.GetCurrentDateTime()))
                .FirstAsync(p => p.Id == id))
                .Bookings.Any();
        }


        //return await _db.Bookings
        //        .AsNoTracking()
        //        .Where(b => b.Treatment!.Id == treatmentId)
        //        .AnyAsync(b => b.EndDateTime > currentDateTime);

        async Task<bool> IEmployeeFutureBookingsChecker.EmployeeHasFutureBookingsWithTreatments(Guid employeeId, List<Guid> toBeRemovedTreatmentIds)
        {
            return (await _db.Bookings.AsNoTracking().Where(b => b.Employee.Id == employeeId).AnyAsync(b => b.EndDateTime > _currentDateTimeProvider.GetCurrentDateTime() && b.Treatment != null && toBeRemovedTreatmentIds.Contains(b.Treatment.Id)));
        }
    }
}
