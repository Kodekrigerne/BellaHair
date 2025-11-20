using System.ComponentModel.DataAnnotations;
using System.Linq;
using BellaHair.Domain;
using BellaHair.Infrastructure;

namespace BellaHair.Presentation.WebUI.Components.Pages.PrivateCustomers.CreatePrivateCustomerComponents.ValidationAttributes;

// Mikkel Dahlmann

/// <summary>
/// Specifies that a data field value is valid only if the date is minium 18 years in the past.
/// </summary>

public class MustBeAdultAttribute : ValidationAttribute
{
    // Overskriver standard fejlbeskeden.
    public MustBeAdultAttribute() :
        base("Kunden skal være fyldt 18 år.")
    {
    }

    protected override ValidationResult? IsValid(object? input, ValidationContext validationContext)
    {
        if (input is DateTime dateTimeValue)
        {
            var dateTimeProvider =
                validationContext.GetService(typeof(ICurrentDateTimeProvider)) as ICurrentDateTimeProvider;

            if (dateTimeProvider == null)
            {
                return new ValidationResult("Internal validation error: Date Time Provider not found.");
            }

            var currentDateTime = dateTimeProvider.GetCurrentDateTime();

            // Kører validering på input.
            if (dateTimeValue > currentDateTime.AddYears(-18))
            {
                return new ValidationResult(ErrorMessage, [validationContext.MemberName!]);
            }
        }

        return ValidationResult.Success;
    }
}