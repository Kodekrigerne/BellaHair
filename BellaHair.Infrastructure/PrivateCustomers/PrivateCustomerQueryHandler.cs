using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain;
using BellaHair.Ports.PrivateCustomers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
            return await _db.PrivateCustomers
                .AsNoTracking()
                .Select(x => new PrivateCustomerDTO(
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
                    x.Visits))
                .ToListAsync();
        }

        async Task<bool> IPrivateCustomerQuery.PCFutureBookingsCheck(Guid id)
        {
                return await _db.PrivateCustomers
                    .Where(p => p.Id == id)
                    .AnyAsync(p => p.Bookings.Any(b => b.StartDateTime > _currentDateTimeProvider.GetCurrentDateTime()));
        }
    }
}
