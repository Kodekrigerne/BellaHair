using BellaHair.Ports.Invoices;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

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
        private readonly IJSRuntime _jsRuntime;

        public InvoiceQueryHandler(BellaHairContext db, IJSRuntime jsRuntime)
        {
            _db = db;
            _jsRuntime = jsRuntime;
        }

        async Task IInvoiceQuery.GetInvoiceByBookingId(Guid bookingId)
        {
            var invoice = await _db.Invoices
                .FirstOrDefaultAsync(i => i.Booking.Id == bookingId)
                ?? throw new InvalidOperationException($"Ingen faktura fundet for booking: {bookingId}");

            byte[] pdfBytes = invoice.InvoicePdf;

            await _jsRuntime.InvokeVoidAsync("openPdfInNewTab", Convert.ToBase64String(pdfBytes));
        }

        async Task<int> IInvoiceQuery.GetNextInvoiceIdAsync()
        {
            return await _db.Invoices.MaxAsync(i => (int?)i.Id) + 1 ?? 1;
        }
    }
}