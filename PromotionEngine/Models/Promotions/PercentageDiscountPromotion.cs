using System;
using System.Collections.Generic;
using System.Linq;
using PromotionEngine.Helpers;

namespace PromotionEngine.Models.Promotions
{

    /*
     * Purpose of the class is to get a percentage for an sku id
     * returns discount percentage for an sku id
     * Example: A has 10% discount
    */
    public class PercentageDiscountPromotion: IPromotion
    {
        public PercentageDiscountPromotion(int priority, string skuId, float discount)
        {
            Priority = priority;
            SkuId = skuId;
            SkuIds = new List<string>{SkuId};
            Discount = discount;
        }
        public int Priority { get; }

        public string SkuId { get; }
        public List<string> SkuIds { get; }
        public float Discount { get; }


        
        public float CalculateTotal(List<Item> items, Dictionary<string, float> prices)
        {
            
            var percentageDiscountItem = items.FirstOrDefault(item => item.SkuId == SkuId);
            if (percentageDiscountItem == null)
                throw new ArgumentException($"0 items for promotion with {SkuId}");
            if (!prices.TryGetValue(SkuId, out var price))
                throw new ArgumentException($"Price missing for {SkuId}");

            if (percentageDiscountItem.SkuId != SkuId)
                throw new ArgumentException("not equal sku ids for item and promotion");

            var totalPrice = percentageDiscountItem.Quantity * price * (100 - Discount) / 100;

            return totalPrice;
        }
    }
}
