
using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using PromotionEngine;
using PromotionEngine.Models;
using PromotionEngine.Models.Promotions;
using PromotionEngine.Repositories;

namespace PromotionEngineTests
{
    public class MainScenariosTest
    {
        private Controller _controller;

        [SetUp]
        public void Setup()
        {
            var pricesRepository = new Mock<IPriceRepository>();
            pricesRepository.Setup(x => x.GetItemPrices()).Returns(
                new Dictionary<string, float>
                {
                    { "A", 50},
                    { "B", 30},
                    { "C", 20},
                    { "D", 15}
                }
            );
            var activePromotions = new Mock<IPromotionRepository>();
            activePromotions.Setup(x => x.GetActivePromotions()).Returns(
                new List<IPromotion>()
                {
                    new MultipleItemsForFixedPricePromotion(10, "A", 3, 130),
                    new MultipleItemsForFixedPricePromotion(30, "B", 2, 45),
                    new BundleItemsTogetherForFixedPricePromotion(50, new List<string> {"C", "D"}, 30)
                }
            );

            _controller = new Controller(activePromotions.Object, pricesRepository.Object);
        }

        [Test]
        public void Scenario1Test()
        {
            //arrange
            var scenario1Items = new List<Item>
            {
                new("A", 1),
                new("B", 1),
                new("C", 1)
            };


            float expectedResult = 100;


            //act

            var actualResult = _controller.CalculateTotal(scenario1Items);

            //assert

            Assert.AreEqual(expectedResult, actualResult);

        }

        [Test]
        public void Scenario2Test()
        {
            var scenario2Items = new List<Item>
            {
                new("A", 5),
                new("B", 5),
                new("C", 1)
            };

            float expectedResult = 370;


            //act

            var actualResult = _controller.CalculateTotal(scenario2Items);

            //assert

            Assert.AreEqual(expectedResult, actualResult);

        }
        [Test]
        public void Scenario3Test()
        {
            //arrange

            var scenario3Items = new List<Item>
            {
                new("A", 3),
                new("B", 5),
                new("C", 1),
                new("D", 1)
            };

            float expectedResult = 280;

            //act

            var actualResult = _controller.CalculateTotal(scenario3Items);

            //assert

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void Modularity_Test_New_Promotion_HardCoded_Price_Result()
        {

            //3A + 2B applies percentage 50% of their combined price. Has top priority. 
            var newPromotion = new Mock<IPromotion>();
            newPromotion.Setup(p => p.SkuIds).Returns(new List<string> {"A","B"});
            newPromotion.Setup(p => p.Priority).Returns(1);
            var listItems = new List<Item>
            {
                new Item("A", 3),
                new Item("B", 2),
                new Item("C", 5)


            };
            var listPrices = new Dictionary<string, float>
            {
                {"A", 50},
                {"B", 30},
                {"C", 20},
                {"D", 15}
            };

            newPromotion.Setup(p=> p.CalculateTotal(It.IsAny<List<Item>>(), It.IsAny<Dictionary<string,float>>()
            )).Returns((List<Item> items, Dictionary<string, float> prices) => 105);

            var pricesRepository = new Mock<IPriceRepository>();
            pricesRepository.Setup(x => x.GetItemPrices()).Returns(
              listPrices
            );
            var activePromotions = new Mock<IPromotionRepository>();
            activePromotions.Setup(x => x.GetActivePromotions()).Returns(
                new List<IPromotion>()
                {
                    new MultipleItemsForFixedPricePromotion(10, "A", 3, 130),
                    new MultipleItemsForFixedPricePromotion(30, "B", 2, 45),
                    new BundleItemsTogetherForFixedPricePromotion(50, new List<string> {"C", "D"}, 30),
                    newPromotion.Object
                }
            );
            var newController = new Controller(activePromotions.Object, pricesRepository.Object);


            var expectedResult = 205;

            var actualResult = newController.CalculateTotal(listItems);

            Assert.AreEqual(expectedResult, actualResult );

        }


        //These test shows the modularity of the promotion engine, by creating a new promotion from the interface and adding it to the list of available promotions. 
        //The priority of this promotion is set to 1 so it's taken first over the other active promotions
        

        [Test]
        [TestCase(630, 10, 7,5)]
        [TestCase(280, 5, 3, 0)]
        [TestCase(660, 12, 8,3)]
        [TestCase(170, 3, 2,1)]
        [TestCase(100*150 + 20 * 100, 3*100, 2*100,100)]
        [TestCase(100, 0, 2,2)]
        public void Modularity_Test_Adding_Logic_To_CalculateTotal(float expectedResult, int quantA, int quantB, int quantC)
        {

            //3A + 2B applies are sold for 150 total. 
            var newPromotion = new Mock<IPromotion>();
            newPromotion.Setup(p => p.SkuIds).Returns(new List<string> { "A", "B" });
            newPromotion.Setup(p => p.Priority).Returns(1);
            var listItems = new List<Item>
            {
                new Item("A", quantA),
                new Item("B", quantB),
                new Item("C", quantC)


            };
            var listPrices = new Dictionary<string, float>
            {
                {"A", 50},
                {"B", 30},
                {"C", 20},
                {"D", 15}
            };


            //Setting the new calculate total for the new promotion: for each ( 3 * A and 2 * B) return 150
            newPromotion.Setup(p => p.CalculateTotal(It.IsAny<List<Item>>(), It.IsAny<Dictionary<string, float>>()
            )).Returns((List<Item> items, Dictionary<string, float> prices) =>
                {
                    var quantityA = items.FirstOrDefault(item => item.SkuId == "A")?.Quantity ?? 0;
                    var quantityB = items.FirstOrDefault(item => item.SkuId == "B")?.Quantity ?? 0;
                    var possiblePromotionsForA = quantityA / 3;
                    var possiblePromotionsForB = quantityB / 2;
                    var applyPromotionTimes = Math.Min(possiblePromotionsForA, possiblePromotionsForB);
                    var totalFor3A2B = 150;

                    var remainderItemsNormalPriceA = quantityA - 3 * applyPromotionTimes;
                    var remainderItemsNormalPriceB = quantityB - 2 * applyPromotionTimes;

                    var promotionPriceTotal = totalFor3A2B * applyPromotionTimes;
                    if (!prices.TryGetValue("A", out var priceA)) return 0;
                    if (!prices.TryGetValue("B", out var priceB)) return 0;

                    var totalPriceForA = priceA * remainderItemsNormalPriceA;
                    var totalPriceForB = priceB * remainderItemsNormalPriceB;

                    var totalPrice = totalPriceForA + totalPriceForB + promotionPriceTotal;
                    return totalPrice;
                }
            );

            var pricesRepository = new Mock<IPriceRepository>();
            pricesRepository.Setup(x => x.GetItemPrices()).Returns(
                listPrices
            );
            var activePromotions = new Mock<IPromotionRepository>();
            activePromotions.Setup(x => x.GetActivePromotions()).Returns(
                new List<IPromotion>()
                {
                    new MultipleItemsForFixedPricePromotion(10, "A", 3, 130),
                    new MultipleItemsForFixedPricePromotion(30, "B", 2, 45),
                    new BundleItemsTogetherForFixedPricePromotion(50, new List<string> {"C", "D"}, 30),
                    newPromotion.Object
                }
            );
            var newController = new Controller(activePromotions.Object, pricesRepository.Object);



            var actualResult = newController.CalculateTotal(listItems);

            Assert.AreEqual(expectedResult, actualResult);

        }
    }
}
