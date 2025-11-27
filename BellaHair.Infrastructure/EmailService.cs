using BellaHair.Application;

namespace BellaHair.Infrastructure
{
    public class EmailService : IEmailService
    {
        async Task IEmailService.SendInvoiceAsync(string Email, byte[] Invoice, string Message)
        {
            Console.WriteLine($"""
                Sender email til:
                {Email}

                Med besked:
                {Message}

                Vedhæftet: 
                {Invoice}
                """);
        }
    }
}
