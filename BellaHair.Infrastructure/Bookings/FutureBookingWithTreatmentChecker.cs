using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain;
using BellaHair.Domain.Discounts;
using BellaHair.Domain.Treatments;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.Bookings
{
    public class FutureBookingWithTreatmentChecker : IFutureBookingWithTreatmentChecker
    {
        private readonly BellaHairContext _db;
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;

        public FutureBookingWithTreatmentChecker(BellaHairContext db, ICurrentDateTimeProvider date)
        {
            _db = db;
            _currentDateTimeProvider = date;
        }

        async Task<bool> IFutureBookingWithTreatmentChecker.CheckFutureBookingsWithTreatment(Guid treatmentId)
        {
            return await _db.Bookings
                .AsNoTracking()
                .AnyAsync(b => b.Treatment!.Id == treatmentId && b.StartDateTime > _currentDateTimeProvider.GetCurrentDateTime());
        }
    }
}
