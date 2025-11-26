using BellaHair.Domain.Bookings;
using BellaHair.Domain.Invoices;
using BellaHair.Ports.Invoices;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace BellaHair.Application.Invoices
{
    // Mikkel Dahlmann

    /// <summary>
    /// Handles commands related to invoice creation and management.
    /// </summary>

    public class InvoiceCommandHandler : IInvoiceCommand
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IInvoiceDocumentDataSource _invoiceDocumentDataSource;

        public InvoiceCommandHandler(IBookingRepository bookingRepository, IInvoiceRepository invoiceRepository, IInvoiceDocumentDataSource invoiceDocumentDataSource)
        {
            _bookingRepository = bookingRepository;
            _invoiceRepository = invoiceRepository;
            _invoiceDocumentDataSource = invoiceDocumentDataSource;
        }

        async Task IInvoiceCommand.CreateInvoiceAsync(CreateInvoiceCommand command)
        {
            bool exists = await _


            QuestPDF.Settings.License = LicenseType.Community;

            var model = await _invoiceDocumentDataSource.GetInvoiceDetailsAsync(command.Id);
            var document = new InvoiceDocument(model);
            var booking = await _bookingRepository.GetAsync(command.Id);

            byte[] pdfBytes = document.GeneratePdf();

            var invoice = Invoice.Create(model.Id, booking, pdfBytes);

            await _invoiceRepository.AddAsync(invoice);
            await _invoiceRepository.SaveChangesAsync();
        }
    }
}
