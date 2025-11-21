using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain;
using BellaHair.Domain.Employees;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.Employees
{
    public class EmployeeFutureBookingsChecker : IEmployeeFutureBookingsChecker
    {
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;
        private readonly BellaHairContext _db;
        public EmployeeFutureBookingsChecker(ICurrentDateTimeProvider currentDateTimeProvider, BellaHairContext db)
        {
            _currentDateTimeProvider = currentDateTimeProvider;
            _db = db;
        }

         async Task<bool> IEmployeeFutureBookingsChecker.CheckFutureBookings(Guid id)
        {
            return (await _db.Employees.Include(e => e.Bookings)
                .FirstAsync(p => p.Id == id))
                .Bookings.Any(b => b.StartDateTime > _currentDateTimeProvider.GetCurrentDateTime());
        }
    }
}
