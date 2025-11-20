using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BellaHair.Presentation.WebUI.Components.Pages.PrivateCustomers.CreatePrivateCustomerComponents.ValidationAttributes;

// Mikkel Dahlmann

/// <summary>
/// Specifies that a data field value is valid only if it consists of number characters.
/// </summary>

public class NumbersOnlyAttribute : ValidationAttribute
{
    // Overskriver standard fejlbeskeden.
    public NumbersOnlyAttribute() :
        base("Feltet kan kun indeholde tal.")
    {
    }

    protected override ValidationResult? IsValid(object? input, ValidationContext validationContext)
    {
        // Kører validering på input.
        if (input is not int)
        {
            return new ValidationResult(ErrorMessage, [validationContext.MemberName!]);
        }

        return ValidationResult.Success;
    }
}