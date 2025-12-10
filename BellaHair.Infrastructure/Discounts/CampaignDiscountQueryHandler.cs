using BellaHair.Domain.Discounts;
using BellaHair.Ports.Discounts;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

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
        private readonly ICurrentDateTimeProvider _dateTimeProvider;

        public CampaignDiscountQueryHandler(BellaHairContext db, ICurrentDateTimeProvider dateTimeProvider)
        {
            _db = db;
            _dateTimeProvider = dateTimeProvider;
        }

        async Task<List<CampaignDiscountDTO>> ICampaignDiscountQuery.GetAllAsync()
        {
            // Vi henter alle kampagner ud
            var campaigns = await _db.Discounts
                .AsNoTracking()
                .OfType<CampaignDiscount>()
                .ToListAsync();

            // Så hentes alle behandlings id'er ud på alle kampagner 
            var allTreatmentIds = campaigns
                .SelectMany(c => c.TreatmentIds)
                .Distinct()
                .ToList();

            // Så trækker vi de behandlinger ud fra vores allTreatmentIds liste
            // og tager både navn og id fra disse, som gemmes i en dictionary (id, navn)
            var treatmentsDict = await _db.Treatments
                .AsNoTracking()
                .Where(t => allTreatmentIds.Contains(t.Id))
                .ToDictionaryAsync(t => t.Id, t => t.Name);

            // Til sidst returnerer vi vores CampaignDiscountDTO som nu indeholder navn og id 
            // på behandlingerne som er tilknyttet vores kampagnerabatter.
            return campaigns.Select(c => new CampaignDiscountDTO(
                c.Id,
                c.Name,
                c.DiscountPercent.Value,
                c.StartDate,
                c.EndDate,
                c.TreatmentIds.Select(id => new CampaignTreatmentDTO(
                    id,
                    treatmentsDict.GetValueOrDefault(id, "Ukendt behandling")))
            )).ToList();
        }

        async Task<int> ICampaignDiscountQuery.GetCountAsync()
        {
            return await _db.Discounts.OfType<CampaignDiscount>().AsNoTracking().CountAsync();
        }

        public async Task<int> GetActiveCountAsync()
        {
            var now = _dateTimeProvider.GetCurrentDateTime().Date;

            return await _db.Discounts
                .AsNoTracking()
                .OfType<CampaignDiscount>()
                .Where(c => c.StartDate.Date <= now && c.EndDate.Date > now)
                .CountAsync();
        }
    }
}
