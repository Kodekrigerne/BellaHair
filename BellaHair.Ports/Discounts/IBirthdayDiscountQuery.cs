namespace BellaHair.Ports.Discounts
{
    public interface IBirthdayDiscountQuery
    {
        Task<BirthdayDiscountDTO?> GetBirthdayDiscountAsync();
    }

    public record BirthdayDiscountDTO(Guid Id, string Name, decimal DiscountPercent);
}
