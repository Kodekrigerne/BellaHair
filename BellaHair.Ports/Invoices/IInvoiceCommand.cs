namespace BellaHair.Ports.Invoices
{
    public interface IInvoiceCommand
    {
        Task CreateInvoiceAsync(CreateInvoiceCommand command);
    }

    public record CreateInvoiceCommand(
        Guid Id);
}
