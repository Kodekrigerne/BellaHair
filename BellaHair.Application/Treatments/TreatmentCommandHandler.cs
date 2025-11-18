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

        public TreatmentCommandHandler(ITreatmentRepository treatmentRepository)
        {
            _treatmentRepository = treatmentRepository;
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
            var treatment = await _treatmentRepository.GetAsync(command.Id);

            _treatmentRepository.Delete(treatment);

            await _treatmentRepository.SaveChangesAsync();
        }
    }
}
