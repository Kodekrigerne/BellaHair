// Mikkel Dahlmann

namespace BellaHair.Domain.Invoices
{

    /// <summary>
    /// Represents a data source for retrieving invoice document details.
    /// </summary>

    public interface IInvoiceDocumentDataSource
    {
        Task<InvoiceModel> GetInvoiceDetailsAsync(Guid Id);
    }
}
