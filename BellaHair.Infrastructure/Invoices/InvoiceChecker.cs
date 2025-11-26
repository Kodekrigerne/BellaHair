using BellaHair.Domain.Invoices;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.Invoices
{
    public class InvoiceChecker : IInvoiceChecker
    {
        private readonly BellaHairContext _db;

        public InvoiceChecker(BellaHairContext db)
        {
            _db = db;
        }

        async Task<bool> IInvoiceChecker.HasBeenPaid(Guid bookingId)
        {
            return await _db.Invoices
                .AnyAsync(i => i.BookingId == bookingId && i.Booking.IsPaid);
        }

        async Task<bool> IInvoiceChecker.HasInvoice(Guid bookingId)
        {
            return await _db.Invoices
                .AnyAsync(i => i.BookingId == bookingId);
        }
    }
}
