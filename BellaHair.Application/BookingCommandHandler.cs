using BellaHair.Application.Invoices;
using BellaHair.Domain;
using BellaHair.Domain.Bookings;
using BellaHair.Domain.Discounts;
using BellaHair.Domain.Employees;
using BellaHair.Domain.Invoices;
using BellaHair.Domain.PrivateCustomers;
using BellaHair.Domain.Treatments;
using BellaHair.Ports.Bookings;
using Microsoft.Extensions.Options;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

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
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly BusinessInfoSettings _businessInfoSettings;

        public BookingCommandHandler(
            IEmployeeRepository employeeRepository,
            IPrivateCustomerRepository privateCustomerRepository,
            ITreatmentRepository treatmentRepository,
            IBookingRepository bookingRepository,
            ICurrentDateTimeProvider currentDateTimeProvider,
            IBookingOverlapChecker bookingOverlapChecker,
            IDiscountCalculatorService discountCalculatorService,
            IInvoiceRepository invoiceRepository,
            IUnitOfWork unitOfWork,
            IOptions<BusinessInfoSettings> businessInfoSettings)
        {
            _employeeRepository = employeeRepository;
            _privateCustomerRepository = privateCustomerRepository;
            _treatmentRepository = treatmentRepository;
            _bookingRepository = bookingRepository;
            _currentDateTimeProvider = currentDateTimeProvider;
            _bookingOverlapChecker = bookingOverlapChecker;
            _discountCalculatorService = discountCalculatorService;
            _invoiceRepository = invoiceRepository;
            _unitOfWork = unitOfWork;
            _businessInfoSettings = businessInfoSettings.Value;
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
            var discount = await _discountCalculatorService.GetBestDiscount(booking, true);
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

        async Task IBookingCommand.PayAndInvoiceBooking(PayAndInvoiceBookingCommand command)
        {
            // Der startes en transaction for at bevare atomicity i forbindelse med først betaling af booking og derefter
            // oprettelse af fakturaen, der afhænger af at bookingen i databasen er markeret betalt.
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var booking = await _bookingRepository.GetAsync(command.Id);

                if (command.Discount != null)
                {
                    var discount = BookingDiscount.Active(command.Discount.Name, command.Discount.Amount, (DiscountType)command.Discount.Type);

                    booking.SetDiscount(discount);
                }
                if (booking.Customer != null && booking.Discount!.Type == DiscountType.BirthdayDiscount)
                {
                    booking.Customer.RegisterBirthdayDiscountUsed(booking.StartDateTime.Year);
                }

                booking.PayBooking(_currentDateTimeProvider);

                await _unitOfWork.SaveChangesAsync();

                QuestPDF.Settings.License = LicenseType.Community;

                var invoiceData = await _invoiceRepository.GetInvoiceDataAsync(command.Id);
                var document = new InvoiceDocument(invoiceData, _businessInfoSettings);

                byte[] pdfBytes = document.GeneratePdf();

                var invoice = Invoice.Create(invoiceData.Id, booking, pdfBytes);

                await _invoiceRepository.AddAsync(invoice);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (DomainException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new DomainException($"Fejl under betaling og fakturering af booking: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception($"Fejl under betaling og fakturering af booking: {ex.Message}", ex);
            }
            

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
            var discount = await _discountCalculatorService.GetBestDiscount(booking, true);
            if (discount != null) booking.SetDiscount(discount);

            booking.Update(command.StartDateTime, employee, treatment, _currentDateTimeProvider);

            await _bookingRepository.SaveChangesAsync();
        }
    }
}
