using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PromotionEngine;
using PromotionEngine.Models;
using PromotionEngine.Models.Promotions;
using PromotionEngine.Repositories;

namespace PromotionEngineTests
{
    public class PromotionsHelperTests
    {
        [Test]
        public void GetApplicablePromotions()
        {
            // Arrange
            var items = new List<Item>
            {
                new("A", 5),
                new("B", 3)
            };

            var promotions = new List<Promotion>();

            var multipleItemsPromotion =
                new MultipleItemsForFixedPricePromotion(10, "A", 3, 130);
            var multipleItemsPromotion2 =
                new MultipleItemsForFixedPricePromotion(30, "B", 2, 45);
            var bundleItemsPromotion =
                new BundleItemsTogetherForFixedPricePromotion(50, new List<string> { "C", "D" }, 30);
            var percentageDiscountPromotion =
                new PercentageDiscountPromotion(100, "D", 10);

            promotions.Add(multipleItemsPromotion);
            promotions.Add(multipleItemsPromotion2);
            promotions.Add(bundleItemsPromotion);
            promotions.Add(percentageDiscountPromotion);

            // Act
            var resultPromotions = PromotionHelper.GetApplicablePromotions(items, promotions);


            // Assert
            Assert.AreEqual(2, resultPromotions.Count);
            Assert.AreEqual(true, resultPromotions.OfType<MultipleItemsForFixedPricePromotion>().Any());
            Assert.Contains("A", resultPromotions.OfType<MultipleItemsForFixedPricePromotion>().Select(r => r.SkuId)?.ToList());
            Assert.Contains("B", resultPromotions.OfType<MultipleItemsForFixedPricePromotion>().Select(r => r.SkuId)?.ToList());
            Assert.False(resultPromotions.OfType<MultipleItemsForFixedPricePromotion>().Select(r => r.SkuId)?.Contains("C"));
            Assert.False(resultPromotions.OfType<MultipleItemsForFixedPricePromotion>().Select(r => r.SkuId)?.Contains("D"));



        }

        [Test]
        public void GetApplicablePromotions_Same_SKUID()
        {
            // Arrange
            var items = new List<Item>
            {
                new("A", 5),
                new("D", 3),
                new("C", 4)
            };

            var promotions = new List<Promotion>();

            var multipleItemsPromotion =
                new MultipleItemsForFixedPricePromotion(10, "A", 3, 130);
            var percentageDiscountPromotion =
                new PercentageDiscountPromotion(15, "D", 10);
            var bundleItemsPromotion =
                new BundleItemsTogetherForFixedPricePromotion(50, new List<string> { "A", "D" }, 30);
           

            promotions.Add(multipleItemsPromotion);
            promotions.Add(bundleItemsPromotion);
            promotions.Add(percentageDiscountPromotion);

            // Act
            var resultPromotions = PromotionHelper.GetApplicablePromotions(items, promotions);

            // Assert
            Assert.AreEqual(2, resultPromotions.Count);
            Assert.AreEqual(1, resultPromotions.OfType<MultipleItemsForFixedPricePromotion>().Count());
            Assert.AreEqual(1, resultPromotions.OfType<PercentageDiscountPromotion>().Count());
            Assert.Contains("A", resultPromotions.OfType<MultipleItemsForFixedPricePromotion>().Select(r => r.SkuId)?.ToList());
            Assert.Contains("D", resultPromotions.OfType<PercentageDiscountPromotion>().Select(r => r.SkuId)?.ToList());
            Assert.AreEqual(0, resultPromotions.OfType<BundleItemsTogetherForFixedPricePromotion>().Count());
        }

        [Test]
        public void GetApplicablePromotions_Throws_Error_Same_PriorityId()
        {
            // Arrange
            var items = new List<Item>
            {
                new("A", 5),
                new("D", 3),
                new("C", 4)
            };
            var promotions = new List<Promotion>();
            var multipleItemsPromotion =
                new MultipleItemsForFixedPricePromotion(10, "A", 3, 130);
            var percentageDiscountPromotion =
                new PercentageDiscountPromotion(10, "D", 10);
            promotions.Add(multipleItemsPromotion);
            promotions.Add(percentageDiscountPromotion);

            // Assert
            Assert.Throws<ArgumentException>(() => PromotionHelper.GetApplicablePromotions(items, promotions));

        }

        [Test]
        public void GetApplicablePromotions_0_Promotions_0_Resulting_Promotions()
        {
            // Arange
            var items = new List<Item>
            {
                new("A", 5),
                new("D", 3),
                new("C", 4)
            };
            var promotions = new List<Promotion>();


            // Assert
            Assert.AreEqual(0, PromotionHelper.GetApplicablePromotions(items, promotions).Count);

        }




    }
}