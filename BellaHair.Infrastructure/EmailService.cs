using BellaHair.Application;

// Mikkel Dahlmann

namespace BellaHair.Infrastructure
{

    /// <summary>
    /// Provides functionality for sending emails with invoice attachments.
    /// </summary>

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

            await Task.Delay(1);
        }
    }
}
