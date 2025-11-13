namespace BellaHair.Presentation.WebUI.Components.Pages.Discounts
{
    public interface IDiscountViewModel;
    public record LoyaltyDiscountViewModel(Guid Id, string Name, int MinimumVisits, decimal DiscountPercent) : IDiscountViewModel;
    public record CampaignDiscountViewModel() : IDiscountViewModel;
}
