using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Domain.Discounts
{
    // Mikkel Klitgaard
    /// <summary>
    /// Specifies the types of discounts that can be applied to a transaction.
    /// </summary>
    /// <remarks>Use this enumeration to indicate the reason or category for a discount.</remarks>


    // !! IMPORTANT !!
    //Når du opretter en ny rabat-type, så HUSK også at tilføje denne til DiscountTypeDTO i BellaHair.Ports
    //Ændre ikke på rækkefølgen, da vi caster DiscountTypeDTO til DiscountType
    public enum DiscountType
    {
        LoyaltyDiscount = 0,
        CampaignDiscount = 1,
        BirthdayDiscount = 2
    }
}
