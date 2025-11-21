using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Domain.Employees
{
    /// <summary>
    /// Defines a method for determining whether an employee has any bookings scheduled in the future.
    /// </summary>
    public interface IEmployeeFutureBookingsChecker
    {
        Task<bool> EmployeeHasFutureBookings(Guid id);
    }
}
