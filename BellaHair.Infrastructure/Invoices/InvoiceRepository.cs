using BellaHair.Domain.Invoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Infrastructure.Invoices
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly BellaHairContext _db;

        public InvoiceRepository(BellaHairContext db) => _db = db;

        async Task IInvoiceRepository.AddAsync(Invoice invoice)
        {
            await _db.Invoices
        }

        Task<Invoice> IInvoiceRepository.GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        Task IInvoiceRepository.SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
