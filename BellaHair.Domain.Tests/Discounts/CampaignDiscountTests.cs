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
        // TODO: Mangler List<Guid> i TestCase
        [TestCase("Efterårskampagne",0.20, new DateTime(2025,12,1),new DateTime(2025,12,20),)
        public void Given_ValidCampaignDiscount_Then_CreatesCampaignDiscount(string name, decimal discountPercent, DateTime startDate, DateTime endDate, List<Guid> treatmentIds)
        {
            var validDiscountPercent = DiscountPercent.FromDecimal(discountPercent);

            var campaignDiscount =
                CampaignDiscount.Create(name, validDiscountPercent, startDate, endDate, treatmentIds);

            Assert.Multiple(() =>
            {
                Assert.That(campaignDiscount.Name, Is.EqualTo(name));
                Assert.That(campaignDiscount.DiscountPercent, Is.EqualTo(validDiscountPercent));
                Assert.That(campaignDiscount.StartDate, Is.EqualTo(startDate));
                Assert.That(campaignDiscount.EndDate, Is.EqualTo(endDate));
                Assert.That(campaignDiscount.TreatmentIds, Is.EqualTo(treatmentIds));   
            });


        }

        public void Given_StartDateIsAfterEndDate_Then_ThrowsException(string name, decimal discountPercent, DateTime startDate, DateTime endDate, List<Guid> treatmentIds)
        {
            var validDiscountPercent = DiscountPercent.FromDecimal(discountPercent);

            Assert.Throws<CampaignDiscountException>(() =>
                CampaignDiscount.Create(name, validDiscountPercent, startDate, endDate, treatmentIds));
        }

        public void Given_EmptyTreatmentList_Then_ThrowsException(string name, decimal discountPercent, DateTime startDate, DateTime endDate, List<Guid> treatmentIds)
        {
            var validDiscountPercent = DiscountPercent.FromDecimal(discountPercent); 
            
            Assert.Throws<CampaignDiscountException>(() =>
                CampaignDiscount.Create(name, validDiscountPercent, startDate, endDate, treatmentIds));

        }
    }
}
