using BellaHair.Domain.Discounts;
using BellaHair.Ports.Discounts;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.Discounts
{
    // Mikkel Klitgaard
    /// <summary>
    /// Provides query operations for retrieving birthday discount information from the database.
    /// </summary>
    /// <remarks>This class implements the <see cref="IBirthdayDiscountQuery"/> interface and is typically
    /// used to access birthday discount data in a read-only manner. Instances should be created with a valid <see
    /// cref="BellaHairContext"/> to ensure proper database access.</remarks>
    public class BirthdayDiscountQueryHandler : IBirthdayDiscountQuery
    {
        private readonly BellaHairContext _db;

        public BirthdayDiscountQueryHandler(BellaHairContext db)
            => _db = db;

        async Task<BirthdayDiscountDTO?> IBirthdayDiscountQuery.GetBirthdayDiscountAsync()
        {
            return await _db.Discounts
                .AsNoTracking()
                .OfType<BirthdayDiscount>()
                .Select(x => new BirthdayDiscountDTO(x.Id, x.Name, x.DiscountPercent.Value))
                .SingleOrDefaultAsync();
        }
    }
}
