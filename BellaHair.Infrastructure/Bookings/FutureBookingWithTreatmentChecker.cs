using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain;
using BellaHair.Domain.Discounts;
using BellaHair.Domain.Treatments;

namespace BellaHair.Infrastructure.Bookings
{
    public class FutureBookingWithTreatmentChecker : IFutureBookingWithTreatmentChecker
    {
        private readonly ITreatmentRepository _treatmentRepo;
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;

        public FutureBookingWithTreatmentChecker(ITreatmentRepository treatmentRepo, ICurrentDateTimeProvider currentDateTimeProvider)
        {
            _treatmentRepo = treatmentRepo;
            _currentDateTimeProvider = currentDateTimeProvider;
        }

        public Task<bool> CheckFutureBookings(Guid id)
        {

        }
    }
}
