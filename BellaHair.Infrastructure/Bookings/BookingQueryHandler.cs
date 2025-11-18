using BellaHair.Domain;
using BellaHair.Ports.Bookings;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.Bookings
{
    internal class BookingQueryHandler : IBookingQueryHandler
    {
        private readonly BellaHairContext _db;
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;

        public BookingQueryHandler(BellaHairContext db, ICurrentDateTimeProvider currentDateTimeProvider)
        {
            _db = db;
            _currentDateTimeProvider = currentDateTimeProvider;
        }

        async Task<IEnumerable<BookingSimpleDTO>> IBookingQueryHandler.GetAllAsync()
        {
            var bookings = await _db.Bookings
                .AsNoTracking()
                .Include(b => b.Treatment)
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .ToListAsync();

            //TODO: Vælg mellem denne og nedenstående
            return bookings.Select(b => new BookingSimpleDTO(
                b.StartDateTime,
                b.Total,
                b.Employee?.Name.FullName ?? b.EmployeeSnapshot?.FullName
                    ?? throw new InvalidOperationException($"Booking {b.Id} does not have an employee attached."),

                b.Customer?.Name.FullName ?? b.CustomerSnapshot?.FullName
                    ?? throw new InvalidOperationException($"Booking {b.Id} does not have a customer attached."),

                b.Treatment?.Name ?? b.TreatmentSnapshot?.Name
                    ?? throw new InvalidOperationException($"Booking {b.Id} does not have a treatment attached."),

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
