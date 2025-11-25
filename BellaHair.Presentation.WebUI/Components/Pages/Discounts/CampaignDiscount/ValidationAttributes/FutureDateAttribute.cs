using System.ComponentModel.DataAnnotations;

namespace BellaHair.Presentation.WebUI.Components.Pages.Discounts.CampaignDiscount.ValidationAttributes
{
    public class FutureDateAttribute : ValidationAttribute
    {

        protected override ValidationResult? IsValid(object? input, ValidationContext validationContext)
        {
            if (input is DateTime dateTimeValue)
            {
                if (dateTimeValue < DateTime.Now)
                {
                    return new ValidationResult("Datoen skal være i fremtiden.", [validationContext.MemberName!]);
                }
            }
            return ValidationResult.Success;
        }
    }
}
