using System.ComponentModel.DataAnnotations;

namespace BellaHair.Presentation.WebUI.Components.Pages.Discounts.CampaignDiscount.ValidationAttributes
{
    // Mikkel Klitgaard
    /// <summary>
    /// Provides a validation attribute that ensures a date value represents today or a future date.
    /// </summary>
    /// <remarks>Apply this attribute to properties or fields of type <see cref="DateTime"/> to restrict
    /// values to today or later. This is commonly used to prevent users from selecting past dates in forms or models.
    /// The validation compares the input date to the current system date and time.</remarks>
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
