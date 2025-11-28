using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain.Bookings;
using BellaHair.Domain.Discounts;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Domain.Treatments;
using BellaHair.Domain.Treatments.ValueObjects;
using FixtureBuilder;

namespace BellaHair.Domain.Tests.Discounts
{
    internal sealed class CampaignDiscountTests
    {
        // Mikkel Klitgaard

        [Test]
        public void Create_Given_ValidCampaignDiscount_Then_CreatesCampaignDiscount()
        {
            // Arrange

            var name = "Vinterkampagne";
            var discountPercent = DiscountPercent.FromDecimal(0.20m);
            var startDate = new DateTime(2025, 12, 1);
            var endDate = new DateTime(2026, 3, 1);

            var treatmentIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

            // Act

            var campaignDiscount =
                CampaignDiscount.Create(name, discountPercent, startDate, endDate, treatmentIds);

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(campaignDiscount.Name, Is.EqualTo(name));
                Assert.That(campaignDiscount.DiscountPercent, Is.EqualTo(discountPercent));
                Assert.That(campaignDiscount.StartDate, Is.EqualTo(startDate));
                Assert.That(campaignDiscount.EndDate, Is.EqualTo(endDate));
                Assert.That(campaignDiscount.TreatmentIds, Is.EqualTo(treatmentIds));   
            });

        }

        [Test]
        public void Create_Given_StartDateIsAfterEndDate_Then_ThrowsException()
        {
            // Arrange

            var name = "Sommerkampagne";
            var discountPercent = DiscountPercent.FromDecimal(0.30m);
            var startDate = new DateTime(2025, 5, 10);
            var invalidEndDate = new DateTime(2025, 5, 1);

            var treatmentIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

            // Act & Assert

            Assert.Throws<CampaignDiscountException>(() => 
                CampaignDiscount.Create(name, discountPercent, startDate, invalidEndDate, treatmentIds));

        }

        [Test]
        public void Create_Given_EmptyTreatmentList_Then_ThrowsException()
        {
            // Arrange

            var name = "Forårskampagne";
            var discountPercent = DiscountPercent.FromDecimal(0.10m);
            var startDate = new DateTime(2025, 3, 1);
            var endDate = new DateTime(2025, 6, 1);

            var treatmentIds = new List<Guid>([]);
            
            // Act & Assert

            Assert.Throws<CampaignDiscountException>(() =>
                CampaignDiscount.Create(name, discountPercent, startDate, endDate, treatmentIds));

        }

        [Test]
        public void CalculateDiscount_Given_BookingInCampaignRange_Then_ReturnsActiveDiscount()
        {
            // Arrange

            var treatment = Treatment.Create("Herreklip", Price.FromDecimal(400), DurationMinutes.FromInt(30));

            var name = "Kampagne";
            var discountPercent = DiscountPercent.FromDecimal(0.20m);
            var startDate = new DateTime(2025, 12, 1);
            var endDate = new DateTime(2025, 12, 7);
            var treatmentIds = new List<Guid> { treatment.Id };

            var campaignDiscount = CampaignDiscount.Create(
                name, discountPercent, startDate, endDate, treatmentIds);

            var validDate = new DateTime(2025, 12, 5);

            var booking = Fixture.New<Booking>()
                .UseConstructor()
                .WithSetter(b => b.Treatment, treatment)
                .WithSetter(b => b.StartDateTime, validDate)
                .WithSetter(b => b.EndDateTime, validDate.AddMinutes(60))
                .Build();

            // Act

            var result = campaignDiscount.CalculateBookingDiscount(booking);

            // Assert

            Assert.That(result.DiscountActive, Is.True);
        }

        [Test]
        public void CalculateDiscount_Given_BookingOutsideCampaignRange_Then_ReturnsInactiveDiscount()
        {
            // Arrange

            var treatment = Treatment.Create("Herreklip", Price.FromDecimal(400), DurationMinutes.FromInt(30));

            var name = "Kampagne";
            var discountPercent = DiscountPercent.FromDecimal(0.20m);
            var startDate = new DateTime(2025, 12, 1);
            var endDate = new DateTime(2025, 12, 7);
            var treatmentIds = new List<Guid> { treatment.Id };

            var campaignDiscount = CampaignDiscount.Create(
                name, discountPercent, startDate, endDate, treatmentIds);

            var dateOutOfRange = new DateTime(2025, 12, 8);

            var booking = Fixture.New<Booking>()
                .UseConstructor()
                .WithSetter(b => b.Treatment, treatment)
                .WithSetter(b => b.StartDateTime, dateOutOfRange)
                .WithSetter(b=>b.EndDateTime, dateOutOfRange.AddMinutes(60))
                .Build();

            // Act

            var result = campaignDiscount.CalculateBookingDiscount(booking);

            // Assert

            Assert.That(result.DiscountActive, Is.False);
        }

        [Test]
        public void CalculateDiscount_Given_BookingWithoutCampaignTreatments_Then_ReturnsInactiveDiscount()
        {
            // Arrange

            var treatmentCampaign = Treatment.Create("Herreklip", Price.FromDecimal(400), DurationMinutes.FromInt(30));
            var treatmentWrong = Treatment.Create("Dameklip",Price.FromDecimal(500m),DurationMinutes.FromInt(60));

            var name = "Kampagne";
            var discountPercent = DiscountPercent.FromDecimal(0.20m);
            var startDate = new DateTime(2025, 12, 1);
            var endDate = new DateTime(2025, 12, 7);
            var treatmentIds = new List<Guid> { treatmentCampaign.Id };

            var campaignDiscount = CampaignDiscount.Create(
                name, discountPercent, startDate, endDate, treatmentIds);

            var dateOutOfRange = new DateTime(2025, 12, 8);

            var booking = Fixture.New<Booking>()
                .UseConstructor()
                .WithSetter(b => b.Treatment, treatmentWrong)
                .Build();

            // Act

            var result = campaignDiscount.CalculateBookingDiscount(booking);

            // Assert

            Assert.That(result.DiscountActive, Is.False);
        }
    }
}
