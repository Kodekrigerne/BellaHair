// Mikkel Dahlmann

namespace BellaHair.Application
{

    /// <summary>
    /// Defines a contract for sending invoice emails asynchronously.
    /// </summary>

    public interface IEmailService
    {
        Task SendInvoiceAsync(string Email, byte[] Invoice, string Message);
    }
}
