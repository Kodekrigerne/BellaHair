namespace BellaHair.Ports.Discounts
{
    //Dennis
    /// <summary>
    /// Exposes commands related to LoyaltyDiscounts for the frontend to use.
    /// </summary>
    public interface ILoyaltyDiscountCommand
    {
        Task CreateLoyaltyDiscountAsync(CreateLoyaltyDiscountCommand command);
        Task DeleteLoyaltyDiscountAsync(DeleteLoyaltyDiscountCommand command);
    }

    public record CreateLoyaltyDiscountCommand(string Name, int MinimumVisits, decimal DiscountPercent);

    public record DeleteLoyaltyDiscountCommand(Guid Id);
}
