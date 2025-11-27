using BellaHair.Domain.Invoices;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.Invoices
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly BellaHairContext _db;

        public InvoiceRepository(BellaHairContext db) => _db = db;

        async Task IInvoiceRepository.AddAsync(Invoice invoice)
        {
            await _db.Invoices.AddAsync(invoice);
        }

        async Task<Invoice> IInvoiceRepository.GetAsync(int id)
        {
            var invoice = await _db.Invoices
                .FirstOrDefaultAsync(p => p.Id == id)
                    ?? throw new KeyNotFoundException($"No invoice exists with ID {id}");

            return invoice;
        }

        async Task<Invoice> IInvoiceRepository.GetInvoiceByBookingIdAsync(Guid bookingId)
        {
            return await _db.Invoices
                .FirstOrDefaultAsync(i => i.BookingId == bookingId)
                    ?? throw new KeyNotFoundException($"No invoice exists for booking ID {bookingId}");
        }

        async Task IInvoiceRepository.SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
