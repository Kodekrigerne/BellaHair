using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Domain.Discounts
{
    public interface IFutureBookingWithTreatmentChecker
    {
        Task<bool> CheckFutureBookingsWithTreatment(Guid id);
    }
}
