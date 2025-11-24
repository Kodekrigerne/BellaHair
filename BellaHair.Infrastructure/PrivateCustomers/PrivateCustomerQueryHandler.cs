using BellaHair.Domain;
using BellaHair.Ports.PrivateCustomers;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.PrivateCustomers
{
    // Mikkel Dahlmann

    /// <summary>
    /// Handles queries for retrieving private customer information from the database.
    /// </summary>

    public class PrivateCustomerQueryHandler : IPrivateCustomerQuery
    {
        private readonly BellaHairContext _db;
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;

        public PrivateCustomerQueryHandler(BellaHairContext db, ICurrentDateTimeProvider currentDateTimeProvider)
        {
            _db = db;
            _currentDateTimeProvider = currentDateTimeProvider;
        }

        async Task<List<PrivateCustomerDTO>> IPrivateCustomerQuery.GetPrivateCustomersAsync()
        {
            var customers = await _db.PrivateCustomers
                .Include(p => p.Bookings)
                .ToListAsync();

            List<PrivateCustomerDTO> pclist = new List<PrivateCustomerDTO>();

            foreach (var customer in customers)
            {
                pclist.Add(new PrivateCustomerDTO(
                        customer.Id,
                        customer.Name.FirstName,
                        customer.Name.MiddleName,
                        customer.Name.LastName,
                        customer.Name.FullName,
                        customer.Address.StreetName,
                        customer.Address.City,
                        customer.Address.StreetNumber,
                        customer.Address.ZipCode,
                        customer.Address.Floor,
                        customer.Address.FullAddress,
                        customer.PhoneNumber.Value,
                        customer.Email.Value,
                        customer.Birthday,
                        customer.Visits));
            }

            return pclist;
        }

        // Checker om der findes nogen bookings for kunden, der ligger i fremtiden.
        async Task<bool> IPrivateCustomerQuery.PCFutureBookingsCheck(Guid id)
        {
            return await _db.PrivateCustomers
                .Where(p => p.Id == id)
                .AnyAsync(p => p.Bookings.Any(b => b.StartDateTime > _currentDateTimeProvider.GetCurrentDateTime()));
        }
    }
}
