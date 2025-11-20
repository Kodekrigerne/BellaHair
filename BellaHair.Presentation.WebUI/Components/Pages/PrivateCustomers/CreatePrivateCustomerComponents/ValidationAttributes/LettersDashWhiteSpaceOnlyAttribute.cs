using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BellaHair.Presentation.WebUI.Components.Pages.PrivateCustomers.CreatePrivateCustomerComponents.ValidationAttributes;

// Mikkel Dahlmann

/// <summary>
/// Specifies that a data field value is valid only if it consists exclusively of letters, dashes or whitespaces.
/// </summary>

public class LettersDashWhiteSpaceOnlyAttribute : ValidationAttribute
{
    // Overskriver standard fejlbeskeden.
    public LettersDashWhiteSpaceOnlyAttribute() :
        base("Feltet kan kun indeholde bogstaver, bindestreger og mellemrum.")
    {
    }

    protected override ValidationResult? IsValid(object? input, ValidationContext validationContext)
    {
        // Kører validering på input.
        if (input is string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue) || stringValue.Any(x => !char.IsLetter(x) && x != '-' && x != ' '))
            {
                return new ValidationResult(ErrorMessage, [validationContext.MemberName!]);
            }
        }

        return ValidationResult.Success;
    }
}
