using BellaHair.Domain;
using BellaHair.Domain.Bookings;
using BellaHair.Domain.Discounts;
using BellaHair.Domain.Employees;
using BellaHair.Domain.PrivateCustomers;
using BellaHair.Domain.Treatments;
using BellaHair.Ports.Bookings;

namespace BellaHair.Application
{
    //Dennis
    /// <inheritdoc cref="IBookingCommand" />
    public class BookingCommandHandler : IBookingCommand
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPrivateCustomerRepository _privateCustomerRepository;
        private readonly ITreatmentRepository _treatmentRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;
        private readonly IBookingOverlapChecker _bookingOverlapChecker;
        private readonly IDiscountCalculatorService _discountCalculatorService;

        public BookingCommandHandler(
            IEmployeeRepository employeeRepository,
            IPrivateCustomerRepository privateCustomerRepository,
            ITreatmentRepository treatmentRepository,
            IBookingRepository bookingRepository,
            ICurrentDateTimeProvider currentDateTimeProvider,
            IBookingOverlapChecker bookingOverlapChecker,
            IDiscountCalculatorService discountCalculatorService)
        {
            _employeeRepository = employeeRepository;
            _privateCustomerRepository = privateCustomerRepository;
            _treatmentRepository = treatmentRepository;
            _bookingRepository = bookingRepository;
            _currentDateTimeProvider = currentDateTimeProvider;
            _bookingOverlapChecker = bookingOverlapChecker;
            _discountCalculatorService = discountCalculatorService;
        }

        async Task IBookingCommand.CreateBooking(CreateBookingCommand command)
        {
            var employee = await _employeeRepository.GetWithTreatmentsAsync(command.EmployeeId);
            var customer = await _privateCustomerRepository.GetAsync(command.CustomerId);
            var treatment = await _treatmentRepository.GetAsync(command.TreatmentId);

            if (await _bookingOverlapChecker.OverlapsWithBooking(
                command.StartDateTime,
                treatment.DurationMinutes.Value,
                command.EmployeeId,
                command.CustomerId))
                throw new DomainException("Kan ikke oprette booking som overlapper med eksisterende booking.");

            var booking = Booking.Create(customer, employee, treatment, command.StartDateTime, _currentDateTimeProvider);

            //Den bedste rabat findes og tilføjes til bookingen
            var discount = await _discountCalculatorService.GetBestDiscount(booking);
            if (discount != null) booking.SetDiscount(discount);

            await _bookingRepository.AddAsync(booking);

            await _bookingRepository.SaveChangesAsync();
        }

        async Task IBookingCommand.DeleteBooking(DeleteBookingCommand command)
        {
            var booking = await _bookingRepository.GetAsync(command.Id);

            booking.ValidateDelete(_currentDateTimeProvider);

            _bookingRepository.Delete(booking);
            await _bookingRepository.SaveChangesAsync();
        }

        async Task IBookingCommand.PayBooking(PayBookingCommand command)
        {
            var booking = await _bookingRepository.GetAsync(command.Id);

            if (command.Discount != null)
            {
                var discount = BookingDiscount.Active(command.Discount.Name, command.Discount.Amount);
                booking.SetDiscount(discount);
            }

            booking.PayBooking(_currentDateTimeProvider);
            await _bookingRepository.SaveChangesAsync();
        }

        async Task IBookingCommand.UpdateBooking(UpdateBookingCommand command)
        {
            var booking = await _bookingRepository.GetAsync(command.Id);

            var employee = await _employeeRepository.GetWithTreatmentsAsync(command.EmployeeId);
            var treatment = await _treatmentRepository.GetAsync(command.TreatmentId);

            if (await _bookingOverlapChecker.OverlapsWithBooking(
                command.StartDateTime,
                treatment.DurationMinutes.Value,
                command.EmployeeId,
                booking.Customer!.Id,
                booking.Id))
                throw new DomainException("Kan ikke ændre booking som overlapper med eksisterende booking.");

            //Den bedste rabat findes og tilføjes til bookingen
            var discount = await _discountCalculatorService.GetBestDiscount(booking);
            if (discount != null) booking.SetDiscount(discount);

            booking.Update(command.StartDateTime, employee, treatment, _currentDateTimeProvider);

            await _bookingRepository.SaveChangesAsync();
        }
    }
}
