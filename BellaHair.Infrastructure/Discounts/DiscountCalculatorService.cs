using BellaHair.Domain.Bookings;
using BellaHair.Domain.Discounts;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.Discounts
{
    //Dennis
    /// <inheritdoc cref="IDiscountCalculatorService"/>
    public class DiscountCalculatorService : IDiscountCalculatorService
    {
        private readonly BellaHairContext _db;
        private readonly Lock _lock = new();

        public DiscountCalculatorService(BellaHairContext db) => _db = db;

        async Task<BookingDiscount?> IDiscountCalculatorService.GetBestDiscount(Booking booking, bool includeBirthdayDiscount)
        {
            BookingDiscount? bestBookingDiscount = null;
            List<DiscountBase> discounts;

            var allDiscounts = await _db.Discounts.AsNoTracking().ToListAsync();

            if (!includeBirthdayDiscount)
            {
                discounts = allDiscounts
                    .Where(d => d.Type != DiscountType.BirthdayDiscount)
                    .ToList();
            }
            else
            {
                 discounts = allDiscounts;
            }

            List<Task> tasks = [];

            foreach (var discount in discounts)
            {
                // Vi udregner hver discount OG tildeler hver (hvis bedre) til bestBookingDiscount som parallele Tasks
                // Dette skaber en race condition
                tasks.Add(Task.Run(() =>
                {
                    var curBookingDiscount = discount.CalculateBookingDiscount(booking);

                    if (curBookingDiscount.DiscountActive)
                    {
                        // Vi anvender en Lock for at løse den opståede race condition
                        lock (_lock)
                        {
                            if (bestBookingDiscount == null || curBookingDiscount.Amount > bestBookingDiscount.Amount)
                            {
                                bestBookingDiscount = curBookingDiscount;
                            }
                        }
                    }
                }));
            }

            await Task.WhenAll(tasks);
            return bestBookingDiscount;
        }
    }
}
