using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain;
using BellaHair.Domain.PrivateCustomers;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.PrivateCustomers
{
    // Mikkel Dahlmann

    /// <summary>
    /// Provides functionality to check whether a private customer has any future bookings.
    /// </summary>

    public class PCustomerFutureBookingChecker : IPCustomerFutureBookingChecker
    {
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;

        public PCustomerFutureBookingChecker(ICurrentDateTimeProvider currentDateTimeProvider)
        {
            _currentDateTimeProvider = currentDateTimeProvider;
        }

        bool IPCustomerFutureBookingChecker.CheckFutureBookings(PrivateCustomer privateCustomer)
        {
            if (privateCustomer.Bookings.Any(b => b.StartDateTime > _currentDateTimeProvider.GetCurrentDateTime())) return true;
            
            return false;
        }
    }
}
