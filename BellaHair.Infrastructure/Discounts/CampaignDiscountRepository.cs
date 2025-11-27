using BellaHair.Domain.Discounts;

namespace BellaHair.Infrastructure.Discounts
{
    // Mikkel Klitgaard
    /// <summary>
    /// Provides methods for managing campaign discount entities in the data store.
    /// </summary>
    /// <remarks>This repository implements operations for adding, retrieving, deleting, and saving changes to
    /// campaign discounts. It is typically used to abstract data access logic for campaign discounts and should be used
    /// in conjunction with dependency injection. All operations are performed against the underlying BellaHairContext
    /// database context.</remarks>
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
