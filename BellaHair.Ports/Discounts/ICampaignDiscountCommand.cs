namespace BellaHair.Ports.Discounts
{
    public interface ICampaignDiscountCommand
    {
        Task CreateCampaignDiscountAsync(CreateCampaignDiscountCommand command);
        Task DeleteCampaignDiscountAsync(DeleteCampaignDiscountCommand command);
    }

    public class CreateCampaignDiscountCommand(
        string Name,
        decimal Discount,
        DateTime StartDate,
        DateTime EndDate,
        IEnumerable<Guid> TreatmentIds);


    public class DeleteCampaignDiscountCommand(Guid id);


}
