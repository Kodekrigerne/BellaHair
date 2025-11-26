using BellaHair.Domain;
using BellaHair.Domain.Bookings;
using BellaHair.Domain.Invoices;
using BellaHair.Ports.Invoices;

// Mikkel Dahlmann

/// <summary>
/// Provides methods for generating sample invoice data for testing or demonstration purposes.
/// </summary>

public class InvoiceDocumentDataSource : IInvoiceDocumentDataSource
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ICurrentDateTimeProvider _currentDateTimeProvider;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IInvoiceQuery _invoiceQueryHandler;

    public InvoiceDocumentDataSource(
        IBookingRepository bookingRepository,
        ICurrentDateTimeProvider currentDateTimeProvider,
        IInvoiceRepository invoiceRepository,
        IInvoiceQuery invoiceQueryHandler)
    {
        _bookingRepository = bookingRepository;
        _currentDateTimeProvider = currentDateTimeProvider;
        _invoiceRepository = invoiceRepository;
        _invoiceQueryHandler = invoiceQueryHandler;
    }

    public async Task<InvoiceModel> GetInvoiceDetailsAsync(Guid Id)
    {
        var booking = await _bookingRepository.GetAsync(Id);
        var currentDate = _currentDateTimeProvider.GetCurrentDateTime();
        var id = await _invoiceQueryHandler.GetNextInvoiceIdAsync();

        return new InvoiceModel(
            id,
            currentDate,
            booking.CustomerSnapshot!,
            booking.TreatmentSnapshot!)
        { };
    }
}
