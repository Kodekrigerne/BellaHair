using BellaHair.Domain.Invoices;
using BellaHair.Ports.Invoices;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

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
        private readonly InvoiceDocumentDataSource _invoiceDocumentDataSource;

        public InvoiceQueryHandler(BellaHairContext db, IJSRuntime jsRuntime, InvoiceDocumentDataSource invoiceDocumentDataSource)
        {
            _db = db;
            _jsRuntime = jsRuntime;
            _invoiceDocumentDataSource = invoiceDocumentDataSource;
        }

        async Task IInvoiceQuery.GetInvoiceByBookingId(Guid bookingId)
        {
            var invoice = await _db.Invoices
            byte[] pdfBytes = document.GeneratePdf();

            await _jsRuntime.InvokeVoidAsync("openPdfInNewTab", Convert.ToBase64String(pdfBytes));
        }

        async Task<int> IInvoiceQuery.GetNextInvoiceIdAsync()
        {
            return await _db.Invoices.MaxAsync(i => (int?)i.Id) + 1 ?? 1;
        }
    }
}