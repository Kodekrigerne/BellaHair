namespace BellaHair.Domain.Invoices
{
    public interface IInvoiceChecker
    {
        Task<bool> HasInvoice(Guid bookingId);
        Task<bool> HasBeenPaid(Guid bookingId);
    }
}
