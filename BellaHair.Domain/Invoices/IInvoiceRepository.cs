namespace BellaHair.Domain.Invoices
{
    // Mikkel Dahlmann

    /// <summary>
    /// Defines a contract for managing and accessing invoice data within a data store.
    /// </summary>

    public interface IInvoiceRepository
    {
        Task AddAsync(Invoice invoice);
        Task<Invoice> GetAsync(int id);
        Task<Invoice> GetInvoiceByBookingIdAsync(Guid bookingId);
        Task<InvoiceData> GetInvoiceDataAsync(Guid Id);
        Task SaveChangesAsync();
    }
}
