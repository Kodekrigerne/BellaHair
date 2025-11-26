// Mikkel Dahlmann

namespace BellaHair.Ports.Invoices
{
    /// <summary>
    /// Defines a contract for creating and printing an invoice asynchronously.
    /// </summary>

    public interface IInvoiceQuery
    {
        Task GetInvoiceByBookingId(Guid BookingId);
        Task<int> GetNextInvoiceIdAsync();
    }
}
