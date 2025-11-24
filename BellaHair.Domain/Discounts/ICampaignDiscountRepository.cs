namespace BellaHair.Domain.Discounts
{
    public interface ICampaignDiscountRepository
    {
        Task AddAsync(CampaignDiscount campaignDiscount);
        Task Delete(CampaignDiscount campaignDiscount);
        Task UpdateAsync(CampaignDiscount campaignDiscount);
        Task<CampaignDiscount> GetAsync(Guid id);
        Task SaveChangesAsync();

    }
}
