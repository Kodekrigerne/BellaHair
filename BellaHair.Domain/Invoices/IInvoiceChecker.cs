// Mikkel Dahlmann

namespace BellaHair.Domain.Invoices
{

    /// <summary>
    /// Defines methods for checking the existence and payment status of invoices associated with a booking.
    /// </summary>

    public interface IInvoiceChecker
    {
        Task<bool> HasInvoice(Guid bookingId);
        Task<bool> HasBeenPaid(Guid bookingId);
    }
}
