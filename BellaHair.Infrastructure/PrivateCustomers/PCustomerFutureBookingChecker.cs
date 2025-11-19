using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain.PrivateCustomers;

namespace BellaHair.Infrastructure.PrivateCustomers
{
    public class PCustomerFutureBookingChecker : IPCustomerFutureBookingChecker
    {
        public async Task<bool> CheckFutureBookingsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
