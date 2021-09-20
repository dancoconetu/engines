using System;
using System.Collections.Generic;
using System.Linq;
using PromotionEngine.Models;
using PromotionEngine.Models.Promotions;

namespace PromotionEngine
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
    }
}
