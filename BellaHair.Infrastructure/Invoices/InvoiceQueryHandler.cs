using BellaHair.Ports.Invoices;
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
        private readonly IJSRuntime _jsRuntime;
        private readonly InvoiceDocumentDataSource _invoiceDocumentDataSource;
        public InvoiceQueryHandler(IJSRuntime jsRuntime, InvoiceDocumentDataSource invoiceDocumentDataSource)
        {
            _jsRuntime = jsRuntime;
            _invoiceDocumentDataSource = invoiceDocumentDataSource;
        }

        public async Task CreateAndPrintInvoice(Guid bookingId)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var model = await _invoiceDocumentDataSource.GetInvoiceDetailsAsync(bookingId);
            var document = new InvoiceDocument(model);

            byte[] pdfBytes = document.GeneratePdf();

            await _jsRuntime.InvokeVoidAsync("openPdfInNewTab", Convert.ToBase64String(pdfBytes));
        }
    }
}