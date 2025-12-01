using BellaHair.Domain.Discounts;
using BellaHair.Domain.Treatments;
using BellaHair.Ports.Discounts;

namespace BellaHair.Application.Discounts
{
    // Mikkel Klitgaard
    /// <summary>
    /// Handles commands related to creating and deleting campaign discounts.
    /// </summary>
    /// <remarks>This class provides the implementation for the ICampaignDiscountCommand interface,
    /// coordinating operations between campaign discount and treatment repositories. It is typically used in
    /// application layers to manage campaign discount lifecycle actions.</remarks>
    public class CampaignDiscountCommandHandler : ICampaignDiscountCommand
    {
        private readonly ICampaignDiscountRepository _campaignDiscountRepo;


        public CampaignDiscountCommandHandler(
            ICampaignDiscountRepository campaignDiscountRepo)
        {
            _campaignDiscountRepo = campaignDiscountRepo;
        }


        async Task ICampaignDiscountCommand.CreateCampaignDiscountAsync(CreateCampaignDiscountCommand command)
        {
            var discountPercent = DiscountPercent.FromDecimal(command.DiscountPercent);

            var campaignDiscount = CampaignDiscount.Create(
                command.Name,
                discountPercent,
                command.StartDate,
                command.EndDate,
                command.TreatmentIds);

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
