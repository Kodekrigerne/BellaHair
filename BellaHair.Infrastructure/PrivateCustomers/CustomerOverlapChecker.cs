using BellaHair.Domain.PrivateCustomers;
using Microsoft.EntityFrameworkCore;


// Mikkel Dahlmann
namespace BellaHair.Infrastructure.PrivateCustomers
{
    /// <summary>
    /// Provides functionality to check for overlapping customer records based on phone number or email address.
    /// </summary>

    public class CustomerOverlapChecker : ICustomerOverlapChecker
    {
        public BellaHairContext _db;

        public CustomerOverlapChecker(BellaHairContext db)
        {
            _db = db;
        }

        // Tjekker for overlapende kunder baseret på telefonnummer eller email.
        // CustomerID gives med for at undgå at sammenligne med sig selv ved opdatering af en eksisterende kunde.
        // CustomerID er nullable og sættes default til null, da metoden også kan bruges ved oprettelse af nye kunder.
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
