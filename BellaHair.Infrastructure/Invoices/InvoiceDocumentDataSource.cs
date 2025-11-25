using BellaHair.Domain.Bookings;
using BellaHair.Domain.Invoices;
using QuestPDF.Helpers;

// Mikkel Dahlmann

/// <summary>
/// Provides methods for generating sample invoice data for testing or demonstration purposes.
/// </summary>

public class InvoiceDocumentDataSource
{
    private readonly IBookingRepository _bookingRepository;

    public InvoiceDocumentDataSource(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public static InvoiceModel GetInvoiceDetails(Guid Id)
    {
        var items = Enumerable
            .Range(1, 8)
            .Select(i => GenerateRandomOrderItem())
            .ToList();

        return new InvoiceModel
        {
            InvoiceNumber = Random.Next(1_000, 10_000),
            IssueDate = DateTime.Now,
            DueDate = DateTime.Now + TimeSpan.FromDays(14),

            SellerAddress = GenerateRandomAddress(),
            CustomerAddress = GenerateRandomAddress(),

            Items = items,
            Comments = Placeholders.Paragraph()
        };
    }

    private static OrderItem GenerateRandomOrderItem()
    {
        return new OrderItem
        {
            Name = Placeholders.Label(),
            Price = (decimal)Math.Round(Random.NextDouble() * 100, 2),
            Quantity = Random.Next(1, 10)
        };
    }

    private static InvoiceAddress GenerateRandomAddress()
    {
        return new InvoiceAddress
        {
            CompanyName = Placeholders.Name(),
            Street = Placeholders.Label(),
            City = Placeholders.Label(),
            State = Placeholders.Label(),
            Email = Placeholders.Email(),
            Phone = Placeholders.PhoneNumber()
        };
    }
}
