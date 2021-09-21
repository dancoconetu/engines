
using System;
using System.Collections.Generic;
using System.Linq;
using PromotionEngine.Helpers;

namespace PromotionEngine.Models.Promotions
{

    /*
     * Purpose of the class is to bundle multiple items of the same type/skuId for a total price
     * QuantityNeeded * SkuId for TotalPrice
     * Example: 3 * A for 130
     */
    public class MultipleItemsForFixedPricePromotion: IPromotion
    {
        public MultipleItemsForFixedPricePromotion(int priority, string skuId, int quantityNeeded,  float totalPrice)
        {
            QuantityNeeded = quantityNeeded;
            TotalPrice = totalPrice;
            Priority = priority;
            SkuId = skuId;
            SkuIds = new List<string> {SkuId};
        }
        public int QuantityNeeded { get; }
        public float TotalPrice { get; }
        public int Priority { get; }
        public string SkuId { get; }
        public List<string> SkuIds { get; }
        public float CalculateTotal(List<Item> items, Dictionary<string, float> prices)
        {
           
            var multiplePromotionItem = items.FirstOrDefault(item => item.SkuId == SkuId);
            if (multiplePromotionItem == null)
                throw new ArgumentException($"0 items for promotion with {SkuId}");
            if (!prices.TryGetValue(SkuId, out var price))
                throw new ArgumentException($"Price missing for {SkuId}");

            if (multiplePromotionItem.SkuId != SkuId)
                throw new ArgumentException("not equal sku ids for item and promotion");
            var applicablePromotionTotal =
                multiplePromotionItem.Quantity / QuantityNeeded;
            var nonApplicablePromotionTotal =
                multiplePromotionItem.Quantity % QuantityNeeded;
            var promotionTotalPrice = applicablePromotionTotal * TotalPrice;

            var otherItemsPrice = price * nonApplicablePromotionTotal;
            var totalPrice = promotionTotalPrice + otherItemsPrice;

            return totalPrice;
        }
    }
}
