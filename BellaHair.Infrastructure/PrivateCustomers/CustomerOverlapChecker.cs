using BellaHair.Domain.PrivateCustomers;
using Microsoft.EntityFrameworkCore;

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
            if (await _db.PrivateCustomers
                .AnyAsync(c => c.PhoneNumber.Value == phoneNumber))
                throw new PrivateCustomerException("Der findes allerede en kunde med det angivne telefonnummer.");

            if (await _db.PrivateCustomers
                .AnyAsync(c => c.Email.Value == email))
                throw new PrivateCustomerException("Der findes allerede en kunde med den angivne email.");
        }
    }
}
