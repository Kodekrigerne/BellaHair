namespace BellaHair.Domain.Invoices
{
    public interface IInvoiceDocumentDataSource
    {
        Task<InvoiceModel> GetInvoiceDetailsAsync(Guid Id);
    }
}
