using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain;
using BellaHair.Domain.Bookings;
using BellaHair.Domain.Treatments;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.Bookings
{
    // Mikkel Klitgaard
    /// <summary>
    /// Provides functionality to check for future bookings associated with a specific treatment.
    /// </summary>
    public class FutureBookingWithTreatmentChecker : IFutureBookingWithTreatmentChecker
    {
        private readonly BellaHairContext _db;
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;

        public FutureBookingWithTreatmentChecker(BellaHairContext db, ICurrentDateTimeProvider date)
        {
            _db = db;
            _currentDateTimeProvider = date;
        }

        async Task<bool> IFutureBookingWithTreatmentChecker.CheckFutureBookingsWithTreatmentAsync(Guid treatmentId)
        {
            var currentDateTime = _currentDateTimeProvider.GetCurrentDateTime();

            return await _db.Bookings
                .AsNoTracking()
                .Where(b => b.Treatment!.Id == treatmentId)
                .AnyAsync(b => b.StartDateTime.AddMinutes(b.Treatment!.DurationMinutes.Value) > currentDateTime);
        }
    }
}
