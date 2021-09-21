using System.Collections.Generic;
using NUnit.Framework;
using PromotionEngine.Models;
using PromotionEngine.Models.Promotions;

namespace PromotionEngineTests
{
    public class PromotionsCalculateTotal
    {
        [TestCase(230, "A", 5, 50, "A", 3, 130)]
        [TestCase(120, "B", 5, 30, "B", 2, 45)]
        [TestCase(225, "B", 10, 30, "B", 2, 45)]
        [TestCase(30, "B", 1, 30, "B", 2, 45)]
        [TestCase(45, "B", 2, 30, "B", 2, 45)]
        [TestCase(0, "B", 0, 30, "B", 2, 45)]
        public void MultipleItemsPromotionCalculation(float expectedResult,
            string itemSkuId,
            int itemQuantity,
            float itemPrice,
            string promotionSkuId,
            int promotionQuantity,
            float promotionTotalPrice)
        {
            
            var promotion =
                new MultipleItemsForFixedPricePromotion(1, promotionSkuId, promotionQuantity, promotionTotalPrice);
            var prices = new Dictionary<string, float>() {{itemSkuId, itemPrice}};
            var items = new List<Item> { new Item(itemSkuId, itemQuantity)};
         
            Assert.AreEqual(expectedResult,
                promotion.CalculateTotal(items, prices ));
        }


        [TestCaseSource(nameof(BundleMultipleItemsTestsData))]
        public void BundleMultipleItemsPromotionCalculatorTest(int expectedResult, List<Item> items,
            Dictionary<string, float> prices,
            BundleItemsTogetherForFixedPricePromotion promotion)
        {
            Assert.AreEqual(expectedResult, promotion.CalculateTotal(items, prices));
        }


        [Test]
        [TestCase(225, "A", 5, 50, "A", 10)]
        [TestCase(120, "B", 5, 30, "B", 20)]
        [TestCase(210, "B", 10, 30, "B", 30)]
        [TestCase(18, "B", 1, 30, "B", 40)]
        [TestCase(30, "B", 2, 30, "B", 50)]
        [TestCase(528, "B", 88, 30, "B", 80)]
        public void DiscountPromotionOnItemsCalculation(float expectedResult,
            string itemSkuId,
            int itemQuantity,
            float itemPrice,
            string promotionSkuId,
            float promotionDiscount)
        {
            var item = new Item(itemSkuId, itemQuantity);
            var promotion =
                new PercentageDiscountPromotion(1, promotionSkuId, promotionDiscount);
            var prices = new Dictionary<string, float>() { { itemSkuId, itemPrice } };
            var items = new List<Item> { new Item(itemSkuId, itemQuantity) };
            Assert.AreEqual(expectedResult,
                promotion.CalculateTotal(items, prices));
        }

        public static IEnumerable<TestCaseData> BundleMultipleItemsTestsData
        {
            get
            {
                var itemPrices = new Dictionary<string, float>
                {
                    {"A", 50},
                    {"B", 30},
                    {"C", 20},
                    {"D", 15}
                };
                yield return new TestCaseData(
                    140,
                    new List<Item>
                    {
                        new("C", 5),
                        new("D", 4)
                    },
                    itemPrices,
                    new BundleItemsTogetherForFixedPricePromotion(1, new List<string> { "C", "D" }, 30)
                );
                yield return new TestCaseData(
                    600,
                    new List<Item>
                    {
                        new("A", 10),
                        new("B", 5)
                    },
                    itemPrices,
                    new BundleItemsTogetherForFixedPricePromotion(1, new List<string> { "A", "B" }, 70)
                );
                yield return new TestCaseData(
                    340,
                    new List<Item>
                    {
                        new("B", 5),
                        new("C", 4),
                        new("D", 10),
                    },
                    itemPrices,
                    new BundleItemsTogetherForFixedPricePromotion(1, new List<string> { "B","C", "D" }, 55)
                );
                yield return new TestCaseData(
                    10730,
                    new List<Item>
                    {
                        new("A", 200),
                        new("B", 15),
                        new("C", 14),
                        new("D", 10)
                    },
                    itemPrices,
                    new BundleItemsTogetherForFixedPricePromotion(1, new List<string> { "A","B","C", "D" }, 100)
                );
                yield return new TestCaseData(
                    0,
                    new List<Item>
                    {
                        new("A", 0),
                        new("B", 0),
                        new("C", 0),
                        new("D", 0)
                    },
                    itemPrices,
                    new BundleItemsTogetherForFixedPricePromotion(1, new List<string> { "A", "B", "C", "D" }, 100)
                );
            }
        }
    }



}