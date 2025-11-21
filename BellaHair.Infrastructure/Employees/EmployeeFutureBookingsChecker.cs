using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain;
using BellaHair.Domain.Employees;
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
        /// Determines whether the specified employee has any bookings scheduled for a future date and time.
        /// </summary>
        /// <param name="id">The unique identifier of the employee to check for future bookings.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if the
        /// employee has at least one future booking; otherwise, <see langword="false"/>.</returns>
         async Task<bool> IEmployeeFutureBookingsChecker.CheckFutureBookings(Guid id)
        {
            return (await _db.Employees.Include(e => e.Bookings)
                .FirstAsync(p => p.Id == id))
                .Bookings.Any(b => b.StartDateTime > _currentDateTimeProvider.GetCurrentDateTime());
        }
    }
}
