using System;
using System.Collections.Generic;
using System.Linq;
using PromotionEngine.Models;
using PromotionEngine.Models.Promotions;

namespace PromotionEngine.Helpers
{
    public class PromotionCalculatorHelper
    {
        public static float GetTotalForMultipleItemsPromotion(Item item , float price, MultipleItemsForFixedPricePromotion promotion)
        {
            
            if (item.SkuId != promotion.SkuId)
                throw new ArgumentException("not equal sku ids for item and promotion");
            var applicablePromotionTotal =
                item.Quantity / promotion.QuantityNeeded;
            var nonApplicablePromotionTotal =
                item.Quantity % promotion.QuantityNeeded;
            var promotionTotalPrice = applicablePromotionTotal * promotion.TotalPrice;
            
            var otherItemsPrice = price * nonApplicablePromotionTotal;
            var totalPrice = promotionTotalPrice + otherItemsPrice;

            return totalPrice;
        }


        public static float GetTotalForBundledItemsPromotion(List<Item> items, Dictionary<string,float> prices, BundleItemsTogetherForFixedPricePromotion promotion)
        {

            var applicablePromotionTotal = items.Select(item => item.Quantity).Min();
            var promotionTotalPrice = applicablePromotionTotal * promotion.TotalPrice;

            var otherItemsPrice = 0.0f;
            foreach (var item in items)
            {
                var remainingQuantity = item.Quantity - applicablePromotionTotal;
                if (!prices.TryGetValue(item.SkuId, out var itemPrice))
                    throw new ArgumentException($"No price found for SkuId{item.SkuId}");
                otherItemsPrice += remainingQuantity * itemPrice;
            }
            var totalPrice = promotionTotalPrice + otherItemsPrice;

            return totalPrice;
        }

        public static float GetTotalForPercentageDiscountItemsPromotion(Item item, float price,
            PercentageDiscountPromotion promotion)
        {
            if (item.SkuId != promotion.SkuId)
                throw new ArgumentException("not equal sku ids for item and promotion");

            return item.Quantity * price * (100 - promotion.Discount) / 100;
        }

    }
}
