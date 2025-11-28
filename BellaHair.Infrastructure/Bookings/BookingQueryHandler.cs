using BellaHair.Domain;
using BellaHair.Domain.Bookings;
using BellaHair.Ports.Bookings;
using BellaHair.Ports.Discounts;
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
            booking.IsPaid,

            booking.Employee?.Id ?? booking.EmployeeSnapshot?.EmployeeId
                ?? throw new InvalidOperationException($"Booking {booking.Id} does not have an employee attached."),

            booking.Customer?.Id ?? booking.CustomerSnapshot?.CustomerId
                ?? throw new InvalidOperationException($"Booking {booking.Id} does not have a customer attached."),

            booking.Treatment?.Id ?? booking.TreatmentSnapshot?.TreatmentId
                ?? throw new InvalidOperationException($"Booking {booking.Id} does not have a treatment attached."),

            booking.Discount != null 
                ? new DiscountDTO(
                    booking.Discount.Name, 
                    booking.Discount.Amount, 
                    (DiscountTypeDTO)booking.Discount.Type) 
                : null
            );
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
                .Where(b => b.EndDateTime < _currentDateTimeProvider.GetCurrentDateTime())
                .Include(b => b.Treatment)
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .ToListAsync();

            return MapToBookingDTOs(bookings);
        }

        async Task<bool> IBookingQuery.BookingHasOverlap(BookingIsAvailableQuery query)
        {
            return await _bookingOverlapChecker.OverlapsWithBooking(query.StartDateTime, query.DurationMinutes, query.EmployeeId, query.CustomerId, query.bookingId);
        }

        private static IEnumerable<BookingDTO> MapToBookingDTOs(IEnumerable<Booking> bookings)
        {
            // Meget verbos, men nødvendigt da relationer kan være slettet for gamle bookings
            // Exceptions kastes kun hvis relationen er slettet OG der ikke er sat snapshots (hvilket der skal være, dermed fejl)
            return bookings.Select(b =>
            {
                if (b.Employee == null && b.EmployeeSnapshot == null) throw new InvalidOperationException($"Booking {b.Id} does not have an employee attached.");
                if (b.Customer == null && b.CustomerSnapshot == null) throw new InvalidOperationException($"Booking {b.Id} does not have a customer attached.");
                if (b.Treatment == null && b.TreatmentSnapshot == null) throw new InvalidOperationException($"Booking {b.Id} does not have a treatment attached.");

                return new BookingDTO(
                b.Id,
                b.StartDateTime,
                b.EndDateTime,
                b.IsPaid,
                b.TotalBase,
                b.TotalWithDiscount,
                b.Employee?.Name.FullName ?? b.EmployeeSnapshot!.FullName,
                b.Customer?.Name.FullName ?? b.CustomerSnapshot!.FullName,
                b.Customer?.Address.FullAddress ?? b.CustomerSnapshot!.FullAddress,
                b.Customer?.PhoneNumber.Value ?? b.CustomerSnapshot!.PhoneNumber,
                b.Customer?.Email.Value ?? b.CustomerSnapshot!.Email,
                b.Treatment?.Name ?? b.TreatmentSnapshot!.Name,

                // Hvis bookingen er betalt (?) bruger vi den snapshottede treatment tid da dennes historiske værdi er mest relevant
                // Hvis den ikke er betalt (:) bruger vi værdien fra relationen
                //TODO: Ryd op i det her
                b.IsPaid
                    ? b.TreatmentSnapshot?.DurationMinutes
                        ?? throw new InvalidOperationException($"Booking {b.Id} is paid but missing a TreatmentSnapshot.")
                    : b.Treatment?.DurationMinutes.Value
                        ?? throw new InvalidOperationException($"Booking {b.Id} is unpaid but missing a treatment."),

                b.Discount != null ? new DiscountDTO(b.Discount.Name, b.Discount.Amount, (DiscountTypeDTO)b.Discount.Type) : null
                );
            });
        }
    }
}
