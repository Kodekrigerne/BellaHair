namespace BellaHair.Domain.Discounts
{
    public interface ICampaignDiscountRepository
    {
        Task AddAsync(CampaignDiscount campaignDiscount);
        void Delete(CampaignDiscount campaignDiscount);
        Task<CampaignDiscount> GetAsync(Guid id);
        Task SaveChangesAsync();

    }
}
