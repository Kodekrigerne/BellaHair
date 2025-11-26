using System.ComponentModel.DataAnnotations;

namespace BellaHair.Presentation.WebUI.Components.Pages.Discounts.CampaignDiscount.ValidationAttributes
{
    // Mikkel Klitgaard
    /// <summary>
    /// Provides a validation attribute that ensures an end date is later than a start date for a model containing both
    /// values.
    /// </summary>
    /// <remarks>Apply this attribute to a property representing an end date in a model that also contains a
    /// start date property. The attribute compares the end date and start date of the object instance during validation
    /// and returns a validation error if the end date is not after the start date. This attribute is typically used in
    /// scenarios where chronological order between two date properties must be enforced, such as campaign or event
    /// models.</remarks>
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
