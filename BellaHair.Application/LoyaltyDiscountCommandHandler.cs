using BellaHair.Domain.Discounts;
using BellaHair.Ports;

namespace BellaHair.Application
{
    //Dennis
    class LoyaltyDiscountCommandHandler : ILoyaltyDiscountCommand
    {
        private readonly ILoyaltyDiscountRepository _loyaltyDiscountRepo;

        public LoyaltyDiscountCommandHandler(ILoyaltyDiscountRepository loyaltyDiscountRepo)
            => _loyaltyDiscountRepo = loyaltyDiscountRepo;

        async Task ILoyaltyDiscountCommand.CreateLoyaltyDiscountAsync(CreateLoyaltyDiscountCommand command)
        {
            var discountPercent = DiscountPercent.FromDecimal(command.DiscountPercent);

            var discount = LoyaltyDiscount.Create(command.Name, command.MinimumVisits, discountPercent);

            await _loyaltyDiscountRepo.AddAsync(discount);

            await _loyaltyDiscountRepo.SaveChangesAsync();
        }

        async Task ILoyaltyDiscountCommand.DeleteLoyaltyDiscountAsync(DeleteLoyaltyDiscountCommand command)
        {
            var discount = await _loyaltyDiscountRepo.Get(command.Id);

            _loyaltyDiscountRepo.Delete(discount);

            await _loyaltyDiscountRepo.SaveChangesAsync();
        }
    }
}
