using BellaHair.Domain.Discounts;
using BellaHair.Ports.Discounts;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.Discounts
{
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
