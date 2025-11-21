using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Domain.Employees
{
    public interface IEmployeeFutureBookingsChecker
    {
        Task<bool> CheckFutureBookings(Guid id);
    }
}
