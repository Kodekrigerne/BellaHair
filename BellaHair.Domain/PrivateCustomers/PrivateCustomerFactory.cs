using BellaHair.Domain.SharedValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Domain.PrivateCustomers
{
    public class PrivateCustomerFactory
    {
        private readonly IFutureBookingsCheck _futureBookingsCheck;

        public PrivateCustomerFactory(IFutureBookingsCheck futureBookingsCheck)
        {
            _futureBookingsCheck = futureBookingsCheck;
        }
        
        public static PrivateCustomer Create(Name name, Address address, PhoneNumber phoneNumber, Email email,
            DateTime birthday)
        {
            return PrivateCustomer.Create(name, address, phoneNumber, email, birthday);
        }
    }
}
