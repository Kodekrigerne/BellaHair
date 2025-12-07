using BellaHair.Domain;
using BellaHair.Domain.Bookings;
using BellaHair.Infrastructure.PrivateCustomers;
using BellaHair.Ports.Bookings;
using BellaHair.Ports.Discounts;
using BellaHair.Ports.Employees;
using BellaHair.Ports.PrivateCustomers;
using BellaHair.Ports.Treatments;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace BellaHair.Infrastructure.Bookings
{
    //Dennis
    /// <inheritdoc cref="IBookingQuery"/>
    internal class BookingQueryHandler : IBookingQuery
    {
        private readonly BellaHairContext _db;
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;
        private readonly IBookingOverlapChecker _bookingOverlapChecker;
        private readonly ICustomerVisitsService _customerVisitsService;

        public BookingQueryHandler(BellaHairContext db, ICurrentDateTimeProvider currentDateTimeProvider, IBookingOverlapChecker bookingOverlapChecker, ICustomerVisitsService customerVisitsService)
        {
            _db = db;
            _currentDateTimeProvider = currentDateTimeProvider;
            _bookingOverlapChecker = bookingOverlapChecker;
            _customerVisitsService = customerVisitsService;
        }

        async Task<BookingWithRelationsDTO> IBookingQuery.GetWithRelationsAsync(GetWithRelationsQuery query)
        {
            var now = _currentDateTimeProvider.GetCurrentDateTime();

            var booking = await _db.Bookings
                .Include(b => b.Treatment)
                    .ThenInclude(bt => bt!.Employees)
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                    .ThenInclude(be => be!.Bookings.Where(beb => beb.EndDateTime > now))
                .Include(b => b.ProductLines)
                    .ThenInclude(bpl => bpl.Product)
                .Include(b => b.ProductLineSnapshots)
                .SingleOrDefaultAsync(b => b.Id == query.Id) ?? throw new Exception("");

            if (booking.Employee == null && booking.EmployeeSnapshot == null) throw new InvalidOperationException($"Booking {booking.Id} does not have an employee attached.");
            if (booking.Customer == null && booking.CustomerSnapshot == null) throw new InvalidOperationException($"Booking {booking.Id} does not have a customer attached.");
            if (booking.Treatment == null && booking.TreatmentSnapshot == null) throw new InvalidOperationException($"Booking {booking.Id} does not have a treatment attached.");

            // Til medarbejder, behandling og kunder foretrækker vi at bruge deres relationer.
            // Hvis relationen er slettet anvender vi snapshots med data fra behandlingsdata

            var employee = new EmployeeNameWithBookingsDTO(
                booking.Employee?.Id ?? booking.EmployeeSnapshot!.EmployeeId,
                booking.Employee?.Name.FullName ?? booking.EmployeeSnapshot!.FullName,
                booking.Employee?.Bookings.Select(eb => new BookingTimesOnlyDTO(eb.Id, eb.StartDateTime, eb.EndDateTime)).ToList() ?? []);

            var treatment = new TreatmentDTO(
                booking.Treatment?.Id ?? booking.TreatmentSnapshot!.TreatmentId,
                booking.Treatment?.Name ?? booking.TreatmentSnapshot!.Name,
                booking.Treatment?.Price.Value ?? booking.TreatmentSnapshot!.Price,
                booking.Treatment?.DurationMinutes.Value ?? booking.TreatmentSnapshot!.DurationMinutes,
                0);

            var visits = await _customerVisitsService.GetCustomerVisitsAsync(booking.Customer?.Id ?? booking.CustomerSnapshot!.CustomerId);

            var customer = new PrivateCustomerSimpleDTO(
                booking.Customer?.Id ?? booking.CustomerSnapshot!.CustomerId,
                booking.Customer?.Name.FullName ?? booking.CustomerSnapshot!.FullName,
                booking.Customer?.Birthday ?? booking.CustomerSnapshot!.Birthday,
                booking.Customer?.Email.Value ?? booking.CustomerSnapshot!.Email,
                booking.Customer?.PhoneNumber.Value ?? booking.CustomerSnapshot!.PhoneNumber,
                booking.Customer?.Address.FullAddress ?? booking.CustomerSnapshot!.FullAddress,
                visits);

            var products = booking.IsPaid
                ? booking.ProductLineSnapshots.Select(pls => new ProductLineDTO(pls.ProductId, pls.Name, pls.Description, pls.Price, pls.Quantity))
                : booking.ProductLines.Select(pl => new ProductLineDTO(pl.Product.Id, pl.Product.Name, pl.Product.Description, pl.Product.Price.Value, pl.Quantity.Value));

            var discount = booking.Discount != null
                ? new DiscountDTO(
                    booking.Discount.Name,
                    booking.Discount.Amount,
                    (DiscountType)booking.Discount.Type)
                : null;

            return new BookingWithRelationsDTO(
                booking.StartDateTime,
                booking.IsPaid,
                employee,
                customer,
                treatment,
                products,
                discount);
        }

        async Task<int> IBookingQuery.GetNewCountAsync()
        {
            return await _db.Bookings
                .AsNoTracking()
                .Where(b => b.EndDateTime > _currentDateTimeProvider.GetCurrentDateTime())
                .CountAsync();
        }

        async Task<int> IBookingQuery.GetOldCountAsync()
        {
            return await _db.Bookings
                .AsNoTracking()
                .Where(b => b.EndDateTime < _currentDateTimeProvider.GetCurrentDateTime())
                .CountAsync();
        }

        async Task<IEnumerable<BookingDTO>> IBookingQuery.GetAllNewAsync()
            => await ((IBookingQuery)this).GetNewPaginatedAsync(0, int.MaxValue);

        async Task<IEnumerable<BookingDTO>> IBookingQuery.GetNewPaginatedAsync(int skip, int take)
        {
            var ordered = _db.Bookings
                .AsNoTracking()
                .Where(b => b.EndDateTime > _currentDateTimeProvider.GetCurrentDateTime())
                .OrderBy(b => b.EndDateTime);

            var filtered = FilterNullBookings(ordered)
                .Skip(skip)
                .Take(take)
                .Include(b => b.Treatment)
                .Include(b => b.ProductLines)
                    .ThenInclude(bpl => bpl.Product)
                .Include(b => b.ProductLineSnapshots);

            return await MapToBookingDTOs(filtered);
        }

        async Task<IEnumerable<BookingDTO>> IBookingQuery.GetAllOldAsync()
            => await ((IBookingQuery)this).GetOldPaginatedAsync(0, int.MaxValue);

        async Task<IEnumerable<BookingDTO>> IBookingQuery.GetOldPaginatedAsync(int skip, int take)
        {
            var ordered = _db.Bookings
                .AsNoTracking()
                .Where(b => b.EndDateTime < _currentDateTimeProvider.GetCurrentDateTime())
                .OrderByDescending(b => b.EndDateTime);

            var filtered = FilterNullBookings(ordered)
                .Skip(skip)
                .Take(take)
                .Include(b => b.Treatment)
                .Include(b => b.ProductLines)
                    .ThenInclude(bpl => bpl.Product)
                .Include(b => b.ProductLineSnapshots);

            return await MapToBookingDTOs(filtered);
        }

        async Task<bool> IBookingQuery.BookingHasOverlap(BookingIsAvailableQuery query)
        {
            return await _bookingOverlapChecker.OverlapsWithBooking(query.StartDateTime, query.DurationMinutes, query.EmployeeId, query.CustomerId, query.BookingId);
        }

        /// <summary>
        /// Returns all bookings on specific employee within a specific range of date.
        /// Only used for booking calendar view to limit bookings loaded for effeciency.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        async Task<IEnumerable<BookingCalendarDTO>> IBookingQuery.GetAllWithinPeriodOnEmployee(DateTime startDate, DateTime endDate, Guid employeeId)
        {
            return await _db.Bookings.AsNoTracking().Where(b => b.Customer != null && b.Treatment != null && b.Employee != null && employeeId == b.Employee.Id && b.StartDateTime > startDate && b.EndDateTime < endDate)
                .Select(b => new BookingCalendarDTO(b.Id, b.StartDateTime, b.EndDateTime, b.Employee!.Name.FullName, b.Customer!.Name.FullName, b.Treatment!.Name)).ToListAsync();
        }

        private static IQueryable<Booking> FilterNullBookings(IQueryable<Booking> query)
        {
            return query.Where(b =>
                (b.Employee != null || b.EmployeeSnapshot != null) &&
                (b.Customer != null || b.CustomerSnapshot != null) &&
                (b.Treatment != null || b.TreatmentSnapshot != null) &&
                ((b.IsPaid && b.TreatmentSnapshot != null) ||
                (!b.IsPaid && b.Treatment != null)));
        }

        private static IQueryable<Booking> ApplySearchFilter(IQueryable<Booking> query, string search)
        {
            if (search == null || search == string.Empty) return query;

            return query.Where(b =>
                (b.Customer != null ? b.Customer.Name.FullName : b.CustomerSnapshot!.FullName)
                    .Contains(search, StringComparison.OrdinalIgnoreCase) ||

                (b.Employee != null ? b.Employee.Name.FullName : b.EmployeeSnapshot!.FullName)
                    .Contains(search, StringComparison.OrdinalIgnoreCase) ||

                (b.Customer != null ? b.Customer.Address.FullAddress : b.CustomerSnapshot!.FullAddress)
                    .Contains(search, StringComparison.OrdinalIgnoreCase) ||

                (b.Customer != null ? b.Customer.PhoneNumber.Value : b.CustomerSnapshot!.PhoneNumber)
                    .Contains(search, StringComparison.OrdinalIgnoreCase) ||

                (b.Customer != null ? b.Customer.Email.Value : b.CustomerSnapshot!.Email)
                    .Contains(search, StringComparison.OrdinalIgnoreCase) ||

                (b.Treatment != null ? b.Treatment.Name : b.TreatmentSnapshot!.Name)
                    .Contains(search, StringComparison.OrdinalIgnoreCase) ||

                (b.Discount != null ? b.Discount.Name : "")
                    .Contains(search, StringComparison.OrdinalIgnoreCase) ||

                b.StartDateTime.ToString("dddd. MMMM. yyyy", new CultureInfo("da-DK"))
                    .Contains(search, StringComparison.OrdinalIgnoreCase) ||

                b.StartDateTime.ToString("dd/MM")
                    .Contains(search, StringComparison.OrdinalIgnoreCase)
            );
        }

        private static async Task<IEnumerable<BookingDTO>> MapToBookingDTOs(IQueryable<Booking> query)
        {
            return await query
            .Select(b => new BookingDTO(
                b.Id,
                b.StartDateTime,
                b.EndDateTime,
                b.IsPaid,
                b.TotalBase,
                b.TotalWithDiscount,
                b.Employee != null ? b.Employee.Name.FullName : b.EmployeeSnapshot!.FullName,
                b.Customer != null ? b.Customer.Name.FullName : b.CustomerSnapshot!.FullName,
                b.Customer != null ? b.Customer.Address.FullAddress : b.CustomerSnapshot!.FullAddress,
                b.Customer != null ? b.Customer.PhoneNumber.Value : b.CustomerSnapshot!.PhoneNumber,
                b.Customer != null ? b.Customer.Email.Value : b.CustomerSnapshot!.Email,
                b.Treatment != null ? b.Treatment.Name : b.TreatmentSnapshot!.Name,

                // Hvis bookingen er betalt (?) bruger vi den snapshottede treatment tid da dennes historiske værdi er mest relevant
                // Hvis den ikke er betalt (:) bruger vi værdien fra relationen
                b.IsPaid ? b.TreatmentSnapshot!.DurationMinutes : b.Treatment!.DurationMinutes.Value,

                b.Discount != null ? new DiscountDTO(b.Discount.Name, b.Discount.Amount, (DiscountType)b.Discount.Type) : null
                ))
            .ToListAsync();
        }
    }
}
