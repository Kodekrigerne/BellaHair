using BellaHair.Domain.Invoices;
using BellaHair.Ports.Invoices;

namespace BellaHair.Application.Invoices
{
    public class InvoiceCommandHandler : IInvoiceCommand
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IEmailService _emailService;

        public InvoiceCommandHandler(IInvoiceRepository invoiceRepository, IEmailService emailService)
        {
            _invoiceRepository = invoiceRepository;
            _emailService = emailService;
        }

        async Task IInvoiceCommand.EmailInvoiceToCustomerAsync(EmailInvoiceToCustomerCommand command)
        {
            var invoice = await _invoiceRepository.GetInvoiceByBookingIdAsync(command.BookingId);

            var message = "Hermed fremsendes din faktura for din booking hos BellaHair.";

            await _emailService.SendInvoiceAsync(command.Email, invoice.InvoicePdf, message);
        }
    }
}
