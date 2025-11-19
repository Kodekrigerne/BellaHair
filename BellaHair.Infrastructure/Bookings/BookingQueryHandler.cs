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

        public BookingQueryHandler(BellaHairContext db, ICurrentDateTimeProvider currentDateTimeProvider)
        {
            _db = db;
            _currentDateTimeProvider = currentDateTimeProvider;
        }

        async Task<IEnumerable<BookingSimpleDTO>> IBookingQuery.GetAllNewAsync()
        {
            var bookings = await _db.Bookings
                .AsNoTracking()
                .Where(b => b.Treatment != null || b.TreatmentSnapshot != null)
                .Where(b => b.StartDateTime.AddMinutes(
                    b.Treatment != null
                    ? b.Treatment.DurationMinutes.Value
                    : b.TreatmentSnapshot!.DurationMinutes)
                > _currentDateTimeProvider.GetCurrentDateTime())
                .Include(b => b.Treatment)
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .ToListAsync();

            return MapToBookingSimpleDTOs(bookings);
        }

        async Task<IEnumerable<BookingSimpleDTO>> IBookingQuery.GetAllOldAsync()
        {
            var bookings = await _db.Bookings
                .AsNoTracking()
                .Where(b => b.Treatment != null || b.TreatmentSnapshot != null)
                .Where(b => b.StartDateTime.AddMinutes(
                    b.Treatment != null
                    ? b.Treatment.DurationMinutes.Value
                    : b.TreatmentSnapshot!.DurationMinutes)
                < _currentDateTimeProvider.GetCurrentDateTime())
                .Include(b => b.Treatment)
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .ToListAsync();

            return MapToBookingSimpleDTOs(bookings);
        }

        private IEnumerable<BookingSimpleDTO> MapToBookingSimpleDTOs(IEnumerable<Booking> bookings)
        {
            // Meget verbos, men nødvendigt da relationer kan være slettet for gamle bookings
            // Exceptions kastes kun hvis relationen er slettet OG der ikke er sat snapshots (hvilket der skal være, dermed fejl)
            return bookings.Select(b => new BookingSimpleDTO(
                b.StartDateTime,
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
