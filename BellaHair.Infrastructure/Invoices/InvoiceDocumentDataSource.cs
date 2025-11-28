using BellaHair.Domain;
using BellaHair.Domain.Invoices;
using BellaHair.Infrastructure;
using Microsoft.EntityFrameworkCore;

// Mikkel Dahlmann

/// <summary>
/// Provides methods for generating sample invoice data for testing or demonstration purposes.
/// </summary>

public class InvoiceDocumentDataSource : IInvoiceDocumentDataSource
{
    private readonly BellaHairContext _db;
    private readonly ICurrentDateTimeProvider _currentDateTimeProvider;

    public InvoiceDocumentDataSource(
        BellaHairContext db,
        ICurrentDateTimeProvider currentDateTimeProvider)
    {
        _db = db;
        _currentDateTimeProvider = currentDateTimeProvider;
    }

    public async Task<InvoiceModel> GetInvoiceDetailsAsync(Guid Id)
    {
        var booking = await _db.Bookings
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == Id)
            ?? throw new DomainException("Booking not found");

        var currentDate = _currentDateTimeProvider.GetCurrentDateTime();
        var id = await _db.Invoices.MaxAsync(i => (int?)i.Id) + 1 ?? 1;
        var discount = booking.Discount;
        var total = booking.Total;

        return InvoiceModel.Create(
            id,
            currentDate,
            booking.CustomerSnapshot!,
            booking.TreatmentSnapshot!,
            total,
            discount);
    }
}
