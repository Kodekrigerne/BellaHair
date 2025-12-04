using BellaHair.Domain;
using BellaHair.Domain.Invoices;
using Microsoft.EntityFrameworkCore;

// Mikkel Dahlmann

namespace BellaHair.Infrastructure.Invoices
{

    /// <summary>
    /// Provides methods for managing and retrieving invoices from the data store.
    /// </summary>

    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly BellaHairContext _db;
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;

        public InvoiceRepository(BellaHairContext db, ICurrentDateTimeProvider currentDataTimeProvider)
        {
            _db = db;
            _currentDateTimeProvider = currentDataTimeProvider;
        }

        async Task IInvoiceRepository.AddAsync(Invoice invoice)
        {
            await _db.Invoices.AddAsync(invoice);
        }

        async Task<Invoice> IInvoiceRepository.GetAsync(int id)
        {
            var invoice = await _db.Invoices
                .FirstOrDefaultAsync(p => p.Id == id)
                    ?? throw new KeyNotFoundException($"No invoice exists with ID {id}");

            return invoice;
        }

        async Task<Invoice> IInvoiceRepository.GetInvoiceByBookingIdAsync(Guid bookingId)
        {
            return await _db.Invoices
                .FirstOrDefaultAsync(i => i.BookingId == bookingId)
                    ?? throw new KeyNotFoundException($"No invoice exists for booking ID {bookingId}");
        }

        async Task<InvoiceData> IInvoiceRepository.GetInvoiceDataAsync(Guid Id)
        {
            var booking = await _db.Bookings
                .AsNoTracking()
                .Include(b => b.ProductLineSnapshots)
                .FirstOrDefaultAsync(b => b.Id == Id)
                ?? throw new DomainException("Booking not found");

            var currentDate = _currentDateTimeProvider.GetCurrentDateTime();

            // Tjekker for den højeste nuværende faktura-ID og øger den med 1 for at generere et nyt ID.
            // Denne løsning kan lade sig gøre, så længe der ikke er tale om et fler-bruger system.
            var id = await _db.Invoices.MaxAsync(i => (int?)i.Id) + 1 ?? 1;
            var discount = booking.Discount;
            var total = booking.TotalBase;

            return new InvoiceData(
                id,
                currentDate,
                booking.CustomerSnapshot!,
                booking.TreatmentSnapshot!,
                total,
                discount);
        }

        async Task IInvoiceRepository.SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
