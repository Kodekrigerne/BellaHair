using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Ports.Discounts
{
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
