namespace BellaHair.Ports.Discounts
{
    //Dennis
    /// <summary>
    /// Exposes queries related to LoyaltyDiscounts for the frontend to use.
    /// </summary>
    public interface ILoyaltyDiscountQuery
    {
        Task<List<LoyaltyDiscountDTO>> GetAllAsync();
        Task<int> GetCountAsync();
    }

    public record LoyaltyDiscountDTO(Guid Id, string Name, int MinimumVisits, decimal DiscountPercent);
}
