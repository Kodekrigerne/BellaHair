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
        private readonly IPrivateCustomerRepository _privateCustomerRepository;

        public PCustomerFutureBookingChecker(ICurrentDateTimeProvider currentDateTimeProvider, IPrivateCustomerRepository privateCustomerRepository)
        {
            _currentDateTimeProvider = currentDateTimeProvider;
            _privateCustomerRepository = privateCustomerRepository;
        }

        // Returnerer en bool, der indikerer om kunden med det givne id har bookinger, der ligger i fremtiden
        async Task<bool> IPCustomerFutureBookingChecker.CheckFutureBookings(Guid id)
        {
            var privateCustomer = await _privateCustomerRepository.GetAsync(id);

            if (privateCustomer.Bookings.Any(b => b.StartDateTime > _currentDateTimeProvider.GetCurrentDateTime())) return true;
            
            return false;
        }
    }
}
