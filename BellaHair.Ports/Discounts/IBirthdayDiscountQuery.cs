namespace BellaHair.Ports.Discounts
{
    // Mikkel Klitgaard
    /// <summary>
    /// Defines a query for retrieving the current birthday discount information asynchronously.
    /// </summary>

    public interface IBirthdayDiscountQuery
    {
        Task<BirthdayDiscountDTO?> GetBirthdayDiscountAsync();
    }

    public record BirthdayDiscountDTO(Guid Id, string Name, decimal DiscountPercent);
}
