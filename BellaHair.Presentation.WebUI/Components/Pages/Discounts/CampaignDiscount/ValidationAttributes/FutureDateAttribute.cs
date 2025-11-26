using System.ComponentModel.DataAnnotations;

namespace BellaHair.Presentation.WebUI.Components.Pages.Discounts.CampaignDiscount.ValidationAttributes
{
    public class FutureDateAttribute : ValidationAttribute
    {

        protected override ValidationResult? IsValid(object? input, ValidationContext validationContext)
        {
            if (input is DateTime dateTimeValue)
            {
                if (dateTimeValue < DateTime.Now.AddDays(-1))
                {
                    return new ValidationResult("Startdato kan tidligst være i dag.", [validationContext.MemberName!]);
                }
            }
            return ValidationResult.Success;
        }
    }
}
