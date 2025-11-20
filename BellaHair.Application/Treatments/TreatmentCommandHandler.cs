using BellaHair.Domain.Bookings;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Domain.Treatments;
using BellaHair.Domain.Treatments.ValueObjects;
using BellaHair.Ports.Treatments;

namespace BellaHair.Application.Treatments
{
    // Mikkel Klitgaard
    /// <summary>
    /// Handles commands related to treatments.
    /// </summary>
    /// <remarks>This class implements the <see cref="ITreatmentCommand"/> interface and provides
    /// functionality for creating and deleting treatments. It relies on an <see cref="ITreatmentRepository"/> to
    /// persist changes to the underlying data store.</remarks>
    public class TreatmentCommandHandler : ITreatmentCommand
    {

        private readonly ITreatmentRepository _treatmentRepository;
        private readonly IFutureBookingWithTreatmentChecker _bookingChecker;

        public TreatmentCommandHandler(ITreatmentRepository treatmentRepository, IFutureBookingWithTreatmentChecker bookingChecker)
        {
            _treatmentRepository = treatmentRepository;
            _bookingChecker = bookingChecker;
        }

        async Task ITreatmentCommand.CreateTreatmentAsync(CreateTreatmentCommand command)
        {
            var price = Price.FromDecimal(command.Price);
            var duration = DurationMinutes.FromInt(command.DurationMinutes);

            var treatment = Treatment.Create(command.Name, price, duration);

            await _treatmentRepository.AddAsync(treatment);
            await _treatmentRepository.SaveChangesAsync();
        }

        async Task ITreatmentCommand.DeleteTreatmentAsync(DeleteTreatmentCommand command)
        {
            bool isBooked = await _bookingChecker.CheckFutureBookingsWithTreatmentAsync(command.Id);

            if (isBooked)
            {
                throw new TreatmentInUseException($"Can't delete treatment with id {command.Id}: Treatment is used in a booking.");
            }

            var treatment = await _treatmentRepository.GetAsync(command.Id);
            _treatmentRepository.Delete(treatment);
            await _treatmentRepository.SaveChangesAsync();
        }
    }
}
