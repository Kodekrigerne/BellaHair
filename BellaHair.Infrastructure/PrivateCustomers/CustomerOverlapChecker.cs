using BellaHair.Domain;
using BellaHair.Domain.PrivateCustomers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Infrastructure.PrivateCustomers
{
    public class CustomerOverlapChecker : ICustomerOverlapChecker
    {
        public BellaHairContext _db;

        public CustomerOverlapChecker(BellaHairContext db)
        {
            _db = db;
        }

        async Task ICustomerOverlapChecker.OverlapsWithCustomer(string phoneNumber, string email)
        {
            await _db.PrivateCustomers
                .AnyAsync(p = p.)


            //if (await _db.PrivateCustomers.AnyAsync(p = p.PhoneNumber == phoneNumber)
            //{
            //    throw new PrivateCustomerException("Der findes allerede en kunde med det angivne telefonnummer.");
            //}
        }
    }
}
