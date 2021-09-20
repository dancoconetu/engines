using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PromotionEngine;
using PromotionEngine.Models;
using PromotionEngine.Models.Promotions;

namespace PromotionEngineTests
{
    public class ApplyPromotionsHelper
    {
        /*
         * Scenario 1 - no promotions
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
         * Scenario 2 
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

    }
}
