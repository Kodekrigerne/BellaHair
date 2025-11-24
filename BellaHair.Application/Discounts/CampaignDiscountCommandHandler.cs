using BellaHair.Domain.Discounts;
using BellaHair.Domain.Treatments;
using BellaHair.Ports.Discounts;

namespace BellaHair.Application.Discounts
{
    public class CampaignDiscountCommandHandler : ICampaignDiscountCommand
    {
        private readonly ICampaignDiscountRepository _campaignDiscountRepo;
        private readonly ITreatmentRepository _treatmentRepo;


        public CampaignDiscountCommandHandler(
            ICampaignDiscountRepository campaignDiscountRepo, ITreatmentRepository treatmentRepo)
        {
            _campaignDiscountRepo = campaignDiscountRepo;
            _treatmentRepo = treatmentRepo;
        }


        async Task ICampaignDiscountCommand.CreateCampaignDiscountAsync(CreateCampaignDiscountCommand command)
        {
            var discountPercent = DiscountPercent.FromDecimal(command.DiscountPercent);

            var treatments = await _treatmentRepo.GetAsync(command.TreatmentIds);

            var campaignDiscount = CampaignDiscount.Create(
                command.Name,
                discountPercent,
                command.StartDate,
                command.EndDate,
                treatments);

            await _campaignDiscountRepo.AddAsync(campaignDiscount);

            await _campaignDiscountRepo.SaveChangesAsync();
        }

        async Task ICampaignDiscountCommand.DeleteCampaignDiscountAsync(DeleteCampaignDiscountCommand command)
        {
            var campaignDiscount = await _campaignDiscountRepo.GetAsync(command.Id);

            _campaignDiscountRepo.Delete(campaignDiscount);

            await _campaignDiscountRepo.SaveChangesAsync();
        }
    }
}
