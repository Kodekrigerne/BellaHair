using System.ComponentModel.DataAnnotations;

namespace BellaHair.Presentation.WebUI.Components.Pages.Discounts.CampaignDiscount.ValidationAttributes
{
    public class EndDateMustBeAfterStartDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? input, ValidationContext validationContext)
        {
            //Vi fortæller at objektet er af typen NewCampaignDiscountModel, så vi let kan tilgå StartDate og EndDate
            var model = (NewCampaignDiscountModel)validationContext.ObjectInstance;

            if (model.EndDate < model.StartDate)
            {
                return new ValidationResult("Slutdato skal være efter startdato.", [validationContext.MemberName!]);
            }

            return ValidationResult.Success;
        }
    }
}
