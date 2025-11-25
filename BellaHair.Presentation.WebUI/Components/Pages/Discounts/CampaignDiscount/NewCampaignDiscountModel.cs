using System.ComponentModel.DataAnnotations;
using BellaHair.Presentation.WebUI.Components.Pages.Discounts.CampaignDiscount.ValidationAttributes;

namespace BellaHair.Presentation.WebUI.Components.Pages.Discounts.CampaignDiscount
{
    public class NewCampaignDiscountModel
    {
        [Required(ErrorMessage = "Kampagnenavn er påkrævet.")]
        public string Name { get; set; } = "";

        [Range(1, 100, ErrorMessage = "Rabatten skal være mellem 1 - 100%")]
        public decimal DiscountPercent { get; set; }

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
