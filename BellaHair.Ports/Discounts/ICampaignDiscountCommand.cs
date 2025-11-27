namespace BellaHair.Ports.Discounts
{
    // Mikkel Klitgaard
    /// <summary>
    /// Defines operations for creating and deleting campaign discounts asynchronously.
    /// </summary>
    /// <remarks>Implementations of this interface should ensure that discount creation and deletion
    /// operations are performed in a reliable and consistent manner. Methods are asynchronous and may involve external
    /// resources such as databases or services.</remarks>
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
