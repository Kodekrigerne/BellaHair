using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BellaHair.Presentation.WebUI.Components.Shared.ValidationAttributes;

// Mikkel Dahlmann

/// <summary>
/// Specifies that a data field value is valid only if it consists of a valid danish zipcode.
/// </summary>

public class ZipCodeAttribute : ValidationAttribute
{
    // Overskriver standard fejlbeskeden.
    public ZipCodeAttribute() :
        base("Feltet kan kun indeholde tal mellem 1000 og 9999")
    {
    }

    protected override ValidationResult? IsValid(object? input, ValidationContext validationContext)
    {
        // Kører validering på input.
        if (input is int intValue)
        {
            if (intValue is > 9999 or < 1000)
            {
                return new ValidationResult(ErrorMessage, [validationContext.MemberName!]);
            }
        }

        return ValidationResult.Success;
    }
}