
using NUnit.Framework;
using PromotionEngine;
using PromotionEngine.Models;
using PromotionEngine.Models.Promotions;

namespace PromotionEngineTests
{
    public class PromotionCalculatorHelperTests
    {
        [Test]
        [TestCase(230, "A", 5, 50, "A", 3, 130) ]
        [TestCase(120, "B", 5, 30, "B", 2, 45) ]
        [TestCase(225, "B", 10, 30, "B", 2, 45) ]
        [TestCase(30, "B", 1, 30, "B", 2, 45) ]
        [TestCase(45, "B", 2, 30, "B", 2, 45) ]
        [TestCase(0, "B", 0, 30, "B", 2, 45) ]
      
        public void MultipleItemsPromotionCalculation(float expectedResult, 
            string itemSkuId, 
            int itemQuantity, 
            float itemPrice, 
            string promotionSkuId, 
            int promotionQuantity, 
            float promotionTotalPrice )
        {

            var item = new Item(itemSkuId, itemQuantity);
            var promotion =
                new MultipleItemsForFixedPricePromotion(1, promotionSkuId, promotionQuantity, promotionTotalPrice);

            Assert.AreEqual(expectedResult, PromotionCalculatorHelper.GetTotalForMultipleItemsPromotion(item, itemPrice, promotion));
        }
    }
}
