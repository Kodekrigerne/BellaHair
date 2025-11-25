using BellaHair.Domain;
using BellaHair.Domain.Bookings;
using BellaHair.Ports.Bookings;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.Bookings
{
    //Dennis
    /// <inheritdoc cref="IBookingQuery"/>
    internal class BookingQueryHandler : IBookingQuery
    {
        private readonly BellaHairContext _db;
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;
        private readonly IBookingOverlapChecker _bookingOverlapChecker;

        public BookingQueryHandler(BellaHairContext db, ICurrentDateTimeProvider currentDateTimeProvider, IBookingOverlapChecker bookingOverlapChecker)
        {
            _db = db;
            _currentDateTimeProvider = currentDateTimeProvider;
            _bookingOverlapChecker = bookingOverlapChecker;
        }

        async Task<BookingWithRelationsDTO> IBookingQuery.GetWithRelationsAsync(GetWithRelationsQuery query)
        {
            var booking = await _db.Bookings
                .Include(b => b.Employee)
                .Include(b => b.Treatment)
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(b => b.Id == query.Id) ?? throw new KeyNotFoundException($"Booking {query.Id} not found.");

            return new BookingWithRelationsDTO(
            booking.StartDateTime,

            booking.Employee?.Id ?? booking.EmployeeSnapshot?.EmployeeId
                ?? throw new InvalidOperationException($"Booking {booking.Id} does not have an employee attached."),

            booking.Customer?.Id ?? booking.CustomerSnapshot?.CustomerId
                ?? throw new InvalidOperationException($"Booking {booking.Id} does not have a customer attached."),

            booking.Treatment?.Id ?? booking.TreatmentSnapshot?.TreatmentId
                ?? throw new InvalidOperationException($"Booking {booking.Id} does not have a treatment attached."),
            booking.Discount != null ? new DiscountDTO(booking.Discount.Name, booking.Discount.Amount) : null);
        }

        async Task<IEnumerable<BookingDTO>> IBookingQuery.GetAllNewAsync()
        {
            var bookings = await _db.Bookings
                .AsNoTracking()
                .Where(b => b.EndDateTime > _currentDateTimeProvider.GetCurrentDateTime())
                .Include(b => b.Treatment)
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .ToListAsync();

            return MapToBookingDTOs(bookings);
        }

        async Task<IEnumerable<BookingDTO>> IBookingQuery.GetAllOldAsync()
        {
            var bookings = await _db.Bookings
                .AsNoTracking()
                .Where(b => b.TreatmentSnapshot != null)
                .Where(b => b.EndDateTime < _currentDateTimeProvider.GetCurrentDateTime())
                .Include(b => b.Treatment)
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .ToListAsync();

            return MapToBookingDTOs(bookings);
        }

        async Task<bool> IBookingQuery.BookingHasOverlap(BookingIsAvailableQuery query)
        {
            return await _bookingOverlapChecker.OverlapsWithBooking(query.StartDateTime, query.DurationMinutes, query.EmployeeId, query.CustomerId);
        }

        private static IEnumerable<BookingDTO> MapToBookingDTOs(IEnumerable<Booking> bookings)
        {
            // Meget verbos, men nødvendigt da relationer kan være slettet for gamle bookings
            // Exceptions kastes kun hvis relationen er slettet OG der ikke er sat snapshots (hvilket der skal være, dermed fejl)
            return bookings.Select(b => new BookingDTO(
                b.StartDateTime,
                b.EndDateTime,
                b.Total,
                b.Employee?.Name.FullName ?? b.EmployeeSnapshot?.FullName
                    ?? throw new InvalidOperationException($"Booking {b.Id} does not have an employee attached."),

                b.Customer?.Name.FullName ?? b.CustomerSnapshot?.FullName
                    ?? throw new InvalidOperationException($"Booking {b.Id} does not have a customer attached."),

                b.Treatment?.Name ?? b.TreatmentSnapshot?.Name
                    ?? throw new InvalidOperationException($"Booking {b.Id} does not have a treatment attached."),

                // Hvis bookingen er betalt (?) bruger vi den snapshottede treatment tid da dennes historiske værdi er mest relevant
                // Hvis den ikke er betalt (:) bruger vi værdien fra relationen
                b.IsPaid
                    ? b.TreatmentSnapshot?.DurationMinutes
                        ?? throw new InvalidOperationException($"Booking {b.Id} is paid but missing a TreatmentSnapshot.")
                    : b.Treatment?.DurationMinutes.Value
                        ?? throw new InvalidOperationException($"Booking {b.Id} is unpaid but missing a treatment."),

                b.Discount != null ? new DiscountDTO(b.Discount.Name, b.Discount.Amount) : null
                ));
        }
    }
}
