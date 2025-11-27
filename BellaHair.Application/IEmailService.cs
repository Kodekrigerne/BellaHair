namespace BellaHair.Application
{
    public interface IEmailService
    {
        Task SendInvoiceAsync(string Email, byte[] Invoice, string Message);
    }
}
