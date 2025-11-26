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
        private readonly IInvoiceChecker _invoiceChecker;

        public InvoiceCommandHandler(IBookingRepository bookingRepository, IInvoiceRepository invoiceRepository, IInvoiceDocumentDataSource invoiceDocumentDataSource, IInvoiceChecker invoiceChecker)
        {
            _bookingRepository = bookingRepository;
            _invoiceRepository = invoiceRepository;
            _invoiceDocumentDataSource = invoiceDocumentDataSource;
            _invoiceChecker = invoiceChecker;
        }

        async Task IInvoiceCommand.CreateInvoiceAsync(CreateInvoiceCommand command)
        {
            if (await _invoiceChecker.HasBeenPaid(command.Id) == false) throw new InvoiceException("Bookingen skal være betalt, før der kan oprettes faktura.");

            if (await _invoiceChecker.HasInvoice(command.Id) == true) throw new InvoiceException("Der er allerede oprettet en faktura for denne booking.");

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
