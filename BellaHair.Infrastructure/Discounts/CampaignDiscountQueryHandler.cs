using BellaHair.Domain.Discounts;
using BellaHair.Ports.Discounts;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.Discounts
{
    public class CampaignDiscountQueryHandler : ICampaignDiscountQuery
    {
        private readonly BellaHairContext _db;

        public CampaignDiscountQueryHandler(BellaHairContext db)
            => _db = db;

        async Task<List<CampaignDiscountDTO>> ICampaignDiscountQuery.GetAllAsync()
        {
            return await _db.Discounts
                .AsNoTracking()
                .OfType<CampaignDiscount>()
                .Select(x => new CampaignDiscountDTO(
                    x.Name,
                    x.DiscountPercent.Value,
                    x.StartDate,
                    x.EndDate,
                    x.Treatments.Select(t => t.Id)))
                .ToListAsync();
        }

        async Task<int> ICampaignDiscountQuery.GetCountAsync()
        {
            return await _db.Discounts.AsNoTracking().CountAsync();
        }
    }
}
