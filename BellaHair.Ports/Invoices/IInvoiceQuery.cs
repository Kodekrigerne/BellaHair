// Mikkel Dahlmann

namespace BellaHair.Ports.Invoices
{
    /// <summary>
    /// Defines a contract for creating and printing an invoice asynchronously.
    /// </summary>

    public interface IInvoiceQuery
    {
        Task<byte[]> GetInvoicePdfByBookingIdAsync(GetInvoiceByBookingIdQuery query);
    }

    public record GetInvoiceByBookingIdQuery(Guid BookingId);
}
