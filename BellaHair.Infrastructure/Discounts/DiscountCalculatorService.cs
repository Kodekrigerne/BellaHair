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

        async Task<BookingDiscount?> IDiscountCalculatorService.GetBestDiscount(Booking booking)
        {
            BookingDiscount? bestBookingDiscount = null;

            var discounts = _db.Discounts.AsNoTracking().ToList();

            List<Task> tasks = [];

            foreach (var discount in discounts)
            {
                // Vi udregner hver discount OG tildeler hver (hvis bedre) til bestBookingDiscount som parallele Tasks
                // Dette skaber en race condition
                tasks.Add(Task.Run(() =>
                {
                    var curBookingDiscount = discount.CalculateBookingDiscount(booking);

                    if (curBookingDiscount.DiscountActive &&
                    (bestBookingDiscount == null || curBookingDiscount.Amount > bestBookingDiscount.Amount))
                    {
                        // Vi anvender en Lock for at løse den opståede race condition
                        lock (_lock)
                        {
                            bestBookingDiscount = curBookingDiscount;
                        }
                    }
                }));
            }

            await Task.WhenAll(tasks);
            return bestBookingDiscount;
        }
    }
}
