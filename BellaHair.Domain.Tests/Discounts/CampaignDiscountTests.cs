using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain.Discounts;
using BellaHair.Domain.Treatments;

namespace BellaHair.Domain.Tests.Discounts
{
    internal sealed class CampaignDiscountTests
    {

        [Test]
        public void Given_ValidCampaignDiscount_Then_CreatesCampaignDiscount()
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
        public void Given_StartDateIsAfterEndDate_Then_ThrowsException()
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
        public void Given_EmptyTreatmentList_Then_ThrowsException()
        {
            // Arrange
            var name = "Forårskampagne";
            var discountPercent = DiscountPercent.FromDecimal(0.10m);
            var startDate = new DateTime(2025, 3, 1);
            var endDate = new DateTime(2025, 6, 1);

            var treatmentIds = new List<Guid>([]);
            
            Assert.Throws<CampaignDiscountException>(() =>
                CampaignDiscount.Create(name, discountPercent, startDate, endDate, treatmentIds));

        }
    }
}
