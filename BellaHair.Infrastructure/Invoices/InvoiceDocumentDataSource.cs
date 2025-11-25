using BellaHair.Domain;
using BellaHair.Domain.Bookings;
using BellaHair.Domain.Invoices;

// Mikkel Dahlmann

/// <summary>
/// Provides methods for generating sample invoice data for testing or demonstration purposes.
/// </summary>

public class InvoiceDocumentDataSource
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ICurrentDateTimeProvider _currentDateTimeProvider;

    public InvoiceDocumentDataSource(IBookingRepository bookingRepository, ICurrentDateTimeProvider currentDateTimeProvider)
    {
        _bookingRepository = bookingRepository;
        _currentDateTimeProvider = currentDateTimeProvider;
    }

    public async Task<InvoiceModel> GetInvoiceDetailsAsync(Guid Id)
    {
        var booking = await _bookingRepository.GetAsync(Id);
        var currentDate = _currentDateTimeProvider.GetCurrentDateTime();

        return new InvoiceModel(
            booking.Id,
            currentDate,
            booking.CustomerSnapshot!,
            booking.TreatmentSnapshot!,
            "Test comment")
        { };
    }
}
