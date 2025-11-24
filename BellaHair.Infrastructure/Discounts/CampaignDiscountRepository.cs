using BellaHair.Domain.Discounts;

namespace BellaHair.Infrastructure.Discounts
{
    public class CampaignDiscountRepository : ICampaignDiscountRepository
    {
        private readonly BellaHairContext _db;

        public CampaignDiscountRepository(BellaHairContext db)
            => _db = db;

        async Task ICampaignDiscountRepository.AddAsync(CampaignDiscount campaignDiscount)
        {
            await _db.Discounts.AddAsync(campaignDiscount);
        }

        void ICampaignDiscountRepository.Delete(CampaignDiscount campaignDiscount)
        {
            _db.Discounts.Remove(campaignDiscount);
        }

        async Task<CampaignDiscount> ICampaignDiscountRepository.GetAsync(Guid id)
        {
            var discount = await _db.Discounts.FindAsync(id)
                           ?? throw new KeyNotFoundException($"Couldn't find campaign discount {id}");

            if (discount is CampaignDiscount campaignDiscount)
                return campaignDiscount;

            throw new InvalidOperationException($"Discount {id} is not a campaign discount.");
        }

        async Task ICampaignDiscountRepository.SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
