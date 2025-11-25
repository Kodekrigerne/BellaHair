using System.Linq;
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
            var campaigns = await _db.Discounts
                .AsNoTracking()
                .OfType<CampaignDiscount>()
                .ToListAsync();

            return campaigns.Select(c => new CampaignDiscountDTO(
                c.Name,
                c.DiscountPercent.Value,
                c.StartDate,
                c.EndDate,
                c.TreatmentIds.Select(id => new CampaignTreatmentDTO(
                    id, 
                    _db.Treatments.Find(id)!.Name))
                )).ToList();
        }

        async Task<int> ICampaignDiscountQuery.GetCountAsync()
        {
            return await _db.Discounts.AsNoTracking().CountAsync();
        }
    }
}
