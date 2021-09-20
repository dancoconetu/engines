
using System.Collections.Generic;
using PromotionEngine.Models;
using PromotionEngine.Models.Promotions;

namespace PromotionEngine.Repositories
{
    public class PromotionRepository : IPromotionRepository
    {
        public List<Promotion> GetActivePromotions()
        {
            var promotions = new List<Promotion>();

            var multipleItemsPromotion = 
                new MultipleItemsForFixedPricePromotion(10, "A", 3, 130);
            var multipleItemsPromotion2 = 
                new MultipleItemsForFixedPricePromotion(30, "B", 2, 45);
            var bundleItemsPromotion =
                new BundleItemsTogetherForFixedPricePromotion(50, new List<string> {"C", "D"}, 30);
            var percentageDiscountPromotion = 
                new PercentageDiscountPromotion(100, "D", 10);

            promotions.Add(multipleItemsPromotion);
            promotions.Add(multipleItemsPromotion2);
            promotions.Add(bundleItemsPromotion);
            promotions.Add(percentageDiscountPromotion);


            return promotions;
        }
    }
}
