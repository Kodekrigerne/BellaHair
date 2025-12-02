namespace BellaHair.Ports.Invoices
{
    public interface IInvoiceCommand
    {
        Task EmailInvoiceToCustomerAsync(EmailInvoiceToCustomerCommand command);
    }

    public record EmailInvoiceToCustomerCommand(Guid BookingId, string Email);
}
