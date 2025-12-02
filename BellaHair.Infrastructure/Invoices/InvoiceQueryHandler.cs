using BellaHair.Ports.Invoices;
using Microsoft.EntityFrameworkCore;

// Mikkel Dahlmann

namespace BellaHair.Infrastructure.Invoices
{
    /// <summary>
    /// Handles invoice query operations and provides functionality to create and print invoices using JavaScript
    /// interop.
    /// </summary>

    public class InvoiceQueryHandler : IInvoiceQuery
    {
        private readonly BellaHairContext _db;

        public InvoiceQueryHandler(BellaHairContext db)
        {
            _db = db;
        }

        async Task<byte[]> IInvoiceQuery.GetInvoicePdfByBookingIdAsync(GetInvoiceByBookingIdQuery query)
        {
            var invoice = await _db.Invoices
                .FirstOrDefaultAsync(i => i.Booking.Id == query.BookingId)
                ?? throw new InvalidOperationException($"Ingen faktura fundet for booking: {query.BookingId}");

            byte[] pdfBytes = invoice.InvoicePdf;

            return pdfBytes;
        }
    }
}