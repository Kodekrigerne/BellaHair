namespace BellaHair.Ports.Discounts
{
    public interface ICampaignDiscountCommand
    {
        Task CreateCampaignDiscountAsync(CreateCampaignDiscountCommand command);
        Task DeleteCampaignDiscountAsync(DeleteCampaignDiscountCommand command);
    }

    public record CreateCampaignDiscountCommand(
        string Name,
        decimal DiscountPercent,
        DateTime StartDate,
        DateTime EndDate,
        IEnumerable<Guid> TreatmentIds);


    public record DeleteCampaignDiscountCommand(Guid Id);


}
