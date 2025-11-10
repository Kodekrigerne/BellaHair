namespace BellaHair.Ports
{
    //Dennis
    public interface ILoyaltyDiscountCommand
    {
        Task CreateLoyaltyDiscountAsync(CreateLoyaltyDiscountCommand command);
        Task DeleteLoyaltyDiscountAsync(DeleteLoyaltyDiscountCommand command);
    }

    public record CreateLoyaltyDiscountCommand(string Name, int MinimumVisits, decimal DiscountPercent);

    public record DeleteLoyaltyDiscountCommand(Guid Id);
}
