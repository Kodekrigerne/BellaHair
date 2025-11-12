using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Ports.PrivateCustomers;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.PrivateCustomers
{
    public class PrivateCustomerQueryHandler : IPrivateCustomerQuery
    {
        private readonly BellaHairContext _db;

        public PrivateCustomerQueryHandler(BellaHairContext db) => _db = db;

        async Task<List<PrivateCustomerDTO>> IPrivateCustomerQuery.GetPrivateCustomers()
        {
            return await _db.PrivateCustomers
                .AsNoTracking()
                .Select(x => new PrivateCustomerDTO(x.Id, x.Name, x.Address, x.PhoneNumber, x.Email, x.Birthday))
                .ToListAsync();
        }
    }
}
