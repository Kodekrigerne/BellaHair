using BellaHair.Domain;
using BellaHair.Domain.Bookings;
using BellaHair.Domain.Employees;
using BellaHair.Domain.PrivateCustomers;
using BellaHair.Domain.Treatments;
using BellaHair.Ports.Bookings;

namespace BellaHair.Application
{
    public class BookingCommandHandler : IBookingCommand
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPrivateCustomerRepository _privateCustomerRepository;
        private readonly ITreatmentRepository _treatmentRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;
        private readonly IBookingOverlapChecker _bookingOverlapChecker;

        public BookingCommandHandler(
            IEmployeeRepository employeeRepository,
            IPrivateCustomerRepository privateCustomerRepository,
            ITreatmentRepository treatmentRepository,
            IBookingRepository bookingRepository,
            ICurrentDateTimeProvider currentDateTimeProvider,
            IBookingOverlapChecker bookingOverlapChecker)
        {
            _employeeRepository = employeeRepository;
            _privateCustomerRepository = privateCustomerRepository;
            _treatmentRepository = treatmentRepository;
            _bookingRepository = bookingRepository;
            _currentDateTimeProvider = currentDateTimeProvider;
            _bookingOverlapChecker = bookingOverlapChecker;
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

            await _bookingRepository.AddAsync(booking);

            await _bookingRepository.SaveChangesAsync();
        }
    }
}
