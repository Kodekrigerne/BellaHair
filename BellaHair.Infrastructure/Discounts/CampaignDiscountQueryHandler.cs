using System.Linq;
using BellaHair.Domain.Discounts;
using BellaHair.Ports.Discounts;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.Discounts
{

    // Mikkel Klitgaard
    /// <summary>
    /// Provides query operations for retrieving campaign discount information from the database.
    /// </summary>
    /// <remarks>This class implements the <see cref="ICampaignDiscountQuery"/> interface and is typically
    /// used to access campaign discount data in a read-only manner. All queries are performed asynchronously and use
    /// no-tracking for improved performance when data modification is not required.</remarks>
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


            // Jeg bruger en CampaignTreatmentDTO for at hente id ud på alle behandlinger
            // og finder det dertilhørende behandlingsnavn
            return campaigns.Select(c => new CampaignDiscountDTO(
                c.Id,
                c.Name,
                c.DiscountPercent.Value,
                c.StartDate,
                c.EndDate,
                c.TreatmentIds.Select(id => new CampaignTreatmentDTO(
                    id, 
                    _db.Treatments.Find(id)?.Name ?? "Ukendt behandling"))
                )).ToList();
        }

        async Task<int> ICampaignDiscountQuery.GetCountAsync()
        {
            return await _db.Discounts.OfType<CampaignDiscount>().AsNoTracking().CountAsync();
        }
    }
}
