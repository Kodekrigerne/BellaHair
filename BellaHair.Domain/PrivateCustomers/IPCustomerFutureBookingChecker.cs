using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Domain.PrivateCustomers
{
    // Mikkel Dahlmann

    /// <summary>
    /// Defines a contract for checking whether a customer has any future bookings.
    /// </summary>
    
    public interface IPCustomerFutureBookingChecker
    {
        bool CheckFutureBookingsAsync(PrivateCustomer privateCustomer);
    }
}
