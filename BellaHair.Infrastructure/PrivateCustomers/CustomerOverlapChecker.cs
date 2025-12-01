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

        async Task ICustomerOverlapChecker.OverlapsWithCustomer(string phoneNumber, string email, Guid? customerId = null)
        {
            if (await _db.PrivateCustomers
                .Where(p => p.Id != customerId)
                .AnyAsync(c => c.PhoneNumber.Value == phoneNumber))
                throw new PrivateCustomerException("Der findes allerede en kunde med det angivne telefonnummer.");

            if (await _db.PrivateCustomers
                .Where(p => p.Id != customerId)
                .AnyAsync(c => c.Email.Value == email))
                throw new PrivateCustomerException("Der findes allerede en kunde med den angivne email.");
        }
    }
}
