using System;
using System.Collections.Generic;
using NUnit.Framework;
using PromotionEngine.Helpers;
using PromotionEngine.Models;
using PromotionEngine.Models.Promotions;

namespace PromotionEngineTests
{
    public class ApplyPromotionsHelper
    {
        /*
         * Scenario A
         */
        [Test]
        public void ApplyNoPromotions()
        {
            //arrange 
            var items = new List<Item>
            {
                new("A", 1),
                new("B", 1),
                new("C", 1)
            };

            var prices = new Dictionary<string, float>
            {
                {"A", 50},
                {"B", 30},
                {"C", 20}
            };

            var emptyPromotions = new List<Promotion>();

            //act 

            var resultPrice = PromotionHelper.TryApplyPromotions(items, prices, emptyPromotions);


            //assert
            Assert.AreEqual(100.0f, resultPrice);
        }

        /*
         * Scenario B 
         */
        [Test]
        public void ApplySimplePromotions()
        {
            //arrange 
            var items = new List<Item>
            {
                new("A", 5),
                new("B", 5),
                new("C", 1)
            };

            var prices = new Dictionary<string, float>
            {
                {"A", 50},
                {"B", 30},
                {"C", 20}
            };

            var promotions = new List<Promotion>
            {
                new MultipleItemsForFixedPricePromotion(10, "A", 3, 130),
                new MultipleItemsForFixedPricePromotion(20, "B", 2, 45),
            };

            //act 

            var resultPrice = PromotionHelper.TryApplyPromotions(items, prices, promotions);


            //assert
            Assert.AreEqual(370.0f, resultPrice);
        }

        /*
      * Scenario C 
      */
        [Test]
        public void Apply3Promotions()
        {
            //arrange 
            var items = new List<Item>
            {
                new("A", 3),
                new("B", 5),
                new("C", 1),
                new("D", 1)
            };

            var prices = new Dictionary<string, float>
            {
                {"A", 50},
                {"B", 30},
                {"C", 20},
                {"D", 15}
            };

            var promotions = new List<Promotion>
            {
                new MultipleItemsForFixedPricePromotion(10, "A", 3, 130),
                new MultipleItemsForFixedPricePromotion(20, "B", 2, 45),
                new BundleItemsTogetherForFixedPricePromotion(30, new List<string>{"C","D"}, 30),
            };

            //act 

            var resultPrice = PromotionHelper.TryApplyPromotions(items, prices, promotions);


            //assert
            Assert.AreEqual(280, resultPrice);
        }

        /*
   * Scenario C 
   */
        [Test]
        public void Apply4Promotions()
        {
            //arrange 
            var items = new List<Item>
            {
                new("A", 3),
                new("B", 5),
                new("C", 1),
                new("D", 1),
                new("E", 10)
            };

            var prices = new Dictionary<string, float>
            {
                {"A", 50},
                {"B", 30},
                {"C", 20},
                {"D", 15},
                {"E", 100}
            };

            var promotions = new List<Promotion>
            {
                new MultipleItemsForFixedPricePromotion(10, "A", 3, 130),
                new MultipleItemsForFixedPricePromotion(20, "B", 2, 45),
                new BundleItemsTogetherForFixedPricePromotion(30, new List<string>{"C","D"}, 30),
                new PercentageDiscountPromotion(40, "E", 10)
            };

            //act 

            var resultPrice = PromotionHelper.TryApplyPromotions(items, prices, promotions);


            //assert
            Assert.AreEqual(1180, resultPrice);
        }

    }
}
