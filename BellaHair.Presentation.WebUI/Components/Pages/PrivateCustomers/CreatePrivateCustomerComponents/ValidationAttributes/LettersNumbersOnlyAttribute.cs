using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BellaHair.Presentation.WebUI.Components.Pages.PrivateCustomers.CreatePrivateCustomerComponents.ValidationAttributes;

// Mikkel Dahlmann

/// <summary>
/// Specifies that a data field value is valid only if it consists exclusively of numbers or characters.
/// </summary>

public class LettersNumbersOnlyAttribute : ValidationAttribute
{
    // Overskriver standard fejlbeskeden.
    public LettersNumbersOnlyAttribute() :
        base("Feltet kan kun indeholde bogstaver og tal.")
    {
    }

    protected override ValidationResult IsValid(object input, ValidationContext validationContext)
    {
        // Kører validering på input.
        if (input is string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue) == stringValue.Any(x => !char.IsLetterOrDigit(x)))
            {
                return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
            }
        }

        return ValidationResult.Success;
    }
}