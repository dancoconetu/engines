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
        [Test]
        public void ApplySimplePromotions()
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

            var resultPrice = PromotionHelper.ApplyPromotions(items, prices, emptyPromotions);


            //assert
            Assert.AreEqual(100.0f, resultPrice);
        }

    }
}
