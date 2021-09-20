using System.Collections.Generic;
using NUnit.Framework;
using PromotionEngine;
using PromotionEngine.Models;
using PromotionEngine.Repositories;

namespace PromotionEngineTests
{
    public class Tests
    {
        private DefaultEngine _promotionEngine;
        [SetUp]
        public void Setup()
        {
            _promotionEngine = new DefaultEngine(new PromotionRepository());
        }

        [Test]
        public void Test_Calculate_Total_Success()
        {
            var items = new List<Item>
            {
                new Item("A", 1),
                new Item("B", 1),
                new Item("C", 1)
            };

            var result = _promotionEngine.CalculateTotal(items);
            Assert.AreEqual(100.0f, result);
        }
    }
}