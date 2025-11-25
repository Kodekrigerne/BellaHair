using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Ports.Discounts
{
    // Mikkel Klitgaard 
    /// <summary>
    /// Defines methods for querying campaign discount data asynchronously.
    /// </summary>
    /// <remarks>Implementations of this interface provide access to campaign discount information, typically
    /// for use in business logic or reporting scenarios. Methods return results asynchronously to support non-blocking
    /// operations, such as database queries or remote service calls.</remarks>
    public interface ICampaignDiscountQuery
    {
        Task<List<CampaignDiscountDTO>> GetAllAsync();
        Task<int> GetCountAsync();
    }

    public record CampaignDiscountDTO(
        string Name,
        decimal DiscountPercent,
        DateTime StartDate,
        DateTime EndDate,
        IEnumerable<CampaignTreatmentDTO> Treatments);

    public record CampaignTreatmentDTO(Guid Id, string Name);
}
