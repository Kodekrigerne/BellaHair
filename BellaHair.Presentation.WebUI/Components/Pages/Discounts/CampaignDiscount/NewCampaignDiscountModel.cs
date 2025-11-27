using System.ComponentModel.DataAnnotations;
using BellaHair.Presentation.WebUI.Components.Pages.Discounts.CampaignDiscount.ValidationAttributes;

namespace BellaHair.Presentation.WebUI.Components.Pages.Discounts.CampaignDiscount
{
    // Mikkel Klitgaard
    public class NewCampaignDiscountModel
    {
        [Required(ErrorMessage = "Kampagnenavn er påkrævet.")]
        public string Name { get; set; } = "";

        // Den "ægte" værdi til backenden (0.0 - 1.0)
        public decimal DiscountPercent { get; set; }

        // Wrapper property til UI (1 - 100)
        [Range(1, 100, ErrorMessage = "Rabatten skal være mellem 1 - 100%")]
        public int DiscountPercentInput
        {
            get => (int)(DiscountPercent * 100);
            set => DiscountPercent = value / 100m;
        }

        [Required(ErrorMessage = "Startdato skal angives.")]
        [FutureDate]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Slutdato skal angives.")]
        [FutureDate]
        [EndDateMustBeAfterStartDate]
        public DateTime EndDate { get; set; }

        public IEnumerable<Guid> TreatmentIds { get; set; } = [];
    }
}
