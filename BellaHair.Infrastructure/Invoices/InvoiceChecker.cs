using BellaHair.Domain.Invoices;
using Microsoft.EntityFrameworkCore;

// Mikkel Dahlmann

namespace BellaHair.Infrastructure.Invoices
{

    /// <summary>
    /// Provides methods for checking invoice and payment status for bookings.
    /// </summary>

    public class InvoiceChecker : IInvoiceChecker
    {
        private readonly BellaHairContext _db;

        public InvoiceChecker(BellaHairContext db)
        {
            _db = db;
        }

        async Task<bool> IInvoiceChecker.HasBeenPaid(Guid bookingId)
        {
            return await _db.Bookings
                .AnyAsync(b => b.Id == bookingId && b.IsPaid);
        }

        async Task<bool> IInvoiceChecker.HasInvoice(Guid bookingId)
        {
            return await _db.Invoices
                .AnyAsync(i => i.BookingId == bookingId);
        }
    }
}
