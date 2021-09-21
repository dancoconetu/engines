using System;
using System.Collections.Generic;
using System.Linq;
using PromotionEngine.Helpers;

namespace PromotionEngine.Models.Promotions
{

    /*
     * Purpose of the class is to bundle different items for a total price
     * 
     * Example:  C & D for 30
     */
    public class BundleItemsTogetherForFixedPricePromotion : IPromotion
    {
        public BundleItemsTogetherForFixedPricePromotion(int priority, List<string> skuIds, float totalPrice)
        {
            Priority = priority;
            SkuIds = skuIds;
            TotalPrice = totalPrice;
        }
        public int Priority { get; }
        public List<string> SkuIds { get; }
        public float TotalPrice { get; }
        public float CalculateTotal(List<Item> items, Dictionary<string, float> prices)
        {
            var bundledItems = items.Where(item => SkuIds.Contains(item.SkuId))?.ToList();
            if (bundledItems == null)
                throw new ArgumentException($"0 items for bundle promotion");

            var applicablePromotionTotal = items.Select(item => item.Quantity).Min();
            var promotionTotalPrice = applicablePromotionTotal * TotalPrice;

            var otherItemsPrice = 0.0f;
            foreach (var item in bundledItems)
            {
                var remainingQuantity = item.Quantity - applicablePromotionTotal;
                if (!prices.TryGetValue(item.SkuId, out var itemPrice))
                    throw new ArgumentException($"No price found for SkuId{item.SkuId}");
                otherItemsPrice += remainingQuantity * itemPrice;
            }
            var totalPrice = promotionTotalPrice + otherItemsPrice;
            return totalPrice;
        }
    }
}
