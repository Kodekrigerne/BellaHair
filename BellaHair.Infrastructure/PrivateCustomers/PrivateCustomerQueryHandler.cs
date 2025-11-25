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
        private readonly ICustomerVisitsService _customerVisitsService;

        public PrivateCustomerQueryHandler(BellaHairContext db, ICurrentDateTimeProvider currentDateTimeProvider, ICustomerVisitsService customerVisitsService)
        {
            _db = db;
            _currentDateTimeProvider = currentDateTimeProvider;
            _customerVisitsService = customerVisitsService;
        }

        async Task<PrivateCustomerDTO> IPrivateCustomerQuery.GetPrivateCustomerAsync(GetPrivateCustomerQuery query)
        {
            var visits = await _customerVisitsService.GetCustomerVisitsAsync(query.Id);

            return await _db.PrivateCustomers
                .AsNoTracking()
                .Where(c => c.Id == query.Id)
                .Select(c => new PrivateCustomerDTO(
                        c.Id,
                        c.Name.FirstName,
                        c.Name.MiddleName,
                        c.Name.LastName,
                        c.Name.FullName,
                        c.Address.StreetName,
                        c.Address.City,
                        c.Address.StreetNumber,
                        c.Address.ZipCode,
                        c.Address.Floor,
                        c.Address.FullAddress,
                        c.PhoneNumber.Value,
                        c.Email.Value,
                        c.Birthday,
                        visits))
                .FirstOrDefaultAsync() ?? throw new KeyNotFoundException($"Customer {query.Id} not found.");
        }

        async Task<List<PrivateCustomerDTO>> IPrivateCustomerQuery.GetPrivateCustomersAsync()
        {
            var customers = await _db.PrivateCustomers.AsNoTracking()
                .ToListAsync();

            List<PrivateCustomerDTO> pclist = new List<PrivateCustomerDTO>();

            foreach (var x in customers)
            {
                var visits = await _customerVisitsService.GetCustomerVisitsAsync(x.Id);

                pclist.Add(new PrivateCustomerDTO(
                        x.Id,
                        x.Name.FirstName,
                        x.Name.MiddleName,
                        x.Name.LastName,
                        x.Name.FullName,
                        x.Address.StreetName,
                        x.Address.City,
                        x.Address.StreetNumber,
                        x.Address.ZipCode,
                        x.Address.Floor,
                        x.Address.FullAddress,
                        x.PhoneNumber.Value,
                        x.Email.Value,
                        x.Birthday,
                        visits));
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
