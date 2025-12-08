using SharedKernel;
using System.ComponentModel.DataAnnotations;

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
            var dateTimeProvider = validationContext.GetRequiredService<ICurrentDateTimeProvider>();

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