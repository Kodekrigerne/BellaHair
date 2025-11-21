using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BellaHair.Presentation.WebUI.Components.Shared.ValidationAttributes;

// Mikkel Dahlmann

/// <summary>
/// Specifies that a data field value is valid only if it consists exclusively of letters or dashes.
/// </summary>

public class LettersDashOnlyAttribute : ValidationAttribute
{
    // Overskriver standard fejlbeskeden.
    public LettersDashOnlyAttribute() :
        base("Feltet kan kun indeholde bogstaver og bindestreger.")
    {
    }

    protected override ValidationResult? IsValid(object? input, ValidationContext validationContext)
    {
        // Kører validering på input.
        if (input != null && input is string stringValue)
        {
            if (stringValue.Any(x => !char.IsLetter(x) && x != '-'))
            {
                return new ValidationResult(ErrorMessage, [validationContext.MemberName!]);
            }
        }

        return ValidationResult.Success;
    }
}
